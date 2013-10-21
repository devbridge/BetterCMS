using System;
using System.Runtime.Serialization;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Modules.Projections;

namespace BetterCms.Module.Root.Projections
{
    [Serializable]
    public class PageContentProjection : IStylesheetAccessor, IHtmlAccessor, IJavaScriptAccessor, ISerializable
    {
        private readonly IPageContent pageContent;

        private readonly IContent content;

        private readonly IContentAccessor contentAccessor;

        public PageContentProjection(IPageContent pageContent, IContent content, IContentAccessor contentAccessor)
        {
            this.pageContent = pageContent;
            this.content = content;
            this.contentAccessor = contentAccessor;
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

        public virtual ContentStatus PageContentStatus
        {
            get
            {
                return content.Status;
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
                return content.Id;
            }
        }

        public virtual int ContentVersion
        {
            get
            {
                return content.Version;
            }
        }

        public virtual IContent Content
        {
            get
            {
                return content;
            }
        }

        public string GetContentWrapperType()
        {
            return contentAccessor.GetContentWrapperType();
        }

        public string GetTitle()
        {
            return contentAccessor.GetTitle();
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

        public string[] GetStylesResources(HtmlHelper html)
        {
            return contentAccessor.GetStylesResources(html);
        }

        public string[] GetJavaScriptResources(HtmlHelper html)
        {
            return contentAccessor.GetJavaScriptResources(html);
        }
    }
}
