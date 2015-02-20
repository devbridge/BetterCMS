using System;
using System.Linq;

using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Templates;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using BetterModules.Core.Exceptions.DataTier;
using BetterModules.Core.Web.Mvc.Commands;

using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Layout.GetTemplateForEdit
{
    public class GetTemplateForEditCommand : CommandBase, ICommand<Guid?, TemplateEditViewModel>
    {
        /// <summary>
        /// The layout service
        /// </summary>
        private ILayoutService layoutService;

        /// <summary>
        /// The option service
        /// </summary>
        private IOptionService optionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetTemplateForEditCommand" /> class.
        /// </summary>
        /// <param name="layoutService">The layout service.</param>
        /// <param name="optionService">The option service.</param>
        public GetTemplateForEditCommand(ILayoutService layoutService, IOptionService optionService)
        {
            this.layoutService = layoutService;
            this.optionService = optionService;
        }

        public TemplateEditViewModel Execute(Guid? templateId)
        {
            TemplateEditViewModel templateModel;

            if (!templateId.HasValue)
            {
                templateModel = new TemplateEditViewModel();
            }
            else
            {
                Root.Models.Layout templateAlias = null;
                TemplateEditViewModel templateViewModelAlias = null;

                var templateFuture = UnitOfWork.Session.QueryOver(() => templateAlias)
                     .Where(() => templateAlias.Id == templateId && !templateAlias.IsDeleted)
                     .SelectList(select => select
                            .Select(() => templateAlias.Id).WithAlias(() => templateViewModelAlias.Id)
                            .Select(() => templateAlias.Version).WithAlias(() => templateViewModelAlias.Version)
                            .Select(() => templateAlias.Name).WithAlias(() => templateViewModelAlias.Name)
                            .Select(() => templateAlias.PreviewUrl).WithAlias(() => templateViewModelAlias.PreviewImageUrl)
                            .Select(() => templateAlias.LayoutPath).WithAlias(() => templateViewModelAlias.Url))

                    .TransformUsing(Transformers.AliasToBean<TemplateEditViewModel>())
                    .FutureValue<TemplateEditViewModel>();

                TemplateRegionItemViewModel templateRegionAlias = null;
                LayoutRegion layoutRegionAlias = null;
                Region regionAlias = null;

                var regions = UnitOfWork.Session
                    .QueryOver(() => layoutRegionAlias)
                    .Inner.JoinAlias(c => c.Region, () => regionAlias)
                    .Where(() => layoutRegionAlias.Layout.Id == templateId && !regionAlias.IsDeleted && !layoutRegionAlias.IsDeleted)
                    .SelectList(select => select
                        .Select(() => regionAlias.Id).WithAlias(() => templateRegionAlias.Id)
                        .Select(() => layoutRegionAlias.Description).WithAlias(() => templateRegionAlias.Description)
                        .Select(() => regionAlias.Version).WithAlias(() => templateRegionAlias.Version)
                        .Select(() => regionAlias.RegionIdentifier).WithAlias(() => templateRegionAlias.Identifier))
                    .TransformUsing(Transformers.AliasToBean<TemplateRegionItemViewModel>())
                    .Future<TemplateRegionItemViewModel>();

                templateModel = templateFuture.Value;
                if (templateModel == null)
                {
                    throw new EntityNotFoundException(typeof(TemplateRegionItemViewModel), templateId.Value);
                }

                templateModel.Regions = regions.ToList();
                templateModel.Options = layoutService.GetLayoutOptions(templateId.Value);
            }

            templateModel.CustomOptions = optionService.GetCustomOptions();

            return templateModel;
        }
    }
}