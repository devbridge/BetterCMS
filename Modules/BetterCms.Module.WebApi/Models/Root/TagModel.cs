using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BetterCms.Module.WebApi.Models.Root
{
    [DataContract]
    public class TagModel : ModelBase
    {
        [DataMember]
        public string Name { get; set; }
    }
}