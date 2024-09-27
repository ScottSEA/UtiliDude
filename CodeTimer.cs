using System;
using System.Diagnostics;

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

        private void Lock(Action action)
        {
            lock (_lock) { action(); }
        }
    }
}
