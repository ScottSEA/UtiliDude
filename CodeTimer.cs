using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace UtiliDude
{
    public class CodeTimer : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly string _name;
        private static readonly object _lock = new object();

        private CodeTimer(string name)
        {
            _name = string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name))
                : name;

            _stopwatch = Stopwatch.StartNew();

            Debug.WriteLine($" →→ Starting {_name} timer.");
            Lock(() => Debug.Indent());
        }

        public static IDisposable Start(string name) => new CodeTimer(name);

        public void Stop()
        {
            _stopwatch.Stop();
            Lock(() => Debug.Unindent());
            Debug.WriteLine($" ←← {_name} took {_stopwatch.ElapsedMilliseconds}ms.");
        }

        public void Dispose() => Stop();


        public static void Time(Delegate action)
        {
            string methodName = action.Method.Name;
            Action actionToInvoke = action as Action ?? (() => action.DynamicInvoke());
            Time(actionToInvoke, methodName);
        }

        public static void Time(Expression<Action> actionExpression)
        {
            if (actionExpression.Body is MethodCallExpression methodCall)
            {
                string methodName = methodCall.Method.Name;
                Action actionToInvoke = actionExpression.Compile();
                Time(actionToInvoke, methodName);
            }
            else
            {
                throw new ArgumentException("The expression must be a method call.");
            }
        }

        private static void Time(Action action, string methodName)
        {
            using (Start(methodName)) { action(); }
        }

        private void Lock(Action action)
        {
            lock (_lock) { action(); }
        }
    }
}
