using System;
using System.Runtime.Serialization;

using BetterCms.Module.MediaManager.Api.DataModels;

namespace BetterCms.Module.Blog.Api.DataModels
{
    [DataContract]
    public class AuthorModel
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public int Version { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public MediaImage Image { get; set; }
    }
}