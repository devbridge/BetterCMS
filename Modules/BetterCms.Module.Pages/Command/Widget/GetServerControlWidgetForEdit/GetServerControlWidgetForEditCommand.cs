using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Pages.ViewModels.Widgets;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="GetServerControlWidgetForEditCommand" /> class.
        /// </summary>
        /// <param name="categoryService">The category service.</param>
        public GetServerControlWidgetForEditCommand(ICategoryService categoryService)
        {
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
            var categories = categoryService.GetCategories();
            EditServerControlWidgetViewModel serverControlWidget;
            
            if (widgetId == null)
            {
                serverControlWidget = new EditServerControlWidgetViewModel();
            }
            else 
            {
                ServerControlWidget widget = null;
                ServerControlWidgetViewModel modelAlias = null;
                Category categoryAlias = null;

                var widgetFuture = UnitOfWork.Session.QueryOver(() => widget)
                    .JoinAlias(() => widget.Category, () => categoryAlias, JoinType.LeftOuterJoin, Restrictions.Where(() => !categoryAlias.IsDeleted))
                    .Where(() => widget.Id == widgetId)
                    .SelectList(select => select
                         .Select(() => widget.Id).WithAlias(() => modelAlias.Id)
                         .Select(() => widget.Version).WithAlias(() => modelAlias.Version)
                         .Select(() => widget.Name).WithAlias(() => modelAlias.Name)
                         .Select(() => widget.Url).WithAlias(() => modelAlias.Url)
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

                serverControlWidget = widgetFuture.Value;
                serverControlWidget.ContentOptions = optionsFuture.ToList();
            }
            
            serverControlWidget.Categories = categories.ToList();
            
            return serverControlWidget;
        }
    }
}