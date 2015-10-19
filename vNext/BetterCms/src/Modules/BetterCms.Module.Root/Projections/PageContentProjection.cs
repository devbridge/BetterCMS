using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Modules.Projections;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace BetterCms.Module.Root.Projections
{
    [Serializable]
    public class PageContentProjection : IStylesheetAccessor, IHtmlAccessor, IJavaScriptAccessor, ISerializable
    {
        private readonly IPageContent pageContent;

        private readonly IContent content;

        private readonly IContentAccessor contentAccessor;

        private readonly IEnumerable<ChildContentProjection> childProjections;

        private readonly IEnumerable<PageContentProjection> childRegionContentProjections;

        public PageContentProjection(IPageContent pageContent, IContent content, IContentAccessor contentAccessor,
            IEnumerable<ChildContentProjection> childProjections = null, IEnumerable<PageContentProjection> childRegionContentProjections = null)
        {
            this.pageContent = pageContent;
            this.content = content;
            this.contentAccessor = contentAccessor;
            this.childProjections = childProjections;
            this.childRegionContentProjections = childRegionContentProjections;
        }

        public PageContentProjection(SerializationInfo info, StreamingContext context)
        { 
            pageContent = (IPageContent)info.GetValue("pageContent", typeof(IPageContent));
            content = (IContent)info.GetValue("content", typeof(IContent));
            contentAccessor = (IContentAccessor)info.GetValue("contentAccessor", typeof(IContentAccessor));
        }        

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("pageContent", pageContent, typeof(IPageContent));
            info.AddValue("content", content, typeof(IContent));
            info.AddValue("contentAccessor", contentAccessor, contentAccessor.GetType());
        }

        public virtual IPageContent PageContent => pageContent;

        public virtual Guid PageContentId => pageContent.Id;

        public virtual Guid PageId => pageContent.Page.Id;

        public virtual int PageContentVersion => pageContent.Version;

        public virtual ContentStatus PageContentStatus => content.Status;

        public virtual int Order => pageContent.Order;

        public virtual Guid RegionId => pageContent.Region.Id;

        public virtual string RegionIdentifier => pageContent.Region.RegionIdentifier;

        public virtual Guid ContentId => content.Id;

        public virtual int ContentVersion => content.Version;

        public virtual IContent Content => content;

        public string GetContentWrapperType()
        {
            return contentAccessor.GetContentWrapperType();
        }

        public string GetTitle()
        {
            return contentAccessor.GetTitle();
        }

        public string GetHtml(IHtmlHelper html)
        {
            return contentAccessor.GetHtml(html);
        }

        public string[] GetCustomStyles(IHtmlHelper html)
        {
            return GetStylesAndScripts(accessor => accessor.GetCustomStyles(html));
        }

        public string[] GetCustomJavaScript(IHtmlHelper html)
        {
            return GetStylesAndScripts(accessor => accessor.GetCustomJavaScript(html));
        }

        public string[] GetStylesResources(IHtmlHelper html)
        {
            return null;
        }

        public string[] GetJavaScriptResources(IHtmlHelper html)
        {
            return null;
        }

        public IEnumerable<ChildContentProjection> GetChildProjections()
        {
            return childProjections;
        }
        
        public IEnumerable<PageContentProjection> GetChildRegionContentProjections()
        {
            return childRegionContentProjections;
        }

        private string[] GetStylesAndScripts(Func<IContentAccessor, string[]> func, List<string> renderedContents = null)
        {
            if (renderedContents == null)
            {
                renderedContents = new List<string>();
            }

            var contentArray = func.Invoke(contentAccessor);
            if (childProjections != null || childRegionContentProjections != null)
            {
                var contentList = contentArray?.ToList() ?? new List<string>();
                var allChildProjections = new List<PageContentProjection>();
                if (childProjections != null)
                {
                    allChildProjections.AddRange(childProjections);
                }
                if (childRegionContentProjections != null)
                {
                    allChildProjections.AddRange(childRegionContentProjections);
                }

                foreach (var childProjection in allChildProjections)
                {
                    var key = string.Format("{0}-{1}", childProjection.PageContentId, childProjection.ContentId);
                    if (!renderedContents.Contains(key))
                    {
                        renderedContents.Add(key);

                        var childContentArray = childProjection.GetStylesAndScripts(func, renderedContents);
                        if (childContentArray != null)
                        {
                            contentList.AddRange(childContentArray);
                        }
                    }
                }

                contentArray = contentList.ToArray();
            }

            return contentArray;
        }
    }
}
