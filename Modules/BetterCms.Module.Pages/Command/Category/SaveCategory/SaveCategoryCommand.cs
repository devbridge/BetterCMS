using System;

using BetterCms.Core.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Pages.ViewModels.Category;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

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

            return new CategoryItemViewModel
                {
                    Id = category.Id,
                    Version = category.Version,
                    Name = category.Name
                };
        }
    }
}