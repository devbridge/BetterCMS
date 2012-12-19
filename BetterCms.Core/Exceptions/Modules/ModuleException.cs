using System;

namespace BetterCms.Core.Exceptions.Modules
{
    public class ModuleException : CmsException
    {
        public ModuleException(string message) : base(message)
        {
        }

        public ModuleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
