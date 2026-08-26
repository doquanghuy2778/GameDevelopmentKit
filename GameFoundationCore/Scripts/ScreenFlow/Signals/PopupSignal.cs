namespace GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Signals
{
    using GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Base.Presenter;

    public class PopupShowedSignal
    {
        public IScreenPresenter ScreenPresenter;
    }

    public class PopupHiddenSignal
    {
        public IScreenPresenter ScreenPresenter;
    }

    public class PopupBlurBgShowedSignal
    {
    }
}