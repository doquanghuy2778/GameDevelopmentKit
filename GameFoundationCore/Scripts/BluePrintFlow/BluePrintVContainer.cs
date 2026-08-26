namespace GameFoundationCore.Scripts.BluePrintFlow
{
    using BlueprintFlow.BlueprintReader;
    using GameFoundationCore.DI;
    using GameFoundationCore.DI.Models;
    using GameFoundationCore.Scripts.BluePrintFlow.BluePrintControlFlow;
    using GameFoundationCore.Scripts.BluePrintFlow.Signal;
    using GameFoundationCore.Scripts.Utilities.Extension;
    using GameFoundationCore.Signals;
    using VContainer;

    public static class BluePrintVContainer
    {
        public static void RegisterBlueprints(this IContainerBuilder builder)
        {
            builder.Register<BlueprintReaderManager>(Lifetime.Singleton);
            builder.Register(container => container.Resolve<GDKConfig>().GetGameConfig<BlueprintConfig>(), Lifetime.Singleton);

            typeof(IGenericBlueprintReader).GetDerivedTypes().ForEach(type => builder.Register(type, Lifetime.Singleton).AsInterfacesAndSelf());

            builder.DeclareSignal<IProgressPercent.ReadBlueprintProgressSignal>();
            builder.DeclareSignal<IProgressPercent.LoadBlueprintDataProgressSignal>();
            builder.DeclareSignal<IProgressPercent.LoadBlueprintDataSucceedSignal>();
        }
    }
}