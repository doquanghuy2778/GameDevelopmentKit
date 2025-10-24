namespace GameFoundationCore.Scripts
{
    using GameFoundationCore.AssetsManager;
    using GameFoundationCore.DI;
    using GameFoundationCore.LogServices;
    using GameFoundationCore.ScreenFlow.Manager;
    using GameFoundationCore.Signals;
    using VContainer;

    public static class GameFoundationCoreVContainer
    {
        public static void RegisterGameFoundationCoreVContainer(this IContainerBuilder builder)
        {
            builder.Register<VcontainerWrapper>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterSignalBus();
            builder.Register<GameAssets>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ScreenManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LogServicesManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }
    }
}