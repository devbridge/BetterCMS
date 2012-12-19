using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.MediaManager.Mvc.Grid.TableRenderers;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    [Serializable]
    public class MediaFileViewModel : MediaViewModel
    {
        public virtual Guid FolderId { get; set; }

        public virtual long Size { get; set; }

        public virtual string SizeKbOrMb
        {
            get
            {

                long sizeKb = (long)Math.Round(Size / 1024f, 0);
                if (sizeKb <= 1024)
                {
                    return sizeKb + " KB";
                }

                return (long)Math.Round(sizeKb / 1024f, 0) + " MB";
            }
        }

        public MediaFileViewModel()
        {
            Type = MediaType.File;
        }
    }
}