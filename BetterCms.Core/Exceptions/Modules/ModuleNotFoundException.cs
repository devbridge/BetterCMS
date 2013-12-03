using System;

namespace BetterCms.Core.Exceptions.Modules
{
    [Serializable]
    public class ModuleNotFoundException : ModuleException
    {
        public ModuleNotFoundException(string message)
            : base(message)
        {
        }

        public ModuleNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
