// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MessagesHelper.cs" company="Devbridge Group LLC">
//
// Copyright (C) 2015,2016 Devbridge Group LLC
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
//
// Website: https://www.bettercms.com
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using BetterCms.Core.Exceptions;

using BetterModules.Core.Web.Mvc;

namespace BetterCms.Module.Root.Mvc.Helpers
{
    /// <summary>
    /// Messages helper methods.
    /// </summary>
    public static class MessagesHelper
    {
        private const string cssClassMessages = "bcms-messages-ui bcms-js-messages";
        private const string cssClassCustomMessages = "bcms-custom-messages";

        public static IHtmlString CustomMessagesBox(this HtmlHelper html, IMessagesIndicator messages, string id = null,
            IDictionary<string, string> attributes = null)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                id = string.Format("bcms-custom-messages-{0}", Guid.NewGuid());
            }

            return MessagesBox(html, id, attributes, cssClassMessages + " " + cssClassCustomMessages, messages);
        }

        public static IHtmlString MessagesBox(this HtmlHelper html, string id = null,
            IDictionary<string, string> attributes = null)
        {
            return MessagesBox(html, id, attributes, cssClassMessages);
        }

        /// <summary>
        /// Renders messages box with given id.
        /// </summary>
        /// <param name="html">The HTML helper.</param>
        /// <param name="id">The messages box id.</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="cssClass">The CSS class.</param>
        /// <param name="messages">The messages.</param>
        /// <returns>
        /// Html string with rendered messages box.
        /// </returns>
        /// <exception cref="CmsException">Unable to generate messages box.;Controller should inherit CmsControllerBase class.</exception>
        /// <exception cref="System.NotSupportedException">Controller should inherit CmsControllerBase class.</exception>
        private static IHtmlString MessagesBox(this HtmlHelper html, string id,
            IDictionary<string, string> attributes, string cssClass, IMessagesIndicator messages = null)
        {
            var controller = html.ViewContext.Controller as CmsControllerBase;
            if (controller == null)
            {
                throw new CmsException("Unable to generate messages box.", new NotSupportedException("Controller should inherit CmsControllerBase class."));
            }

            string customCssClass = null;
            if (attributes != null)
            {
                customCssClass = attributes
                    .Where(a => a.Key == "class" && !string.IsNullOrWhiteSpace(a.Value))
                    .Select(a => string.Format(" {0}", a.Value))
                    .FirstOrDefault();
            }

            var sb = new StringBuilder();
            sb.AppendFormat("<div class=\"{0} {1}\"", cssClass, customCssClass);
            if (!string.IsNullOrEmpty(id))
            {
                sb.Append(" id=\"" + id + "\"");
            }
            if (attributes != null)
            {
                foreach (var pair in attributes)
                {
                    if (pair.Key != "class")
                    {
                        sb.AppendFormat(" {0}=\"{1}\"", pair.Key, pair.Value);
                    }
                }
            }
            sb.AppendLine(">");

            if (messages == null)
            {
                messages = controller.Messages;
            }

            AddMessagesBoxBlock(sb, "bcms-success-messages", messages.Success);
            AddMessagesBoxBlock(sb, "bcms-info-messages", messages.Info);
            AddMessagesBoxBlock(sb, "bcms-warning-messages", messages.Warn);
            AddMessagesBoxBlock(sb, "bcms-error-messages", messages.Error);

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
                    sb.Append("<div class=\"bcms-messages-close bcms-js-btn-close\">Close</div>");
                    sb.Append(message);
                    sb.AppendLine("</li>");
                }
            }
            sb.AppendLine("</ul>");
        }
    }
}