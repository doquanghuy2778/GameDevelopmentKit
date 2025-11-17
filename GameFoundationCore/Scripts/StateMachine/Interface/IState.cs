namespace GameDevelopmentKit.GameFoundationCore.StateMachine.Interface
{
    public interface IState
    {
        public void Enter();
        public void Exit();
    }

    public interface IState<in TModel> : IState
    {
        public TModel Model { set; }
    }
}