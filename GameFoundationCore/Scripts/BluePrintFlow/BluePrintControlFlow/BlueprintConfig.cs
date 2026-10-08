namespace GameFoundationCore.Scripts.BluePrintFlow.BluePrintControlFlow
{
    using UnityEngine;
    using IGameConfig = GameFoundationCore.DI.Models.Interfaces.IGameConfig;

    [ CreateAssetMenu(fileName = "BlueprintConfig", menuName = "GameFoundationCore/BlueprintConfig", order = 1)]
    public class BlueprintConfig : ScriptableObject, IGameConfig
    {
        [SerializeField] private string currentBlueprintVersion = "0.0.1";
        [SerializeField] private bool   isResourceMode          = true;
        [SerializeField] private string resourceBlueprintPath   = "BlueprintData/";
        [SerializeField] private string blueprintFileType       = ".csv";

        private string persistentDataPath;

        public bool   IsResourceMode        => this.isResourceMode;
        public string ResourceBlueprintPath => this.resourceBlueprintPath;
        public string BlueprintFileType     => this.blueprintFileType;

        private void OnEnable()
        {
            this.persistentDataPath = Application.persistentDataPath;
        }
    }
}