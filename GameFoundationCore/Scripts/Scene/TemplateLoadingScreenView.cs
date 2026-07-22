using ILogServices = GameFoundationCore.LogServices.ILogServices;
using SignalTransmitter = GameFoundationCore.Signals.SignalTransmitter;

namespace GameDevelopmentKit.GameFoundationCore.Scene
{
    using Cysharp.Threading.Tasks;
    using GameDevelopmentKit.GameFoundationCore.AssetsManager;
    using GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Base.Presenter;
    using GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Base.View;
    using GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Manager;
    using TMPro;
    using UnityEngine;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.UI;

    public class TemplateLoadingScreenView : BaseView
    {
        [field: SerializeField] public Slider          LoadingSlider       { get; set; }
        [field: SerializeField] public TextMeshProUGUI LoadingProgressText { get; set; }

        private float  visibleProgress;
        public  float  Progress    { get; set; }
        public  string LoadingText { get; set; }

        public void Update()
        {
            this.visibleProgress = Mathf.Lerp(this.visibleProgress, this.Progress, Time.unscaledDeltaTime * 5f);
            if (this.LoadingSlider is { })
            {
                this.LoadingSlider.value = this.visibleProgress;
            }
            if (this.LoadingProgressText is { } && this.LoadingText is { })
            {
                this.LoadingProgressText.text = string.Format(this.LoadingText, Mathf.RoundToInt(this.visibleProgress * 100));
            }
        }

        public UniTask CompleteLoading()
        {
            this.Progress = 1f;
            return UniTask.WaitUntil(() => this.visibleProgress >= .999f);
        }
    }

    [ScreenInfo(nameof(TemplateLoadingScreenView))]
    public class TemplateLoadingScreenPresenter : BaseScreenPresenter<TemplateLoadingScreenView>
    {
        private readonly IGameAssets gameAssets;

        protected TemplateLoadingScreenPresenter(
            SignalTransmitter signalTransmitter,
            ILogServices      logServices,
            IGameAssets       gameAssets
        ) : base(signalTransmitter, logServices)
        {
            this.gameAssets = gameAssets;
        }

        private float loadingProgress;
        private int   loadingSteps;

        protected virtual string NextSceneName => "1.MainScene";

        private float LoadingProgress
        {
            get => this.loadingProgress;
            set
            {
                this.loadingProgress = value;
                if (value / this.loadingSteps <= this.View.Progress) return;
                this.View.Progress = value / this.loadingSteps;
            }
        }

        protected virtual string GetLoadingText() { return "Loading {0}%"; }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.LoadingText = this.GetLoadingText();
            this.OpenViewAsync().Forget();
        }

        public override UniTask BindData()
        {
            this.LoadingProgress = 0;
            this.loadingSteps    = 1;
            UniTask.WhenAll(this.Preload()).ContinueWith(this.LoadNextScene).Forget();
            return UniTask.CompletedTask;
        }

        protected virtual async UniTask LoadNextScene()
        {
            SceneDirector.CurrentSceneName = this.NextSceneName;
            await this.View.CompleteLoading();
            await this.TrackProgress(this.LoadSceneAsync());
        }

        protected virtual UniTask Preload()
        {
            return UniTask.CompletedTask;
        }

        protected virtual AsyncOperationHandle<SceneInstance> LoadSceneAsync()
        {
            return this.gameAssets.LoadSceneAsync(this.NextSceneName);
        }

        protected UniTask<T> TrackProgress<T>(AsyncOperationHandle<T> handle)
        {
            ++this.loadingSteps;
            var localLoadingProgress = 0f;

            void UpdateProgress(float progress)
            {
                this.LoadingProgress += progress - localLoadingProgress;
                localLoadingProgress =  progress;
            }

            return handle.ToUniTask(Progress.CreateOnlyValueChanged<float>(UpdateProgress))
                .ContinueWith(result =>
                {
                    UpdateProgress(1f);
                    return result;
                });
        }
    }
}