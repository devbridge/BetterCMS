using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class MembershipException : CmsException
    {
        public MembershipException(string message) : base(message)
        {
        }

        public MembershipException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
