using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BetterCms.Module.WebApi.Models.Root
{
    [DataContract]
    public class LayoutModel : ModelBase
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string LayoutPath { get; set; }

        [DataMember]
        public string PreviewUrl { get; set; }

        [DataMember]
        public Guid ModuleId { get; set; }        
    }
}