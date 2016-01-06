using System;
using System.Web.Mvc;

using BetterCms.Core.Security;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc.Attributes;

namespace BetterCms.Module.Root.ViewModels.Security
{
    [Serializable]
    public class UserAccessViewModel : IAccessRule
    {
        public Guid Id { get; set; }        

        [AllowHtml]
        [DisallowNonActiveDirectoryNameCompliantAttribute(ErrorMessageResourceType = typeof(RootGlobalization), ErrorMessageResourceName = "Validation_ActiveDirectoryCompliant_Message")]
        public string Identity { get; set; }

        public AccessLevel AccessLevel { get; set; }

        public bool IsForRole { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccessViewModel" /> class.
        /// </summary>
        public UserAccessViewModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccessViewModel" /> class.
        /// </summary>
        /// <param name="accessRule">The access rule.</param>
        public UserAccessViewModel(IAccessRule accessRule)
        {
            Id = accessRule.Id;
            Identity = accessRule.Identity;
            AccessLevel = accessRule.AccessLevel;
            IsForRole = accessRule.IsForRole;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Id: {0}, Identity: {1}, AccessLevel: {2}", Id, Identity, AccessLevel);
        }
    }
}