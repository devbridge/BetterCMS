using System;
using System.Linq;

using BetterCms.Module.Blog.Models;
using BetterCms.Module.Blog.ViewModels.Setting;
using BetterCms.Module.Pages.Models.Enums;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Blog.Commands.SaveBlogPostSetting
{
    public class SaveBlogPostSettingCommand : CommandBase, ICommand<SettingItemViewModel, SettingItemViewModel>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>True</c>, if save successful</returns>
        public SettingItemViewModel Execute(SettingItemViewModel request)
        {
            var option = Repository.AsQueryable<Option>().OrderByDescending(o => o.CreatedOn).FirstOrDefault(o => !o.IsDeleted);
            if (option == null)
            {
                option = new Option();
            }

            switch (request.Key)
            {
                case BlogModuleConstants.BlogPostDefaultContentTextModeKey :
                    option.DefaultContentTextMode = (ContentTextMode)request.Value;
                    break;

                default:
                    throw new NotImplementedException(string.Format("Unknown blog setting: {0}", request.Key));
            }

            Repository.Save(option);
            UnitOfWork.Commit();

            return request;
        }
    }
}