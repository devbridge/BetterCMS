using System;
using System.Collections.Generic;
using System.Web.Mvc;

using BetterCms.Core.DataContracts;

namespace BetterCms.Core.Modules.Projections
{
    public interface IJavaScriptAccessor
    {
        string[] GetCustomJavaScript(HtmlHelper html);

        string[] GetJavaScriptResources(HtmlHelper html);
    }

    public interface IStylesheetAccessor
    {
        string[] GetCustomStyles(HtmlHelper html);

        string[] GetStylesResources(HtmlHelper html);
    }

    public interface IHtmlAccessor
    {
        string GetContentWrapperType();

        string GetHtml(HtmlHelper html);

        string GetTitle();
    }

    public interface IContentAccessor : IHtmlAccessor, IStylesheetAccessor, IJavaScriptAccessor
    {
    }

    [Serializable]
    public abstract class ContentAccessor<TContent> :  IContentAccessor 
        where TContent : IContent
    {
        protected TContent Content { get; private set; }

        protected IList<IOptionValue> Options { get; private set; }
        
        protected ContentAccessor(TContent content, IList<IOptionValue> options)
        {
            Content = content;
            Options = options;
        }

        public virtual string GetTitle()
        {
            return Content.Name;
        }

        public abstract string GetContentWrapperType();

        public abstract string GetHtml(HtmlHelper html);

        public abstract string[] GetCustomStyles(HtmlHelper html);

        public abstract string[] GetCustomJavaScript(HtmlHelper html);

        public abstract string[] GetStylesResources(HtmlHelper html);

        public abstract string[] GetJavaScriptResources(HtmlHelper html);
    }
}