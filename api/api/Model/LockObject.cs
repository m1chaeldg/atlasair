using System;

namespace api.Model
{
    public class LockObject : IDisposable
    {
        private readonly Action dispose;

        public LockObject(Action dispose)
        {
            this.dispose = dispose;
        }

        public void Dispose()
        {
            dispose?.Invoke();
            GC.SuppressFinalize(this);
        }
    }
}
