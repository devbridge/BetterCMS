using System;

namespace BetterCms.Core.Exceptions.Modules
{
    public class ModuleManagerNullException : ModuleException
    {
        public ModuleManagerNullException(string message) : base(message)
        {
        }

        public ModuleManagerNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
