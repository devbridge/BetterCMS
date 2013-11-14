using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Models;

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

        public virtual IList<PageContent> PageContents { get; set; }

        public virtual IList<ContentOption> ContentOptions { get; set; }
        
        public virtual IList<ContentRegion> ContentRegions { get; set; }

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

        public virtual Content Clone()
        {
            return CopyDataTo(new Content());
        }

        public virtual Content CopyDataTo(Content content, bool copyOptions = true, bool copyRegions = true)
        {
            content.Name = Name;
            content.PreviewUrl = PreviewUrl;
            content.PublishedOn = PublishedOn;
            content.PublishedByUser = PublishedByUser;
            content.Status = Status;
            content.Original = Original;

            if (copyOptions && ContentOptions != null)
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

            if (copyRegions && ContentRegions != null)
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

            return content;
        }
    }
}