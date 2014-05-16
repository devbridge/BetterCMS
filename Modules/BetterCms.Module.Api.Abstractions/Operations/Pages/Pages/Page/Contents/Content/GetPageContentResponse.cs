using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content
{
    [DataContract]
    [Serializable]
    public class GetPageContentResponse : ResponseBase<PageContentModel>
    {
        /// <summary>
        /// Gets or sets the list of page content options.
        /// </summary>
        /// <value>
        /// The list of page content options.
        /// </value>
        [DataMember]
        public System.Collections.Generic.IList<OptionValueModel> Options { get; set; }
    }
}