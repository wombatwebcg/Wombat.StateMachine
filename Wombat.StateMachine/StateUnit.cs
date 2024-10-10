using System;
using System.Collections.Generic;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace Wombat.StateMachine
{
    /// 型 StateUnit 扩展，携带自身的上下文信息
    public class StateUnit<TInput, TOutput>
    {
        public string Name { get; set; }
        public IObservable<TInput> Input { get; private set; }
        public Subject<TOutput> Output { get; private set; }

        // 状态单元携带的上下文信息
        public Dictionary<string, object> Context { get; private set; } = new Dictionary<string, object>();

        // 异步状态转换逻辑
        private readonly Func<TInput, StateUnit<TInput, TOutput>, StateUnit<TInput, TOutput>, Task<TOutput>> _asyncTransitionLogic;
        private IDisposable _subscription;
        private StateUnit<TInput, TOutput> _previousState;

        public StateUnit(string name, Func<TInput, StateUnit<TInput, TOutput>, StateUnit<TInput, TOutput>, Task<TOutput>> asyncTransitionLogic)
        {
            Name = name;
            Output = new Subject<TOutput>();
            _asyncTransitionLogic = asyncTransitionLogic;
        }

        // Bind input and transition logic asynchronously, passing both the current and previous StateUnit
        public void Bind(IObservable<TInput> input, StateUnit<TInput, TOutput> previousState)
        {
            Input = input;
            _previousState = previousState;
            _subscription = Input.SelectMany(async inputValue =>
            {
                var outputValue = await _asyncTransitionLogic(inputValue, previousState, this);
                return outputValue;
            })
            .Subscribe(outputValue =>
            {
                Output.OnNext(outputValue);
            });
        }

        public async Task<TOutput> TriggerTransition(TInput input)
        {
            var outputValue = await _asyncTransitionLogic(input, _previousState, this);
            Output.OnNext(outputValue);  // 将结果推送到下一个状态单元
            return outputValue;
        }

        // 取消绑定状态单元
        public void Unbind()
        {
            _subscription?.Dispose();
        }
    }
}
