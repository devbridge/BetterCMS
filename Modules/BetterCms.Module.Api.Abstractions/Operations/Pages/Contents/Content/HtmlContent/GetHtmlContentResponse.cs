using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    [DataContract]
    [Serializable]
    public class GetHtmlContentResponse : ResponseBase<HtmlContentModel>
    {
        /// <summary>
        /// Gets or sets the list of child contents option values.
        /// </summary>
        /// <value>
        /// The list of child contents option values.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<ChildContentOptionValuesModel> ChildContentsOptionValues { get; set; }
    }
}