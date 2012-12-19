using System;

using BetterCms.Module.MediaManager.ViewModels.MediaManager;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.SiteSettings;

namespace BetterCms.Module.MediaManager.ViewModels.Images
{
    [Serializable]
    public class ImagesTabViewModel : SearchableGridViewModel<MediaViewModel>
    {
        public MediaPathViewModel Path { get; set; }

        public virtual bool IsRootFolder
        {
            get
            {
                if (Path != null && Path.CurrentFolder != null && !Path.CurrentFolder.Id.HasDefaultValue())
                {
                    return false;
                }
                return true;
            }
        }
    }
}