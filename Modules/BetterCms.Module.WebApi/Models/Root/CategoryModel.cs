using System.Runtime.Serialization;

namespace BetterCms.Module.WebApi.Models.Root
{
    [DataContract]
    public class CategoryModel : ModelBase
    {
        [DataMember]
        public string Name { get; set; }
    }
}