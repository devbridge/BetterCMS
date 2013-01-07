using System;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.Templates;
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
                TemplateEditViewModel modelAlias = null;

                var templateFuture = UnitOfWork.Session.QueryOver(() => template)
                     .Where(() => template.Id == templateId).SelectList(select => select
                            .Select(() => template.Id).WithAlias(() => modelAlias.Id)
                            .Select(() => template.Version).WithAlias(() => modelAlias.Version)
                            .Select(() => template.Name).WithAlias(() => modelAlias.Name)
                            .Select(() => template.PreviewUrl).WithAlias(() => modelAlias.PreviewImageUrl)
                            .Select(() => template.LayoutPath).WithAlias(() => modelAlias.Url))
                    .TransformUsing(Transformers.AliasToBean<TemplateEditViewModel>())
                    .FutureValue<TemplateEditViewModel>();

                templateModel = templateFuture.Value;
            }

            return templateModel;
        }
    }
}