using System;
using Microsoft.Extensions.Primitives;

namespace Extensions.Configuration.Sources.Object
{
    internal class EmptyChangeToken : IChangeToken
    {
        public bool HasChanged => false;

        public bool ActiveChangeCallbacks => false;

        public IDisposable RegisterChangeCallback(Action<object> callback, object state) => Disposable.Empty;
    }
}