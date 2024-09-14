using System;
using System.Threading.Tasks;
using Wombat.StateMachine;

namespace StateMachineTest
{
    class Program
    {
        // Asynchronous transition logic for each state, now with access to both previous and current StateUnit
        static async Task<string> StartStateTransition(string input, StateUnit<string, string> previousState, StateUnit<string, string> currentState)
        {
            await Task.Delay(1);
            //await Task.Delay(1); // Simulating some async work
            //Console.WriteLine($"Transitioning from {previousState?.Name ?? "null"} to {currentState.Name} with input {input}");
            ////currentState.Output.OnNext("step2");
            //return input == "Start" ? "step1" : "Idle";
            return string.Empty;
        }
        static int index1 = 0;
        static async Task<string> Step1(string input, StateUnit<string, string> previousState, StateUnit<string, string> currentState)
        {
            await Task.Delay(1);
            index1++;
            Console.WriteLine($"Transitioning from {previousState?.Name ?? "null"} to {currentState.Name} with input {input}   {index1}");
            await Task.Delay(1); // Simulating some async work

            return string.Empty;

        }

        static int index2 = 0;

        static async Task<string> Step2(string input, StateUnit<string, string> previousState, StateUnit<string, string> currentState)
        {
            await Task.Delay(1);
            index2++;
            await Task.Delay(1); // Simulating some async work

            Console.WriteLine($"Transitioning from {previousState?.Name ?? "null"} to {currentState.Name} with input {input}   {index2}");
            return input == "step2" ? "step3" : "step0";

        }
        static int index3 = 0;

        static async Task<string> Step3(string input, StateUnit<string, string> previousState, StateUnit<string, string> currentState)
        {
          var yy=  currentState.Input;
            await Task.Delay(1);
            index3++;
            await Task.Delay(1); // Simulating some async work
            Console.WriteLine($"Transitioning from {previousState?.Name ?? "null"} to {currentState.Name} with input {input}   {index3}");
            return string.Empty;

        }


        static async Task Main(string[] args)
        {
            // Create state machine
            var fsm = new StateMachine<string>();

            // Create states with async transition logic
            var startState = new StateUnit<string, string>("Start", StartStateTransition);
            var step1 = new StateUnit<string, string>("step1", Step1);
            var step2 = new StateUnit<string, string>("step2", Step2);
            var step3 = new StateUnit<string, string>("step3", Step3);

            // Add states to state machine
            fsm.AddState(startState);
            fsm.AddState(step1);
            fsm.AddState(step2);
            fsm.AddState(step3);
            //fsm.AddState(step4);

            // Connect states, passing both previous and current StateUnit references
            fsm.Connect(startState, step1);
            fsm.Connect(step1, step2);
            //fsm.Connect(step2, step1);
            fsm.Connect(step2, step3);
            //fsm.Connect(step3, step1);

            //fsm.Connect(step3, startState);

            await startState.TriggerTransition("Start");

            ////fsm.Connect(step1, step3);
            //// Simulate input triggers
            //Console.WriteLine("Simulating input...");
            //for (; ; )
            //{
            //    await startState.TriggerTransition("Start");
            //    await Task.Delay(1000); // Wait for transitions

            //}

            //// Simulate input triggers
            //Console.WriteLine("Simulating input...");
            //await startState.TriggerTransition("Start");
            //await Task.Delay(1000); // Wait for transitions

            //await step1.TriggerTransition("step1");
            //await Task.Delay(1000); // Wait for transitions

            //await step2.TriggerTransition("step2");
            //await Task.Delay(1000); // Wait for transitions

            //await step3.TriggerTransition("step3"); // This should trigger transition back to step1
            await Task.Delay(1000); // Wait for transitions
            Console.ReadKey();
        }
    }
}
