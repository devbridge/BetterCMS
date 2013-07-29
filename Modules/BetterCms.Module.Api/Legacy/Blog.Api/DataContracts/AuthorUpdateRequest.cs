using System;

namespace BetterCms.Module.Blog.Api.DataContracts
{
    public class AuthorUpdateRequest
    {
        public Guid AuthorId { get; set; }
        public int Version { get; set; }

        public string Name { get; set; }
        public Guid? ImageId { get; set; }
    }
}