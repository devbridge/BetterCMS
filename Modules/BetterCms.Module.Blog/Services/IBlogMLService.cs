using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;

using BetterCms.Module.Blog.Models;

using BlogML.Xml;

namespace BetterCms.Module.Blog.Services
{
    public interface IBlogMLService
    {
        BlogMLBlog DeserializeXMLFile(string filePath);
        
        BlogMLBlog DeserializeXMLStream(Stream fileStream);

        List<BlogPostImportResult> ValidateImport(BlogMLBlog blogPosts);

        List<BlogPostImportResult> ImportBlogs(BlogMLBlog blogPosts, List<BlogPostImportResult> modifications, IPrincipal principal, bool createRedirects);

        Uri ConstructFilePath(Guid guid);
    }
}