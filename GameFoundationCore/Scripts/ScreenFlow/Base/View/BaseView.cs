namespace GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Base.View
{
    using System;
    using Cysharp.Threading.Tasks;
    using global::GameFoundationCore.Scripts.ScreenFlow.Base.View;
    using UnityEngine;

    [RequireComponent(typeof(CanvasGroup))]
    public class BaseView : MonoBehaviour, IScreenView
    {
        [SerializeField] private CanvasGroup viewRoot;

        protected virtual CanvasGroup        ViewRoot         { get => this.viewRoot; set => this.viewRoot = value; }

        public RectTransform RectTransform { get; private set; }
        public bool          IsReadyToUse  { get; private set; }
        public event Action  ViewDidClose;
        public event Action  ViewDidOpen;
        public event Action  ViewDidDestroy;

        private void Awake()
        {
            if (!this.ViewRoot)
            {
                this.ViewRoot      = this.GetComponent<CanvasGroup>();
                this.RectTransform = this.GetComponent<RectTransform>();
                this.UpdateAlpha(0);
                this.AwakeUnityEvent();
                this.IsReadyToUse = true;
            }
        }

        private void Start()
        {
            this.StartUnityEvent();
        }

        private void OnDestroy()
        {
            this.OnDestroyUnityEvent();
            this.ViewDidDestroy?.Invoke();
        }

        #region Implementaion IScreenView

        public UniTask Open()
        {
            this.UpdateAlpha(1f);
            Debug.Log($"open screen view {this.name}");
            this.ViewDidOpen?.Invoke();
            return UniTask.CompletedTask;
        }

        public UniTask Close()
        {
            Debug.Log($"Close screen view {this.name}");
            this.UpdateAlpha(0);
            this.ViewDidClose?.Invoke();
            return UniTask.CompletedTask;
        }

        public void Hide()
        {
            this.UpdateAlpha(0);
        }

        public void Show()
        {
            this.UpdateAlpha(1);
        }

        public void DestroySelf()
        {
            Destroy(this.gameObject);
        }

        #endregion

        #region Unity3D Messages propagation

        protected virtual void AwakeUnityEvent()
        {
        }

        protected virtual void StartUnityEvent()
        {
        }

        protected virtual void OnDestroyUnityEvent()
        {
        }

        #endregion

        protected void UpdateAlpha(float value)
        {
            this.ViewRoot.alpha          = value;
            this.ViewRoot.blocksRaycasts = value >= 1;
        }
    }
}