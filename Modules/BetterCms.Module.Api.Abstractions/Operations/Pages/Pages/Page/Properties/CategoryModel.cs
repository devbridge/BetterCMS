using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    [DataContract]
    [Serializable]
    public class CategoryModel : ModelBase
    {
        /// <summary>
        /// Gets or sets the category name.
        /// </summary>
        /// <value>
        /// The category name.
        /// </value>
        [DataMember]
        public string Name { get; set; }
    }
}