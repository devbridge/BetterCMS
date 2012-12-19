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
        /// <summary>
        /// Renders messages box with given id.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <param name="id">The messages box id.</param>
        /// <returns>Html string with rendered messages box.</returns>
        public static IHtmlString MessagesBox(this HtmlHelper html, string id)
        {
            CmsControllerBase controller = html.ViewContext.Controller as CmsControllerBase;
            if (controller == null)
            {
                throw new CmsException("Unable to generate messages box.", new NotSupportedException("Controller should inherit CmsControllerBase class."));
            }
                
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class=\"bcms-messages\"");
            if (!string.IsNullOrEmpty(id))
            {
                sb.Append(" id=\"" + id + "\"");
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