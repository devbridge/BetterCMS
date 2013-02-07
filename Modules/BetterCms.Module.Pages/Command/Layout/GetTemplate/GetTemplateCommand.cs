using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Layout.GetTemplate
{
    /// <summary>
    /// Command for getting the list of templates
    /// </summary>
    public class GetTemplateCommand : CommandBase, ICommand<Guid, Root.Models.Layout>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="templateId">The template id.</param>
        /// <returns>Null or <see cref="Root.Models.Layout"/></returns>
        public Root.Models.Layout Execute(Guid templateId)
        {
            return Repository
                .AsQueryable<Root.Models.Layout>()
                .FirstOrDefault(l => l.Id == templateId);
        }
    }
}