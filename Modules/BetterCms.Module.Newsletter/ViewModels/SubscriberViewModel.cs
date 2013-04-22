using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Core.Models;
using BetterCms.Module.Newsletter.Content.Resources;
using BetterCms.Module.Root;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Grids;

namespace BetterCms.Module.Newsletter.ViewModels
{
    public class SubscriberViewModel : IEditableGridItem
    {
        /// <summary>
        /// Gets or sets the author id.
        /// </summary>
        /// <value>
        /// The author id.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        public virtual Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the entity version.
        /// </summary>
        /// <value>
        /// The entity version.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        public virtual int Version { get; set; }

        /// <summary>
        /// Gets or sets the author name.
        /// </summary>
        /// <value>
        /// The author name.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_RequiredAttribute_Message")]
        [StringLength(MaxLength.Email, ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_StringLengthAttribute_Message")]
        [RegularExpression(RootModuleConstants.EmailRegularExpression, ErrorMessageResourceType = typeof(NewsletterGlobalization), ErrorMessageResourceName = "EditSubscriber_IvalidEmail_Message")]
        public virtual string Email { get; set; }
    }
}