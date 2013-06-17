using System;
using System.Runtime.Serialization;

namespace BetterCms.Module.Pages.Api.DataModels
{
    [DataContract]
    public class CategoryModel
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public string Title { get; set; }
    }
}