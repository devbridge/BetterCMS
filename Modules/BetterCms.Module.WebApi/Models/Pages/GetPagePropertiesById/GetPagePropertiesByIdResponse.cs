using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Pages.GetPagePropertiesById
{
    [DataContract]
    public class GetPagePropertiesByIdResponse : ResponseBase<PagePropertiesModel>
    {
        [DataMember(Order = 20, Name = "layout")]
        public LayoutModel Layout { get; set; }

        [DataMember(Order = 30, Name = "category")]
        public CategoryModel Category { get; set; }

        [DataMember(Order = 40, Name = "tags")]
        public List<TagModel> Tags { get; set; }        
    }
}