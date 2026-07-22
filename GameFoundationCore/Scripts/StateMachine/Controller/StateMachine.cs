using ILogServices = GameFoundationCore.LogServices.ILogServices;

namespace GameDevelopmentKit.GameFoundationCore.StateMachine.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameDevelopmentKit.GameFoundationCore.StateMachine.Interface;
    using VContainer.Unity;

    public abstract class StateMachine : IStateMachine, ITickable
    {

        #region Inject

        protected readonly ILogServices LogServices;
        protected readonly Dictionary<Type, IState> TypeToState;

        protected StateMachine(
            List<IState> states,
            ILogServices logServices)
        {
            this.TypeToState = states.ToDictionary(states => states.GetType(), states => states);
            this.LogServices = logServices;
        }

        #endregion

        public IState CurrentState { get; private set; }

        #region Implement IStateMachine

        public void TransitionTo(Type stateType)
        {
            if (!this.TypeToState.TryGetValue(stateType, out var nextState)) return;

            this.InternalStateTransition(nextState);
        }

        public void TransitionTo<T>() where T : class, IState
        {
            this.TransitionTo(typeof(T));
        }

        public void TransitionTo<TState, TModel>(TModel model) where TState : class, IState<TModel>
        {
            var stateType = typeof(TState);
            if (!this.TypeToState.TryGetValue(stateType, out var nextState)) return;

            if (nextState is not TState nextStateT) return;
            nextStateT.Model = model;

            this.InternalStateTransition(nextState);
        }

        #endregion

        private void InternalStateTransition(IState nextState)
        {
            if (this.CurrentState != null)
            {
                this.CurrentState.Exit();
                this.LogServices.Log($"Exit {this.CurrentState.GetType().Name} State!!!");
            }

            this.CurrentState = nextState;
            this.LogServices.Log($"Enter {nextState.GetType().Name} State!!!");
            nextState.Enter();
        }

        public void Tick()
        {
            if (this.CurrentState is not ITickable tickableState) return;
            tickableState.Tick();
        }
    }
}