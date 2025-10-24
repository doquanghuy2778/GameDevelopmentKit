namespace UniTemplate.ScreenFlow.Manager
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Cysharp.Threading.Tasks;
    using UniTemplate.AssetsManager;
    using UniTemplate.ScreenFlow.Base;
    using VContainer.Unity;

    public interface IScreenManager
    {
        public UniTask<TPresenter> OpenScreen<TPresenter>() where TPresenter : IScreenLifecycle;
        public UniTask<TPresenter> OpenScreen<TPresenter, TModel>(TModel model) where TPresenter : IScreenLifecycle<TModel>;

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

        private readonly List<IScreenLifecycle>                   activeScreens       = new();
        private readonly Dictionary<Type, IScreenLifecycle>       typeToLoadScreen    = new();
        private readonly Dictionary<Type, Task<IScreenLifecycle>> typeToPendingScreen = new();

        public UniTask<TPresenter> OpenScreen<TPresenter>() where TPresenter : IScreenLifecycle
        {
            throw new System.NotImplementedException();
        }

        public UniTask<TPresenter> OpenScreen<TPresenter, TModel>(TModel model) where TPresenter : IScreenLifecycle<TModel>
        {
            throw new System.NotImplementedException();
        }

        public UniTask CloseCurrentScreen()
        {
            throw new System.NotImplementedException();
        }

        public UniTask CloseAllScreenAsync()
        {
            throw new System.NotImplementedException();
        }

        public void CloseAllScreen()
        {
            throw new System.NotImplementedException();
        }

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
    }
}