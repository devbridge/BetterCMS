using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ChildContent : EquatableEntity<ChildContent>, IChildContent
    {
        public virtual Content Parent { get; set; }

        public virtual Content Child { get; set; }
        
        public virtual Guid AssignmentIdentifier { get; set; }

        public virtual IList<ChildContentOption> Options { get; set; }

        IContent IChildContent.ChildContent
        {
            get
            {
                return Child;
            }
            set
            {
                Child = (Content)value;
            }
        }

        IEnumerable<IOptionEntity> IChildContent.Options
        {
            get
            {
                return Options;
            }
        }

        public virtual ChildContent Clone()
        {
            return CopyDataTo(new ChildContent());
        }

        public virtual ChildContent CopyDataTo(ChildContent targetChildContent, bool copyOptions = true)
        {
            targetChildContent.Id = Id;
            targetChildContent.Version = Version;
            targetChildContent.Child = Child;
            targetChildContent.Parent = Parent;
            targetChildContent.AssignmentIdentifier = AssignmentIdentifier;

            if (copyOptions && Options != null)
            {
                if (targetChildContent.Options == null)
                {
                    targetChildContent.Options = new List<ChildContentOption>();
                }

                foreach (var childContentOption in Options)
                {
                    var clonedOption = childContentOption.Clone();
                    clonedOption.ChildContent = targetChildContent;

                    targetChildContent.Options.Add(clonedOption);
                }
            }

            return targetChildContent;
        }
    }
}