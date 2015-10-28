using System;
using System.Linq;

using BetterCms.Core.Modules.Projections;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Projections;

using BetterModules.Core.DataAccess.DataContext.Fetching;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Pages.Command.History.GetContentVersionProperties
{
    /// <summary>
    /// Command for getting page content version.
    /// </summary>
    public class GetContentVersionPropertiesCommand : CommandBase, ICommand<Guid, PropertiesPreview>
    {
        private readonly PageContentProjectionFactory projectionFactory;

        public GetContentVersionPropertiesCommand(PageContentProjectionFactory projectionFactory)
        {
            this.projectionFactory = projectionFactory;
        }

        public PropertiesPreview Execute(Guid contentId)
        {
            var content = Repository
                .AsQueryable<Root.Models.Content>(c => c.Id == contentId)
                .FirstOrDefault();

            var accessor = projectionFactory.GetAccessorForType(content);
            return accessor.GetHtmlPropertiesPreview();
        }
    }
}