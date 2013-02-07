using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using BetterCms.Core.Exceptions;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    /// <summary>
    /// Messages helper methods.
    /// </summary>
    public static class MessagesHelper
    {
        private const string cssClassMessagesType1 = "bcms-messages-type-2";
        private const string cssClassMessagesType2 = "bcms-messages-type-3";

        public static IHtmlString TabbedContentMessagesBox(this HtmlHelper html, string id = null, IDictionary<string, string> attributes = null)
        {
            return MessagesBox(html, id, attributes, cssClassMessagesType1);
        }

        public static IHtmlString SiteSettingsMessagesBox(this HtmlHelper html, string id = null, IDictionary<string, string> attributes = null)
        {
            return MessagesBox(html, id, attributes, cssClassMessagesType2);
        }

        /// <summary>
        /// Renders messages box with given id.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <param name="id">The messages box id.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="cssClass">The CSS class.</param>
        /// <returns>
        /// Html string with rendered messages box.
        /// </returns>
        /// <exception cref="CmsException">Unable to generate messages box.;Controller should inherit CmsControllerBase class.</exception>
        /// <exception cref="System.NotSupportedException">Controller should inherit CmsControllerBase class.</exception>
        private static IHtmlString MessagesBox(this HtmlHelper html, string id, IDictionary<string, string> attributes, string cssClass)
        {
            CmsControllerBase controller = html.ViewContext.Controller as CmsControllerBase;
            if (controller == null)
            {
                throw new CmsException("Unable to generate messages box.", new NotSupportedException("Controller should inherit CmsControllerBase class."));
            }
                
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<div class=\"{0}\"", cssClass);
            if (!string.IsNullOrEmpty(id))
            {
                sb.Append(" id=\"" + id + "\"");
            }
            if (attributes != null)
            {
                foreach (var pair in attributes)
                {
                    sb.AppendFormat(" {0}=\"{1}\"", pair.Key, pair.Value);
                }
            }
            sb.AppendLine(">");

            AddMessagesBoxBlock(sb, "bcms-success-messages", controller.Messages.Success);
            AddMessagesBoxBlock(sb, "bcms-info-messages", controller.Messages.Info);
            AddMessagesBoxBlock(sb, "bcms-warning-messages", controller.Messages.Warn);
            AddMessagesBoxBlock(sb, "bcms-error-messages", controller.Messages.Error);
            
            sb.AppendLine("</div>");

            return new MvcHtmlString(sb.ToString());
        }

        /// <summary>
        /// Adds messages block to a messages box.
        /// </summary>
        /// <param name="sb">The string builder to render html.</param>
        /// <param name="blockClass">The block class.</param>
        /// <param name="messages">The messages collection.</param>
        private static void AddMessagesBoxBlock(StringBuilder sb, string blockClass, IList<string> messages)
        {
            sb.Append("<ul");
            sb.Append(" class=\"" + blockClass + "\"");
            if (messages == null || !messages.Any())
            {
                sb.Append(" style=\"display:none\"");
            }
            sb.AppendLine(">");

            if (messages != null)
            {
                foreach (var message in messages)
                {
                    sb.AppendLine("<li>");
                    sb.Append(message);
                    sb.AppendLine("</li>");                    
                }
            }            
            sb.AppendLine("</ul>");
        }
    }
}