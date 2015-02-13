using Devbridge.Platform.Core.Web.Modules;

namespace Devbridge.Platform.Sample.Web.Module
{
    public class SampleWebModuleDescriptor : WebModuleDescriptor
    {
        public override string Description
        {
            get
            {
                return "Sample Devbridge Platform Web Module";
            }
        }

        public override string Name
        {
            get
            {
                return "DBWebSample";
            }
        }
    }
}
