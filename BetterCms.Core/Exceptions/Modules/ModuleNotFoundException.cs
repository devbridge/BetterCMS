using System;

namespace BetterCms.Core.Exceptions.Modules
{
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
