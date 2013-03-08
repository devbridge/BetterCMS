using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    public class MediaViewModel : IEditableGridItem
    {
        public virtual Guid Id { get; set; }

        public virtual int Version { get; set; }

        [Required]
        [StringLength(MaxLength.Name)]
        public virtual string Name { get; set; }

        public virtual MediaType Type { get; set; }
        
        public virtual MediaContentType ContentType { get; set; }

        public virtual string FileExtension { get; set; }

        public MediaViewModel()
        {
            ContentType = MediaContentType.File;
        }
    }
}