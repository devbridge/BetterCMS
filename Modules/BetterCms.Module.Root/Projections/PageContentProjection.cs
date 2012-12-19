using System;
using System.Runtime.Serialization;
using System.Web.Mvc;

using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Models;

namespace BetterCms.Module.Root.Projections
{
    [Serializable]
    public class PageContentProjection : IStylesheetAccessor, IHtmlAccessor, IJavaScriptAccessor, ISerializable
    {
        private readonly PageContent pageContent;
        private readonly IContentAccessor contentAccessor;

        public PageContentProjection(PageContent pageContent, IContentAccessor contentAccessor)
        {
            this.pageContent = pageContent;
            this.contentAccessor = contentAccessor;
        }

        public PageContentProjection(SerializationInfo info, StreamingContext context)
        { 
            pageContent = (PageContent)info.GetValue("pageContent", typeof(PageContent));
            contentAccessor = (IContentAccessor)info.GetValue("contentAccessor", typeof(IContentAccessor));
        }        

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("pageContent", pageContent, typeof(PageContent));
            info.AddValue("contentAccessor", contentAccessor, contentAccessor.GetType());
        }

        public virtual Guid PageContentId
        {
            get
            {
                return pageContent.Id;
            }
        }

        public virtual int PageContentVersion
        {
            get
            {
                return pageContent.Version;
            }
        }

        public virtual int Order
        {
            get
            {
                return pageContent.Order; 
            }
        }

        public virtual Guid RegionId
        {
            get
            {
                return pageContent.Region.Id;
            }
        }

        public virtual Guid ContentId
        {
            get
            {
                return pageContent.Content.Id;
            }
        }

        public virtual int ContentVersion
        {
            get
            {
                return pageContent.Content.Version;
            }
        }

        public string GetRegionWrapperCssClass(HtmlHelper html)
        {
            return contentAccessor.GetRegionWrapperCssClass(html);
        }

        public string GetHtml(HtmlHelper html)
        {
            return contentAccessor.GetHtml(html);
        }

        public string GetCustomStyles(HtmlHelper html)
        {
            return contentAccessor.GetCustomStyles(html);
        }

        public string GetCustomJavaScript(HtmlHelper html)
        {
            return contentAccessor.GetCustomJavaScript(html);
        }
    }
}
