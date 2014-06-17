using System;

using BetterCms.Module.Pages.Models;

namespace BetterCms.Module.Pages.Command.Content.GetChildContentType
{
    public class GetChildContentTypeCommandResponse
    {
        public Guid Id { get; set; }
        
        public string Type { get; set; }
    }
}