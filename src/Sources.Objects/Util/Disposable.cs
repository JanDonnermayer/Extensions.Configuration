using System;
using System.Diagnostics;
using System.Threading;

namespace System
{
    /// <summary>
    /// An <see cref="IDisposable"/> implementation, that executes a specified <see cref="Action"/> on disposal
    /// </summary>
    /// <history>
    /// [25-10-2018] - Donnermayer - Created
    /// </history>
    [DebuggerStepThrough]
    internal sealed class Disposable : IDisposable
    {
        private int _disposed;
        private readonly Action _disposeAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="Disposable"/> class,
        /// that performs no action on disposal.
        /// </summary>
        public Disposable() : this(() => { }) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Disposable"/> class,
        /// that performs the specified <paramref name="DisposeAction"/> on disposal.
        /// </summary>
        /// <param name="DisposeAction"></param>
        public Disposable(Action DisposeAction) =>
            _disposeAction = DisposeAction ?? throw new ArgumentNullException(nameof(DisposeAction));

        /// <summary>
        /// Returns an IDisposable,
        /// that performs no action on disposal.
        /// </summary>
        public static IDisposable Empty => new Disposable();

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) == 1) return;
            _disposeAction.Invoke();
        }
    }
}