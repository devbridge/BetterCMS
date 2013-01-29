using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;

using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Transform;

namespace BetterCms.Module.Pages.Command.Widget.GetServerControlWidgetForEdit
{
    /// <summary>
    /// A command to get widget by id for editing.
    /// </summary>
    public class GetServerControlWidgetForEditCommand : CommandBase, ICommand<Guid?, EditServerControlWidgetViewModel>
    {
        /// <summary>
        /// The category service
        /// </summary>
        private readonly ICategoryService categoryService;

        private readonly IContentService contentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetServerControlWidgetForEditCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        public GetServerControlWidgetForEditCommand(ICategoryService categoryService, IContentService contentService)
        {
            this.contentService = contentService;
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="widgetId">The request.</param>
        /// <returns>
        /// Executed command result.
        /// </returns>
        public EditServerControlWidgetViewModel Execute(Guid? widgetId)
        {            
            EditServerControlWidgetViewModel model = null;
            var categories = categoryService.GetCategories();

            if (widgetId != null)
            {
                var serverControlWidget = contentService.GetContentForEdit(widgetId.Value) as ServerControlWidget;
                if (serverControlWidget != null)
                {
                    model = new EditServerControlWidgetViewModel {
                                                                     Id = serverControlWidget.Id,
                                                                     Version = serverControlWidget.Version,
                                                                     Name = serverControlWidget.Name,
                                                                     Url = serverControlWidget.Url,
                                                                     PreviewImageUrl = serverControlWidget.PreviewUrl,
                                                                     CurrentStatus = serverControlWidget.Status,
                                                                     HasPublishedContent = serverControlWidget.Original != null,
                                                                     WidgetType = WidgetType.ServerControl,
                                                                     CategoryId = serverControlWidget.Category != null ? serverControlWidget.Category.Id : (Guid?)null
                                                                 };
                    model.ContentOptions = serverControlWidget.ContentOptions
                        .Select(
                            f => 
                                new ContentOptionViewModel
                                 {
                                     Type = f.Type,
                                     OptionDefaultValue = f.DefaultValue,
                                     OptionKey = f.Key
                                 })
                        .ToList();
                }
            }
            /*
            ServerControlWidgetViewModel modelAlias = null;
                Category categoryAlias = null;

                var widgetFuture = UnitOfWork.Session.QueryOver(() => serverControlWidget)
                    .JoinAlias(() => serverControlWidget.Category, () => categoryAlias, JoinType.LeftOuterJoin, Restrictions.Where(() => !categoryAlias.IsDeleted))
                    .Where(() => serverControlWidget.Id == widgetId)
                    .SelectList(select => select
                         .Select(() => serverControlWidget.Id).WithAlias(() => modelAlias.Id)
                         .Select(() => serverControlWidget.Version).WithAlias(() => modelAlias.Version)
                         .Select(() => serverControlWidget.Name).WithAlias(() => modelAlias.Name)
                         .Select(() => serverControlWidget.Url).WithAlias(() => modelAlias.Url)
                         .Select(() => serverControlWidget.PreviewUrl).WithAlias(() => modelAlias.PreviewImageUrl)
                         .Select(() => serverControlWidget.Status).WithAlias(() => modelAlias.CurrentStatus)

                         .Select(Projections.Conditional(Restrictions.Where(() => serverControlWidget.Original != null), 
                                 Projections.Constant(true, NHibernateUtil.Boolean),
                                 Projections.Constant(false, NHibernateUtil.Boolean))).WithAlias(() => modelAlias.HasPublishedContent)

                         .Select(() => categoryAlias.Id).WithAlias(() => modelAlias.CategoryId))
                    .TransformUsing(Transformers.AliasToBean<EditServerControlWidgetViewModel>())
                    .FutureValue<EditServerControlWidgetViewModel>();

                ContentOption contentOptionAlias = null;
                ContentOptionViewModel contentOptionModelAlias = null;

                var optionsFuture = UnitOfWork.Session.QueryOver(() => contentOptionAlias)
                        .Where(() => contentOptionAlias.Content.Id == widgetId.Value && !contentOptionAlias.IsDeleted)
                        .SelectList(select => select                            
                            .Select(() => contentOptionAlias.Key).WithAlias(() => contentOptionModelAlias.OptionKey)
                            .Select(() => contentOptionAlias.DefaultValue).WithAlias(() => contentOptionModelAlias.OptionDefaultValue)
                            .Select(() => contentOptionAlias.Type).WithAlias(() => contentOptionModelAlias.Type))
                        .TransformUsing(Transformers.AliasToBean<ContentOptionViewModel>())
                        .Future<ContentOptionViewModel>();

                model = widgetFuture.Value;
                model.ContentOptions = optionsFuture.ToList();
            }
            */
            
            if (model == null)
            {
                model = new EditServerControlWidgetViewModel();
            }

            model.Categories = categories.ToList();
            
            return model;
        }
    }
}