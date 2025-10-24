namespace UniTemplate.DI
{
    using VContainer;
    using Object = UnityEngine.Object;

    public static class DIExtensions
    {
        private static SceneScope? CurrentSceneContext;

        /// <summary>
        ///     Get current scene <see cref="IDependencyContainer"/>
        /// </summary>
        public static IDependencyContainer GetCurrentContainer()
        {
            if(CurrentSceneContext == null)
                CurrentSceneContext = Object.FindObjectOfType<SceneScope>();
            return CurrentSceneContext.Container.Resolve<IDependencyContainer>();
        }

        /// <inheritdoc cref="GetCurrentContainer()"/>
        public static IDependencyContainer GetCurrentContainer(this object _)
        {
            return GetCurrentContainer();
        }
    }
}