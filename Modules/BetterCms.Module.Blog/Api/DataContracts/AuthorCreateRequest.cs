using System;

namespace BetterCms.Module.Blog.Api.DataContracts
{
    public class AuthorCreateRequest
    {
        public string Name { get; set; }
        public Guid? ImageId { get; set; }
    }
}