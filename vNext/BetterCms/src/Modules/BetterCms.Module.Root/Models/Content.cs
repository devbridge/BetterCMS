using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Content : EquatableEntity<Content>, IContent, IOptionContainer<Content>
    {
        public virtual string Name { get; set; }

        public virtual string PreviewUrl { get; set; }

        public virtual DateTime? PublishedOn { get; set; }

        public virtual string PublishedByUser { get; set; }

        public virtual ContentStatus Status { get; set; }

        public virtual IList<Content> History { get; set; }

        public virtual Content Original { get; set; }

        public virtual IList<ChildContent> ChildContents { get; set; }

        public virtual IList<PageContent> PageContents { get; set; }

        public virtual IList<ContentOption> ContentOptions { get; set; }
        
        public virtual IList<ContentRegion> ContentRegions { get; set; }

        IEnumerable<IContentRegion> IContent.ContentRegions
        {
            get
            {
                return ContentRegions;
            }
        }

        IEnumerable<IDeletableOption<Content>> IOptionContainer<Content>.Options
        {
            get
            {
                return ContentOptions;
            }
            set
            {
                ContentOptions = value.Cast<ContentOption>().ToList();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether child contents were loaded from the database, or were populated manually.
        /// </summary>
        /// <value>
        ///   <c>true</c> if child contents were loaded from the database; if child contents were populated manually, <c>false</c>.
        /// </value>
        public virtual bool ChildContentsLoaded { get; set; }

        public virtual Content Clone()
        {
            return CopyDataTo(new Content());
        }

        public virtual Content CopyDataTo(Content content, bool copyCollections = true)
        {
            content.Name = Name;
            content.PreviewUrl = PreviewUrl;
            content.PublishedOn = PublishedOn;
            content.PublishedByUser = PublishedByUser;
            content.Status = Status;
            content.Original = Original;

            if (copyCollections && ContentOptions != null)
            {
                if (content.ContentOptions == null)
                {
                    content.ContentOptions = new List<ContentOption>();
                }

                foreach (var contentOption in ContentOptions)
                {
                    var clonedOption = contentOption.Clone();
                    clonedOption.Content = content;

                    content.ContentOptions.Add(clonedOption);
                }
            }

            if (copyCollections && ContentRegions != null)
            {
                if (content.ContentRegions == null)
                {
                    content.ContentRegions = new List<ContentRegion>();
                }

                foreach (var contentRegion in ContentRegions)
                {
                    content.ContentRegions.Add(new ContentRegion
                        {
                            Content = content,
                            Region = contentRegion.Region
                        });
                }
            }

            if (copyCollections && ChildContents != null)
            {
                if (content.ChildContents == null)
                {
                    content.ChildContents = new List<ChildContent>();
                }

                foreach (var childContent in ChildContents)
                {
                    var newChild = new ChildContent
                        {
                            Parent = content,
                            Child = childContent.Child,
                            AssignmentIdentifier = childContent.AssignmentIdentifier
                        };
                    content.ChildContents.Add(newChild);

                    if (childContent.Options != null)
                    {
                        newChild.Options = new List<ChildContentOption>();
                        foreach (var childContentOption in childContent.Options)
                        {
                            var clonedOption = childContentOption.Clone();
                            clonedOption.ChildContent = newChild;

                            newChild.Options.Add(clonedOption);
                        }
                    }
                }
            }

            return content;
        }

        public override string ToString()
        {
            return string.Format("{0}, Name: {1}", base.ToString(), Name);
        }
    }
}