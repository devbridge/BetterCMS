using System;
using System.Linq;

using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.ViewModels.Content;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

using ContentEntity = BetterCms.Module.Root.Models.Content;

namespace BetterCms.Module.Pages.Command.Content.SavePageContentOptions
{
    public class SavePageContentOptionsCommand : CommandBase, ICommand<PageContentOptionsViewModel, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public bool Execute(PageContentOptionsViewModel model)
        {
            if (model != null && !model.PageContentId.HasDefaultValue() && model.OptionValues != null)
            {
                var pageContent = Repository.AsQueryable<PageContent>()
                              .Where(f => f.Id == model.PageContentId && !f.IsDeleted && !f.Content.IsDeleted)
                              .Fetch(f => f.Content)
                              .ThenFetchMany(f => f.ContentOptions)
                              .FetchMany(f => f.Options)
                              .ToList()
                              .FirstOrDefault();

                if (pageContent != null)
                {
                    UnitOfWork.BeginTransaction();

                    foreach (var widgetOption in model.OptionValues)
                    {
                        var pageContentOption = pageContent.Options.FirstOrDefault(f => f.Key.Trim().Equals(widgetOption.OptionKey.Trim(), StringComparison.OrdinalIgnoreCase));

                        if (!string.IsNullOrEmpty(widgetOption.OptionValue) && widgetOption.OptionValue != widgetOption.OptionDefaultValue)
                        {
                            if (pageContentOption == null)
                            {
                                pageContentOption = new PageContentOption {
                                                                             PageContent = pageContent,
                                                                             Key = widgetOption.OptionKey,
                                                                             Value = widgetOption.OptionValue,
                                                                             Type = widgetOption.Type                                                                            
                                                                          };
                            }
                            else
                            {
                                pageContentOption.Value = widgetOption.OptionValue;
                                pageContentOption.Type = widgetOption.Type;
                            }
                            Repository.Save(pageContentOption);
                        }
                        else if (pageContentOption != null)
                        {
                            Repository.Delete(pageContentOption);
                        }
                    }

                    UnitOfWork.Commit();   
                }                
            }

            return true;
        }
    }
}