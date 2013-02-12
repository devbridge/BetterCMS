using System;
using System.Collections.Generic;

using BetterCms.Api.Interfaces.Models;
using BetterCms.Api.Interfaces.Models.Enums;
using BetterCms.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class Content : EquatableEntity<Content>, IContent
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

        public virtual Content Clone()
        {
            return CopyDataTo(new Content());
        }

        public virtual Content CopyDataTo(Content content)
        {
            content.Name = Name;
            content.PreviewUrl = PreviewUrl;
            content.PublishedOn = PublishedOn;
            content.PublishedByUser = PublishedByUser;
            content.Status = Status;
            content.Original = Original;

            if (ContentOptions != null)
            {
                if (content.ContentOptions == null)
                {
                    content.ContentOptions = new List<ContentOption>();
                }

                foreach (var contentOption in ContentOptions)
                {
                    content.ContentOptions.Add(
                        new ContentOption
                            {
                                Type = contentOption.Type,
                                Key = contentOption.Key,
                                DefaultValue = contentOption.DefaultValue,
                                Content = content
                            });
                }
            }

            return content;
        }
    }
}