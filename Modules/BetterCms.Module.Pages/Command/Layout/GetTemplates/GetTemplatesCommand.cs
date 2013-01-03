using System.Linq;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Page;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Commands.GetTemplates
{
    /// <summary>
    /// Command for getting the list of templates
    /// </summary>
    public class GetTemplatesCommand : CommandBase, ICommand<GetTemplatesRequest, GetTemplatesResponse>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        /// <exception cref="CmsException">Failed to get layouts.</exception>
        /// <exception cref="System.NotImplementedException"></exception>
        public GetTemplatesResponse Execute(GetTemplatesRequest request)
        {
            var templates = Repository
                .AsQueryable<Layout>()
                .Select(t => new TemplateViewModel
                    {
                        Title = t.Name,
                        TemplateId = t.Id,
                        PreviewUrl = t.PreviewUrl
                    })
                .ToList();

            return new GetTemplatesResponse { Templates = templates };
        }
    }
}