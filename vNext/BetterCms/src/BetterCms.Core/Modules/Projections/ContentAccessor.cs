using System;
using System.Collections.Generic;

using BetterCms.Core.DataContracts;
using Microsoft.AspNet.Mvc.Rendering;
using Microsoft.AspNet.Mvc.ViewFeatures;

namespace BetterCms.Core.Modules.Projections
{
    public interface IJavaScriptAccessor
    {
        string[] GetCustomJavaScript(IHtmlHelper html);

        string[] GetJavaScriptResources(IHtmlHelper html);
    }

    public interface IStylesheetAccessor
    {
        string[] GetCustomStyles(IHtmlHelper html);

        string[] GetStylesResources(IHtmlHelper html);
    }

    public interface IHtmlAccessor
    {
        string GetContentWrapperType();

        string GetHtml(IHtmlHelper html);

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

        public abstract string GetHtml(IHtmlHelper html);

        public abstract string[] GetCustomStyles(IHtmlHelper html);

        public abstract string[] GetCustomJavaScript(IHtmlHelper html);

        public abstract string[] GetStylesResources(IHtmlHelper html);

        public abstract string[] GetJavaScriptResources(IHtmlHelper html);
    }
}