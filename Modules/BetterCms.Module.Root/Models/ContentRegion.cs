using System;

using BetterCms.Core.DataContracts;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ContentRegion : EquatableEntity<ContentRegion>, IContentRegion
    {
        public virtual Content Content { get; set; }

        public virtual Region Region { get; set; }

        IDynamicContentContainer IContentRegion.Content
        {
            get
            {
                return (IDynamicContentContainer)Content;
            }
            set
            {
                Content = (Content)value;
            }
        }

        IRegion IContentRegion.Region
        {
            get
            {
                return Region;
            }
            set
            {
                Region = (Region)value;
            }
        }
    }
}