namespace Devbridge.Platform.Core.Modules.Registration
{
    public class ModuleRegistrationContext
    {
        public ModuleRegistrationContext(ModuleDescriptor moduleDescriptor)
        {
            ModuleDescriptor = moduleDescriptor;
        }

        public virtual ModuleDescriptor ModuleDescriptor { get; private set; }

        public virtual string GetRegistrationName()
        {
            return ModuleDescriptor.Name.ToLowerInvariant();
        }
    }
}