using System;
using System.Diagnostics;

#if !NET20
using System.Linq.Expressions;
#endif

#if !NET20 && !NET35
using System.Threading.Tasks;
#endif

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

#if DEBUG
         Debug.WriteLine($"Starting {_name} timer.");
#if !NET20
         ExecuteWithLock(() => Debug.Indent());
#endif
#endif
      }

      public static IDisposable Start(string name) => new CodeTimer(name);

      public void Stop()
      {
         if (_stopwatch.IsRunning)
         {
            _stopwatch.Stop();

#if DEBUG
#if !NET20
            ExecuteWithLock(() => Debug.Unindent());
#endif
            Debug.WriteLine($" ←← {_name} took {_stopwatch.ElapsedMilliseconds}ms.");
#endif
         }
      }

      public void Dispose() => Stop();

      // Time for synchronous Actions
      public static void Time(Action action) => TimeInternal(action, action.Method.Name);

#if !NET20
      // Time for Expression-based Actions (not available in .NET 2.0)
      public static void Time(Expression<Action> actionExpression)
      {
         if (actionExpression.Body is MethodCallExpression methodCall)
         {
            string methodName = methodCall.Method.Name;
            Action actionToInvoke = actionExpression.Compile();
            TimeInternal(actionToInvoke, methodName);
         }
         else
         {
            throw new ArgumentException("The expression must be a method call.");
         }
      }
#endif

      private static void TimeInternal(Action action, string methodName)
      {
         using (Start(methodName))
         {
            action();
         }
      }

#if !NET20 && !NET35
      // Time for async Func<Task> (not available in .NET 2.0 and .NET 3.5)
      public static async Task TimeAsync(Func<Task> action)
      {
         string methodName = action.Method.Name;
         await TimeInternal(action, methodName);
      }

      private static async Task TimeInternal(Func<Task> action, string methodName)
      {
         using (Start(methodName))
         {
            await action();
         }
      }
#endif

      private void ExecuteWithLock(Action action)
      {
         lock (_lock) { action(); }
      }
   }
}
