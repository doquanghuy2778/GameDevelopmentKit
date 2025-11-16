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
    using GameFoundationCore.DI;
    using GameFoundationCore.Scripts.Extension;
    using GameFoundationCore.Scripts.ScreenFlow.Base.View;
    using R3;
    using UnityEngine;
    using VContainer.Unity;
    using Object = UnityEngine.Object;

    public interface IScreenManager
    {
        public UniTask<TPresenter> OpenScreen<TPresenter>() where TPresenter : IScreenPresenter;
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

    }

    public class ScreenManager : IScreenManager, ITickable, IInitializable, IDisposable
    {
        #region Constructor

        private readonly IGameAssets gameAssets;

        private ScreenManager(IGameAssets gameAssets)
        {
            this.gameAssets = gameAssets;
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
            throw new System.NotImplementedException();
        }

        #endregion
        
        public void Tick()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
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