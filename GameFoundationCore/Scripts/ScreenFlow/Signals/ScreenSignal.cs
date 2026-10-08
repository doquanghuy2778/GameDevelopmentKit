namespace GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Signals
{
    using GameDevelopmentKit.GameFoundationCore.Scripts.ScreenFlow.Base.Presenter;

    public class ScreenCloseSignal
    {
        public IScreenPresenter ScreenPresenter;
    }

    public class ScreenHideSignal
    {
        public IScreenPresenter ScreenPresenter;
    }

    public class ScreenShowSignal
    {
        public IScreenPresenter ScreenPresenter;
    }

    public class ManualInitScreenSignal
    {
        public IScreenPresenter ScreenPresenter;
        public bool             IncludingBindData;
    }

    public class ScreenSelfDestroyedSignal
    {
        public IScreenPresenter ScreenPresenter;
    }

    public class ForceDestroyScreenSignal
    {
        public IScreenPresenter ScreenPresenter;
    }
}