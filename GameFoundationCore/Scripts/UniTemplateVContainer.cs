namespace GameFoundationCore.Scripts
{
    using GameDevelopmentKit.GameFoundationCore.AssetsManager;
    using GameDevelopmentKit.GameFoundationCore.ObjectPooling;
    using GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Signals;
    using GameFoundationCore.DI;
    using GameFoundationCore.DI.Models;
    using GameFoundationCore.LogServices;
    using GameFoundationCore.ScreenFlow.Manager;
    using GameFoundationCore.Scripts.BluePrintFlow;
    using GameFoundationCore.Signals;
    using UnityEngine;
    using VContainer;

    public static class GameFoundationCoreVContainer
    {
        public static void RegisterGameFoundationCoreVContainer(this IContainerBuilder builder)
        {
            builder.Register<VcontainerWrapper>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<VContainerAdapter>(Lifetime.Scoped).AsImplementedInterfaces();

            builder.RegisterSignalBus();
            builder.RegisterBlueprints();
            builder.Register<GameAssets>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<ObjectPoolingManager>(Lifetime.Singleton);
            builder.Register<ScreenManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LogServicesManager>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterInstance(Resources.Load<GDKConfig>("GameConfig/GDKConfig/GDKConfig"));

            builder.DeclareSignal<InitScreenManualSignal>();
        }
    }
}