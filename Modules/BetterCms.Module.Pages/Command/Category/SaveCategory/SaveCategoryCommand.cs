using System;

using BetterCms.Api;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Content.Resources;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Category;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Commands.SaveCategory
{
    /// <summary>
    /// A command to save category item.
    /// </summary>
    public class SaveCategoryCommand : CommandBase, ICommand<CategoryItemViewModel, CategoryItemViewModel>
    {
        /// <summary>
        /// Executes a command to save category.
        /// </summary>
        /// <param name="categoryItem">The category item.</param>
        /// <returns>
        /// true if category saved successfully; false otherwise.
        /// </returns>
        public CategoryItemViewModel Execute(CategoryItemViewModel categoryItem)
        {
            Category category;

            var categoryName = Repository.FirstOrDefault<Category>(c => c.Name == categoryItem.Name);
            if (categoryName != null && categoryName.Id != categoryItem.Id)
            {
                var message = string.Format(PagesGlobalization.SaveCategory_CategoryExists_Message, categoryItem.Name);
                var logMessage = string.Format("Category already exists. Category name: {0}, Id: {1}", categoryItem.Name, categoryItem.Id);

                throw new ValidationException(() => message, logMessage);
            }   

            if (categoryItem.Id == default(Guid))
            {
                category = new Category();
            }
            else
            {
                category = Repository.AsProxy<Category>(categoryItem.Id);                
            }                     

            category.Version = categoryItem.Version;
            category.Name = categoryItem.Name;

            Repository.Save(category);                       
            UnitOfWork.Commit();

            if (categoryItem.Id == default(Guid))
            {
                PagesApiContext.Events.OnCategoryCreated(category);
            }
            else
            {
                PagesApiContext.Events.OnCategoryUpdated(category);
            }

            return new CategoryItemViewModel
                {
                    Id = category.Id,
                    Version = category.Version,
                    Name = category.Name
                };
        }
    }
}