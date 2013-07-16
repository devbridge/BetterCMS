using System;

namespace BetterCms.Module.Blog.Api.DataContracts
{
    public class AuthorDeleteRequest
    {
        public Guid AuthorId { get; set; }

        public int Version { get; set; }
    }
}