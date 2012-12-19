using System;

namespace BetterCms.Core.Exceptions.Service
{
    public class ApprovalException : CmsException
    {
        public ApprovalException(string message) : base(message)
        {
        }

        public ApprovalException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
