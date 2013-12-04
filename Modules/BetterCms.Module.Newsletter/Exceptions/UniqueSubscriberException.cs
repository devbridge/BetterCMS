using System;

using BetterCms.Core.Exceptions.Mvc;

namespace BetterCms.Module.Newsletter.Exceptions
{
    [Serializable]
    public class UniqueSubscriberException : ValidationException
    {
        public UniqueSubscriberException(Func<string> resource, string message)
            : base(resource, message)
        {
        }

        public UniqueSubscriberException(Func<string> resource, string message, Exception innerException)
            : base(resource, message, innerException)
        {
        }
    }
}