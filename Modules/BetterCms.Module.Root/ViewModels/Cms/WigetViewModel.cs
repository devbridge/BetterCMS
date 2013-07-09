using BetterCms.Core.DataContracts;

namespace BetterCms.Module.Root.ViewModels.Cms
{
    public class WigetViewModel
    {
        public IPage Page { get; set; }

        public IWidget Widget { get; set; }
    }
}