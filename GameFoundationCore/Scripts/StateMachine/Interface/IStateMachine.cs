namespace GameDevelopmentKit.GameFoundationCore.StateMachine.Interface
{
    using System;

    public interface IStateMachine
    {
        public IState CurrentState { get; }
        public void   TransitionTo(Type stateType);
        public void   TransitionTo<T>() where T : class, IState;
        public void   TransitionTo<TState, TModel>(TModel model) where TState : class, IState<TModel>;
    }
}