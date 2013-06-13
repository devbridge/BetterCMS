using System;

using BetterCms.Module.MediaManager.Api.DataModels;

namespace BetterCms.Module.Blog.Api.DataModels
{
    public class AuthorModel
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public MediaImage Image { get; set; }
    }
}