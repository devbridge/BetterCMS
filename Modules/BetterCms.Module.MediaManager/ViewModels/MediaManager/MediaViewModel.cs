using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.MediaManager.Models;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    public class MediaViewModel : IEditableGridItem
    {
        public virtual Guid Id { get; set; }

        public virtual int Version { get; set; }
        
        public DateTime CreatedOn { get; set; }

        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [StringLength(MaxLength.Name, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        public virtual string Name { get; set; }

        public virtual MediaType Type { get; set; }
        
        public virtual MediaContentType ContentType { get; set; }

        public virtual bool IsArchived { get; set; }

        public virtual Guid? ParentFolderId { get; set; }

        public virtual string ParentFolderName { get; set; }

        public virtual string Tooltip { get; set; }

        public virtual string ThumbnailUrl { get; set; }

        public MediaViewModel()
        {
            ContentType = MediaContentType.File;
        }
    }
}