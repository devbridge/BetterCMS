using System;

namespace BetterCms.Core.Exceptions.Modules
{
    [Serializable]
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
