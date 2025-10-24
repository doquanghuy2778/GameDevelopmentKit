namespace UniTemplate.Signals
{
    using MessagePipe;
    using UniTemplate.DI;
    using VContainer;

    public static class SignalTransmitterVcontainer
    {
        private static readonly MessagePipeOptions MessagePipeOptions = new();

        public static void RegisterSignalBus(this IContainerBuilder builder)
        {
            builder.Register<SignalTransmitter>(Lifetime.Scoped).AsInterfacesAndSelf();
            builder.RegisterMessagePipe();
        }

        public static void DeclareSignal<TSignal>(this IContainerBuilder builder)
        {
            builder.RegisterMessageBroker<TSignal>(MessagePipeOptions);
        }
    }
}