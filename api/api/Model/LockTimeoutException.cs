using System;

namespace api.Model
{
    public class LockTimeoutException : Exception
    {
        public LockTimeoutException(string message): base(message)
        {

        }
    }
}
