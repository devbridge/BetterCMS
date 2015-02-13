using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Categories.Category;

namespace BetterCms.Module.Api.Operations.Pages.Widgets.Widget.HtmlContentWidget
{
    [DataContract]
    [Serializable]
    public class GetHtmlContentWidgetResponse : ResponseBase<HtmlContentWidgetModel>
    {
        /// <summary>
        /// Gets or sets the list of widget options.
        /// </summary>
        /// <value>
        /// The list of widget options.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<OptionModel> Options { get; set; }

        /// <summary>
        /// Gets or sets the list of child contents option values.
        /// </summary>
        /// <value>
        /// The list of child contents option values.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<ChildContentOptionValuesModel> ChildContentsOptionValues { get; set; }

        /// <summary>
        /// Gets or sets the list of widget options.
        /// </summary>
        /// <value>
        /// The list of widget options.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<CategoryModel> Categories { get; set; }
    }
}