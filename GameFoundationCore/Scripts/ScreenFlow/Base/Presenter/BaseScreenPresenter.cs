namespace GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Base.Presenter
{
    using Cysharp.Threading.Tasks;
    using GameDevelopmentKit.GameFoundationCore.Scripts.MVP;
    using global::GameFoundationCore.LogServices;
    using global::GameFoundationCore.Scripts.ScreenFlow.Base.View;
    using global::GameFoundationCore.Signals;
    using UnityEngine;

    public class BaseScreenPresenter<TView> : IScreenLifecycle where TView : IScreenView
    {

        #region Inject

        protected SignalTransmitter signalTransmitter;
        protected ILogServices      logServices;

        protected BaseScreenPresenter(
            SignalTransmitter signalTransmitter,
            ILogServices      logServices
        )
        {
            this.signalTransmitter = signalTransmitter;
            this.logServices       = logServices;
        }

        #endregion

        public TView        View     { get; private set; }
        public string       ScreenId { get; private set; }
        public ScreenStatus ScreenStatus   { get; set; } = ScreenStatus.Closed;
        
        public void SetView(IUIView viewInstance)
        {
            this.View     = (TView)viewInstance;
            this.ScreenId = ScreenHelper.GetScreenId<TView>();
            if (!this.View.IsReadyToUse) UniTask.WaitUntil(this, state => state.View.IsReadyToUse).Forget();
            this.OnViewReady();
        }

        public void SetViewParent(Transform parent)
        {
            if(parent == null)
            {
                this.logServices.LogWithColor($"parent {parent.name} is null", Color.red);
                return;
            }
            if (this.View.Equals(null)) return;
            this.View.RectTransform.SetParent(parent);
        }

        public Transform GetViewParent()
        {
            return this.View.RectTransform.parent;
        }

        public virtual UniTask BindData()
        {
            return UniTask.CompletedTask;
        }

        public virtual async UniTask OpenViewAsync()
        {
            //Fill data for screen first
            await this.BindData();

            if (this.ScreenStatus == ScreenStatus.Opened) return;
            this.ScreenStatus = ScreenStatus.Opened;
            await this.View.Open();
        }

        public virtual async UniTask CloseViewAsync()
        {
            if (this.ScreenStatus == ScreenStatus.Closed) return;
            this.ScreenStatus = ScreenStatus.Closed;
            await this.View.Close();
            this.Dispose();
        }

        public virtual void CloseView()
        {
            this.CloseViewAsync().Forget();
        }

        public virtual void HideView()
        {
            if(this.ScreenStatus == ScreenStatus.Closed || this.ScreenStatus == ScreenStatus.Hide) return;
            this.ScreenStatus = ScreenStatus.Hide;
            this.View.Hide();
            this.Dispose();
        }

        public virtual void DestroyView()
        {
            if(this.ScreenStatus == ScreenStatus.Destroyed) return;
            this.ScreenStatus = ScreenStatus.Destroyed;
            if (this.View.Equals(null)) return;
            this.Dispose();
            this.View.DestroySelf();
        }

        protected virtual void OnViewReady()
        {

        }

        protected virtual void Dispose()
        {

        }
    }
}