using System;
using System.Threading;

namespace api.Model
{
    public class LockService
    {
        private readonly ReaderWriterLockSlim lockSlim = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

        private readonly TimeSpan timeout = TimeSpan.FromMinutes(1);

        public LockObject LockRead()
        {
            if (lockSlim.IsReadLockHeld || lockSlim.IsWriteLockHeld)
            {
                return new LockObject(() => { });
            }

            //lockSlim.EnterReadLock();
            if (!lockSlim.TryEnterReadLock(timeout))
            {
                throw new LockTimeoutException(timeout.ToString());
            }

            return new LockObject(() =>
            {
                lockSlim.ExitReadLock();
            });
        }

        public LockObject LockWrite()
        {
            if (lockSlim.IsWriteLockHeld)
            {
                return new LockObject(() => { });
            }

            //lockSlim.EnterWriteLock();
            if (!lockSlim.TryEnterWriteLock(timeout))
            {
                throw new LockTimeoutException(timeout.ToString());
            }

            return new LockObject(() =>
            {
                lockSlim.ExitWriteLock();
            });


        }
    }
}
