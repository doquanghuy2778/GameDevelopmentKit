namespace GameFoundationCore.Scripts.Extension
{
    using GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Base.Presenter;
    using GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Signals;
    using GameFoundationCore.DI;
    using GameFoundationCore.Signals;
    using VContainer;

    public static class ExtensionMethod
    {
        public static void InitScreenManually<T>(this IContainerBuilder builder) where T : IScreenPresenter
        {
            builder.RegisterBuildCallback(container => container.Resolve<SignalTransmitter>().Fire(new InitScreenManualSignal
                { ScreenPresenter = container.Instantiate<T>() }));
        }
    }
}