using System;

namespace BetterCms.Module.Blog.Services
{
    public interface IOptionService
    {
        /// <summary>
        /// Gets the default template id.
        /// </summary>
        /// <returns>Default template id or null, if such is not set</returns>
        Guid? GetDefaultTemplateId();
    }
}