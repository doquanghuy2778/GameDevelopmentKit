using ILogServices = GameFoundationCore.LogServices.ILogServices;
using IScreenView = GameFoundationCore.Scripts.ScreenFlow.Base.View.IScreenView;
using SignalTransmitter = GameFoundationCore.Signals.SignalTransmitter;

namespace GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Base.Presenter
{
    using Cysharp.Threading.Tasks;
    using GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Signals;

    public abstract class BasePopupPresenter<TView> : BaseScreenPresenter<TView> where TView : IScreenView
    {
        private readonly SignalTransmitter signalBus;
        private readonly ILogServices      logServices;
        protected BasePopupPresenter(
            SignalTransmitter signalBus,
                ILogServices logServices
            ) : base(signalBus, logServices)
        {
            this.signalBus   = signalBus;
            this.logServices = logServices;
        }

        public override async UniTask OpenViewAsync()
        {
            await this.BindData();

            if (this.ScreenStatus == ScreenStatus.Opened) return;
            this.ScreenStatus = ScreenStatus.Opened;
            this.signalBus.Fire(new ScreenShowSignal() { ScreenPresenter  = this });
            this.signalBus.Fire(new PopupShowedSignal() { ScreenPresenter = this });
            await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            await this.View.Open();
        }

        public override async UniTask CloseViewAsync()
        {
            if (this.ScreenStatus == ScreenStatus.Closed) return;
            this.ScreenStatus = ScreenStatus.Closed;
            await this.View.Close();
            this.signalBus.Fire(new PopupHiddenSignal() { ScreenPresenter = this });
            this.signalBus.Fire(new ScreenCloseSignal() { ScreenPresenter = this });
            this.Dispose();
        }

        public override void HideView()
        {
            if (this.ScreenStatus == ScreenStatus.Hide) return;
            this.ScreenStatus = ScreenStatus.Hide;
            this.View.Hide();
            this.signalBus.Fire(new PopupHiddenSignal() { ScreenPresenter = this });
            this.Dispose();
        }
    }

    public abstract class BasePopupPresenter<TView, TModel> : BasePopupPresenter<TView>, IScreenPresenter<TModel> where TView : IScreenView
    {
        private readonly SignalTransmitter signalBus;
        private readonly ILogServices      logger;
        protected        TModel            Model { get; private set; }

        protected BasePopupPresenter(SignalTransmitter signalBus, ILogServices logger) : base(signalBus, logger)
        {
            this.signalBus = signalBus;
            this.logger    = logger;
        }

        public async UniTask OpenView(TModel model)
        {
            if (model != null) this.Model = model;

            await this.OpenViewAsync();
        }

        public override async UniTask OpenViewAsync()
        {
            if (this.Model != null)
                await this.BindData(this.Model);
            else
                this.logger.LogWarning($"{this.GetType().Name} don't have Model!!!");

            await base.OpenViewAsync();
        }

        public sealed override UniTask BindData() { return UniTask.CompletedTask; }

        public abstract UniTask BindData(TModel popupModel);
    }
}