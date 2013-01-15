using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Blog;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Transform;

namespace BetterCms.Module.Blog.Commands.GetTemplatesList
{
    public class GetTemplatesCommand : CommandBase, ICommand<bool, IList<BlogTemplateViewModel>>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The list of blog template view models</returns>
        public IList<BlogTemplateViewModel> Execute(bool request)
        {
            BlogTemplateViewModel modelAlias = null;
            Layout layoutAlias = null;

            // Load templates
            var templates = UnitOfWork.Session
                .QueryOver(() => layoutAlias)
                .Where(() => !layoutAlias.IsDeleted)
                .SelectList(select => select
                    .Select(() => layoutAlias.Id).WithAlias(() => modelAlias.TemplateId)
                    .Select(() => layoutAlias.Name).WithAlias(() => modelAlias.Title)
                    .Select(() => layoutAlias.PreviewUrl).WithAlias(() => modelAlias.PreviewUrl))
                .TransformUsing(Transformers.AliasToBean<BlogTemplateViewModel>())
                .List<BlogTemplateViewModel>();

            // Load regions
            Region regionAlias = null;
            LayoutRegion layoutRegionAlias = null;

            var compatibleLayouts = UnitOfWork.Session
                .QueryOver(() => layoutRegionAlias)
                .Inner.JoinQueryOver(() => layoutRegionAlias.Region, () => regionAlias)
                .Where(() => !layoutRegionAlias.IsDeleted
                    && !regionAlias.IsDeleted
                    && regionAlias.RegionIdentifier == BlogModuleConstants.BlogPostMainContentRegionIdentifier)
                .Select(select => select.Layout.Id)
                .List<Guid>();

            foreach (var id in compatibleLayouts)
            {
                templates
                    .Where(t => t.TemplateId == id)
                    .ToList()
                    .ForEach(t => t.IsCompatible = true);
            }

            // Load default template
            Option optionAlias = null;

            var defaultTemplateId = UnitOfWork.Session
                .QueryOver(() => optionAlias)
                .Left.JoinQueryOver(() => optionAlias.DefaultLayout, () => layoutAlias)
                .Where(() => !optionAlias.IsDeleted)
                .OrderBy(() => optionAlias.CreatedOn).Desc
                .Select(select => select.DefaultLayout.Id)
                .Take(1)
                .SingleOrDefault<Guid>();

            if (!defaultTemplateId.HasDefaultValue())
            {
                var defaultTemplate = templates.FirstOrDefault(t => t.TemplateId == defaultTemplateId);
                if (defaultTemplate != null)
                {
                    defaultTemplate.IsActive = true;
                }
            }

            return templates;
        }
    }
}