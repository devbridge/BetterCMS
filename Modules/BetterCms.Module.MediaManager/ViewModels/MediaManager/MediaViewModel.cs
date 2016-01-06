using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.MediaManager.Models;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Grids;
using BetterCms.Module.Root.ViewModels.Security;

using BetterModules.Core.Models;

namespace BetterCms.Module.MediaManager.ViewModels.MediaManager
{
    public class MediaViewModel : IEditableGridItem, IAccessSecuredViewModel
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

        public virtual bool IsReadOnly { get; set; }

        public MediaViewModel()
        {
            ContentType = MediaContentType.File;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Name: {2}, Type: {3}, ContentType: {4}, IsArchived: {5}, ParentFolderId: {6}, ParentFolderName: {7}", Id, Version, Name, Type, ContentType, IsArchived, ParentFolderId, ParentFolderName);
        }
    }
}