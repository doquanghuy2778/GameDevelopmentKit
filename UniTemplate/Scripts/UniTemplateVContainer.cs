namespace UniTemplate.Scripts
{
    using UniTemplate.AssetsManager;
    using UniTemplate.DI;
    using UniTemplate.LogServices;
    using UniTemplate.ScreenFlow.Manager;
    using UniTemplate.Signals;
    using VContainer;

    public static class UniTemplateVContainer
    {
        public static void RegisterUniTemplateVContainer(this IContainerBuilder builder)
        {
            builder.Register<VcontainerWrapper>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterSignalBus();
            builder.Register<GameAssets>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ScreenManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LogServicesManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}