using System;

namespace BetterCms.Core.Exceptions.Modules
{
    public class ModuleRepositoryNullException : ModuleException
    {
        public ModuleRepositoryNullException(string message) : base(message)
        {
        }

        public ModuleRepositoryNullException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
