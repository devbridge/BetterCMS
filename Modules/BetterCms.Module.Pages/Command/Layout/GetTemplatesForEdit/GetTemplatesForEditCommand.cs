using System;

using BetterCms.Core.Exceptions.DataTier;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.Templates;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Layout.GetTemplatesForEdit
{
    public class GetTemplatesForEditCommand : CommandBase, ICommand<Guid?, TemplateEditViewModel>
    {
        public TemplateEditViewModel Execute(Guid? templateId)
        {
            TemplateEditViewModel templateModel;

            if (templateId == null)
            {
                templateModel = new TemplateEditViewModel();
            }
            else
            {
                Root.Models.Layout template = null;
                TemplateRegionItemViewModel templateRegionAlias = null;

                TemplateEditViewModel templateAlias = null;

                var templateFuture = UnitOfWork.Session.QueryOver(() => template)
                     .Where(() => template.Id == templateId && !template.IsDeleted)
                     .SelectList(select => select
                            .Select(() => template.Id).WithAlias(() => templateAlias.Id)
                            .Select(() => template.Version).WithAlias(() => templateAlias.Version)
                            .Select(() => template.Name).WithAlias(() => templateAlias.Name)
                            .Select(() => template.PreviewUrl).WithAlias(() => templateAlias.PreviewImageUrl)
                            .Select(() => template.LayoutPath).WithAlias(() => templateAlias.Url))

                    .TransformUsing(Transformers.AliasToBean<TemplateEditViewModel>())
                    .FutureValue<TemplateEditViewModel>();

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
                      .List<TemplateRegionItemViewModel>();
                
                templateModel = templateFuture.Value;
                if (templateModel == null)
                {
                    throw new EntityNotFoundException(typeof(TemplateRegionItemViewModel), templateId.Value);
                }

                templateModel.RegionOptions = regions;
    
            }

            return templateModel;
        }
    }
}