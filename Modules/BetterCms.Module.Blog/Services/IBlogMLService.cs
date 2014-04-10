using System.Collections.Generic;
using System.Security.Principal;

using BetterCms.Module.Blog.Models;

using BlogML.Xml;

namespace BetterCms.Module.Blog.Services
{
    public interface IBlogMLService
    {
        BlogMLBlog DeserializeXMLFile(string filePath);

        List<BlogPost> ImportBlogs(BlogMLBlog blogPosts, IPrincipal principal, bool useOriginalUrls = false, bool createRedirects = false);
    }
}