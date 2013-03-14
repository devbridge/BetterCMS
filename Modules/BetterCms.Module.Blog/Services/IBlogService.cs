using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCms.Module.Blog.Services
{
    public interface IBlogService
    {
        string CreateBlogPermalink(string title);
    }
}