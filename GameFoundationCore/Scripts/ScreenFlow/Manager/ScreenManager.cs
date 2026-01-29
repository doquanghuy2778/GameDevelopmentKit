namespace GameFoundationCore.ScreenFlow.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using GameDevelopmentKit.GameFoundationCore.AssetsManager;
    using GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Base.Presenter;
    using GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Manager;
    using GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Signals;
    using GameFoundationCore.DI;
    using GameFoundationCore.LogServices;
    using GameFoundationCore.Scripts.Extension;
    using GameFoundationCore.Scripts.ScreenFlow.Base.View;
    using GameFoundationCore.Signals;
    using R3;
    using UnityEngine;
    using VContainer.Unity;
    using IInitializable = GameFoundationCore.DI.IInitializable;
    using ITickable = GameFoundationCore.DI.ITickable;
    using Object = UnityEngine.Object;

    public interface IScreenManager
    {
        /// <summary>
        /// Open Screen
        /// </summary>
        /// <typeparam name="TPresenter"></typeparam>
        /// <returns></returns>
        public UniTask<TPresenter> OpenScreen<TPresenter>() where TPresenter : IScreenPresenter;

        /// <summary>
        /// Open Screen with model
        /// </summary>
        /// <param name="model"></param>
        /// <typeparam name="TPresenter"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public UniTask<TPresenter> OpenScreen<TPresenter, TModel>(TModel model) where TPresenter : IScreenPresenter<TModel>;

        /// <summary>
        /// Close current screen on top
        /// </summary>
        /// <returns></returns>
        public UniTask CloseCurrentScreen();

        /// <summary>
        /// Close all screen in queue async
        /// </summary>
        /// <returns></returns>
        public UniTask CloseAllScreenAsync();

        /// <summary>
        /// Close all screen in queue
        /// </summary>
        public void CloseAllScreen();

        /// <summary>
        /// Close current screen with name
        /// </summary>
        /// <typeparam name="TPresenter"></typeparam>
        /// <returns></returns>
        public UniTask<TPresenter> CloseCurrentScreenWithName<TPresenter>() where TPresenter : IScreenPresenter;

    }

    public class ScreenManager : IScreenManager, ITickable, IInitializable, IDisposable
    {
        #region Constructor

        private readonly IGameAssets       gameAssets;
        private readonly SignalTransmitter signalTransmitter;
        private readonly ILogServices      logger;

        private ScreenManager(IGameAssets gameAssets,
            SignalTransmitter signalTransmitter,
            ILogServices      logger)
        {
            this.gameAssets        = gameAssets;
            this.signalTransmitter = signalTransmitter;
            this.logger            = logger;

            this.signalTransmitter.Subscribe<InitScreenManualSignal>(this.OnManualInitScreen);
        }

        #endregion

        private readonly List<IScreenPresenter>                   activeScreens               = new();
        private readonly Dictionary<Type, IScreenPresenter>       typeToLoadedScreenPresenter = new();
        private readonly Dictionary<Type, Task<IScreenPresenter>> typeToPendingScreen         = new();

        public  ReactiveProperty<IScreenPresenter> CurrentActiveScreen { get; } = new();
        private RootUICanvas                       rootUICanvas;

        public RootUICanvas RootUICanvas
        {
            get
            {
                if (!this.rootUICanvas) this.rootUICanvas = Object.FindObjectOfType<RootUICanvas>();
                return this.rootUICanvas;
            }
        }

        public  Transform        CurrentRootScreen  => this.RootUICanvas.RootUIShowTransform;
        public  Transform        CurrentHiddenRoot  => this.RootUICanvas.RootUIClosedTransform;
        public  Transform        CurrentOverlayRoot => this.RootUICanvas.RootUIOverlayTransform;
        private IScreenPresenter previousActiveScreen;

        #region Implenetation IScreenManager

        public async UniTask<T> OpenScreen<T>() where T : IScreenPresenter
        {
            var nextScreen = await this.GetScreen<T>();

            if (nextScreen != null)
            {
                await nextScreen.OpenViewAsync();

                return nextScreen;
            }
            else
            {
                Debug.LogError($"The {typeof(T).Name} screen does not exist");
                return default;
            }
        }

        public UniTask<TPresenter> OpenScreen<TPresenter, TModel>(TModel model) where TPresenter : IScreenPresenter<TModel>
        {
            throw new System.NotImplementedException();
        }

        public async UniTask CloseCurrentScreen()
        {
            if (this.activeScreens.Count > 0)
            {
                await this.activeScreens.Last().CloseViewAsync();
            }
        }

        public async UniTask CloseAllScreenAsync()
        {
            var tasks              = new List<UniTask>();
            var cacheActiveScreens = this.activeScreens.ToList();
            this.activeScreens.Clear();

            foreach (var screen in cacheActiveScreens) tasks.Add(screen.CloseViewAsync());

            this.CurrentActiveScreen.Value = null;
            this.previousActiveScreen      = null;

            await UniTask.WhenAll(tasks);
        }

        public void CloseAllScreen()
        {
            var cacheActiveScreens = this.activeScreens.ToList();
            this.activeScreens.Clear();

            foreach (var screen in cacheActiveScreens) screen.CloseViewAsync().Forget();

            this.CurrentActiveScreen.Value = null;
            this.previousActiveScreen      = null;
        }

        #endregion

        public void Tick()
        {
            if (this.activeScreens.Count > 1)
            {
                Debug.Log("Close last screen");
                this.activeScreens.Last().CloseViewAsync();
            }
        }

        public void Initialize()
        {
            Debug.Log("Initialize Screen Manager");
        }

        public void Dispose()
        {
            Debug.Log("Dispose Screen Manager");
        }

        public async UniTask<T> GetScreen<T>() where T : IScreenPresenter
        {
            var screenType = typeof(T);

            return (T)await this.GetScreen(screenType);
        }

        public async UniTask<IScreenPresenter> GetScreen(Type screenType)
        {
            if (this.typeToLoadedScreenPresenter.TryGetValue(screenType, out var screenPresenter)) return screenPresenter;

            if (!this.typeToPendingScreen.TryGetValue(screenType, out var loadingTask))
            {
                loadingTask = InstantiateScreen();
                this.typeToPendingScreen.Add(screenType, loadingTask);
            }

            var result = await loadingTask;
            this.typeToPendingScreen.Remove(screenType);

            return result;

            async Task<IScreenPresenter> InstantiateScreen()
            {
                screenPresenter = this.GetCurrentContainer().Instantiate(screenType) as IScreenPresenter;
                var screenInfo = screenPresenter.GetCustomAttribute<ScreenInfoAttribute>();

                var viewObject = Object.Instantiate(await this.gameAssets.LoadAssetAsync<GameObject>(screenInfo.AddressableScreenPath),
                    this.CheckPopupIsOverlay(screenPresenter) ? this.CurrentOverlayRoot : this.CurrentRootScreen).GetComponent<IScreenView>();

                screenPresenter.SetView(viewObject);
                this.typeToLoadedScreenPresenter.Add(screenType, screenPresenter);

                return screenPresenter;
            }
        }

        private void OnManualInitScreen(InitScreenManualSignal signal)
        {
            var screenPresenter = signal.ScreenPresenter;
            var screenType      = screenPresenter.GetType();

            if (!this.typeToLoadedScreenPresenter.TryAdd(screenType, screenPresenter)) return;
            var screenInfo = screenPresenter.GetCustomAttribute<ScreenInfoAttribute>();

            var viewObj = this.CurrentRootScreen.Find(screenInfo.AddressableScreenPath);

            if (viewObj != null)
            {
                screenPresenter.SetView(viewObj.GetComponent<IScreenView>());
            }
            else
            {
                this.logger.LogError($"The {screenInfo.AddressableScreenPath} object may be not instantiated in the RootUICanvas!!!");
            }
        }

        public async UniTask<TPresenter> CloseCurrentScreenWithName<TPresenter>() where TPresenter : IScreenPresenter
        {
            var screenClose = await this.GetScreen<TPresenter>();

            if (screenClose != null && this.activeScreens.Contains(screenClose))
            {
                await screenClose.CloseViewAsync();
                return screenClose;
            }
            else
            {
                this.logger.LogError($"The {typeof(TPresenter).Name} screen does not exist");
                return default;
            }
        }

        #region Check Overlay Popup

        private bool CheckScreenIsPopup(IScreenPresenter screenPresenter)
        {
            return screenPresenter.GetType().IsSubclassOfRawGeneric(typeof(BaseScreenPresenter<>));
        }

        private bool CheckPopupIsOverlay(IScreenPresenter screenPresenter)
        {
            return this.CheckScreenIsPopup(screenPresenter) && screenPresenter.GetCustomAttribute<PopupInfoAttribute>().IsOverlay;
        }

        #endregion
    }
}