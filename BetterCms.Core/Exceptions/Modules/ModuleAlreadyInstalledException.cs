using System;

namespace BetterCms.Core.Exceptions.Modules
{
    [Serializable]
    public class ModuleAlreadyInstalledException : ModuleException
    {
        public ModuleAlreadyInstalledException(string message) : base(message)
        {
        }

        public ModuleAlreadyInstalledException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
