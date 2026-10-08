namespace GameFoundationCore.Scripts.BluePrintFlow.BluePrintControlFlow
{
    using System.Collections.Generic;
    using BlueprintFlow.BlueprintReader;
    using Cysharp.Threading.Tasks;
    using GameFoundationCore.LogServices;
    using GameFoundationCore.Scripts.BluePrintFlow.Signal;
    using GameFoundationCore.Scripts.Extension;
    using GameFoundationCore.Signals;
    using UnityEngine;

    public class BlueprintReaderManager
    {
        #region Inject

        private readonly SignalTransmitter                            signalTransmitter;
        private readonly ILogServices                                 logServices;
        private readonly BlueprintConfig                              blueprintConfig;
        private readonly IReadOnlyCollection<IGenericBlueprintReader> blueprints;

        public BlueprintReaderManager(
            SignalTransmitter                            signalTransmitter,
            ILogServices                                 logServices,
            BlueprintConfig                              blueprintConfig,
            IReadOnlyCollection<IGenericBlueprintReader> blueprints
            )
        {
            this.signalTransmitter = signalTransmitter;
            this.logServices       = logServices;
            this.blueprintConfig   = blueprintConfig;
            this.blueprints        = blueprints;
        }

        #endregion

        private readonly IProgressPercent.ReadBlueprintProgressSignal readBlueprintProgressSignal = new();

        /// <summary>
        /// Load all blueprints from Resources folder.
        /// This is the simplified OFFLINE MODE implementation.
        /// </summary>
        public virtual async UniTask LoadBlueprint()
        {
            this.logServices.Log("[BlueprintReader] Start loading (OFFLINE MODE)");

            this.readBlueprintProgressSignal.MaxBlueprint    = this.blueprints.Count;
            this.readBlueprintProgressSignal.CurrentProgress = 0;
            this.signalTransmitter.Fire(this.readBlueprintProgressSignal);

            await UniTask.WhenAll(this.blueprints.Select(this.LoadBlueprintFromResources));

            this.logServices.Log("[BlueprintReader] All blueprints loaded successfully");
            this.signalTransmitter.Fire<IProgressPercent.LoadBlueprintDataSucceedSignal>();
        }

        /// <summary>
        /// Load a single blueprint from Resources folder.
        /// </summary>
        private async UniTask LoadBlueprintFromResources(IGenericBlueprintReader blueprintReader)
        {
            var bpAttribute = blueprintReader.GetCustomAttribute<BlueprintReaderAttribute>();

            if (bpAttribute == null)
            {
                this.logServices.LogWarning($"[BlueprintReader] Class {blueprintReader.GetType().Name} does not have BlueprintReaderAttribute");
                return;
            }

            // Skip Server-scoped blueprints
            if (bpAttribute.BlueprintScope == BlueprintScope.Server) return;

            await UniTask.SwitchToMainThread();

            // Build resource path
            var resourcePath = $"{this.blueprintConfig.ResourceBlueprintPath}{bpAttribute.DataPath}";

            // Load TextAsset from Resources
            var textAsset = (TextAsset)await Resources.LoadAsync<TextAsset>(resourcePath);

            await UniTask.SwitchToThreadPool();

            if (textAsset != null)
            {
                // Deserialize CSV content to blueprint
                await blueprintReader.DeserializeFromCsv(textAsset.text);
                this.logServices.Log($"[BlueprintReader] Loaded: {bpAttribute.DataPath}");

                lock (this.readBlueprintProgressSignal)
                {
                    this.readBlueprintProgressSignal.CurrentProgress++;
                    this.signalTransmitter.Fire(this.readBlueprintProgressSignal);
                }
            }
            else
            {
                this.logServices.LogWarning($"[BlueprintReader] Resource not found: {resourcePath}");
            }
        }
    }
}