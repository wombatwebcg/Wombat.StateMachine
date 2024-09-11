using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Wombat.StateMachine
{
    public class StateMachine<TCommon>
    {
        private readonly List<StateUnit<TCommon, TCommon>> _states = new List<StateUnit<TCommon, TCommon>>();

        public void AddState(StateUnit<TCommon, TCommon> state)
        {
            _states.Add(state);
        }

        // Connect states, ensure the output type of the previous state matches the input type of the next state
        public void Connect(StateUnit<TCommon, TCommon> fromState, StateUnit<TCommon, TCommon> toState)
        {
            toState.Bind(fromState.Output, fromState);
        }

        // Disconnect states by unbinding the target state
        public void Disconnect(StateUnit<TCommon, TCommon> state)
        {
            state.Unbind();
        }
    }
}