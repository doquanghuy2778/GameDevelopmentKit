namespace GameDevelopmentKit.GameFoundationCore.Scripts.MVP
{
    public interface IUIPresenter
    {
        public void SetView(IUIView viewInstance);
    }
    
    public interface IUIPresenterWithModel<TModel> : IUIPresenter
    {
        void Init(IUIView viewInstance, TModel param);
    }
}