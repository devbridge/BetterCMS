using BetterCms.Module.Blog.Models;

namespace BetterCms.Module.Blog.Services
{
    public interface IOptionService
    {
        /// <summary>
        /// Gets the default option.
        /// </summary>
        Option GetDefaultOption();
    }
}