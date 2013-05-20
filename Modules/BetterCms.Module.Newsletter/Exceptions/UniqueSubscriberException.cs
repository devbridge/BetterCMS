using System;

using BetterCms.Core.Exceptions.Mvc;

namespace BetterCms.Module.Newsletter.Exceptions
{
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