using System;
using System.ComponentModel.DataAnnotations;

using BetterCms.Module.Newsletter.Content.Resources;

using BetterCms.Module.Root;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Grids;
using BetterCms.Module.Root.ViewModels.Cms;

using BetterModules.Core.Models;

namespace BetterCms.Module.Newsletter.ViewModels
{
    public class SubscriberViewModel : RenderWidgetViewModel, IEditableGridItem
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

        /// <summary>
        /// Gets or sets a value indicating whether to ignore unique subscriber exception.
        /// </summary>
        /// <value>
        /// <c>true</c> if to ignore unique subscriber exception; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreUniqueSubscriberException { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Version: {1}, Email: {2}, IgnoreUniqueSubscriberException: {3}", Id, Version, Email, IgnoreUniqueSubscriberException);
        }
    }
}