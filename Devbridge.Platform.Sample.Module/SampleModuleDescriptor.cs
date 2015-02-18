using Devbridge.Platform.Core.Modules;

namespace Devbridge.Platform.Sample.Module
{
    public class SampleModuleDescriptor : ModuleDescriptor
    {
        public const string ModuleName = "DBSample";

        public override string Description
        {
            get
            {
                return "Sample Devbridge Platform Module";
            }
        }

        public override string Name
        {
            get
            {
                return ModuleName;
            }
        }
    }
}
