using System;
using System.Collections.Generic;

using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.Commands.UploadBlogsImportFile
{
    public class UploadBlogsImportFileResponse
    {
        public IList<BlogPostImportResult> Results { get; set; }
        
        public Guid FileId { get; set; }
    }
}