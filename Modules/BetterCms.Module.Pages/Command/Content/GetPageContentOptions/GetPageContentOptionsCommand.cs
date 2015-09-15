using System;
using System.Collections.Generic;
using System.Linq;

using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Core.Security;

using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Services;
using BetterCms.Module.Root.ViewModels.Option;

using FluentNHibernate.Testing.Values;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Content.GetPageContentOptions
{
    public class GetPageContentOptionsCommand : CommandBase, ICommand<Guid, ContentOptionValuesViewModel>
    {
        /// <summary>
        /// Gets or sets the option service.
        /// </summary>
        /// <value>
        /// The option service.
        /// </value>
        public IOptionService OptionService { get; set; }

        /// <summary>
        /// Gets or sets the CMS configuration.
        /// </summary>
        /// <value>
        /// The CMS configuration.
        /// </value>
        public ICmsConfiguration CmsConfiguration { get; set; }

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="pageContentId">The page content id.</param>
        /// <returns></returns>        
        public ContentOptionValuesViewModel Execute(Guid pageContentId)
        {
            var model = new ContentOptionValuesViewModel
            {
                OptionValuesContainerId = pageContentId
            };

            if (!pageContentId.HasDefaultValue())
            {
                var contentQuery = Repository.AsQueryable<PageContent>()
                    .Where(f => f.Id == pageContentId && !f.IsDeleted && !f.Content.IsDeleted);

                IEnumerable<AccessRule> accessRules = new List<AccessRule>();
                if (CmsConfiguration.Security.AccessControlEnabled)
                {
                    accessRules = contentQuery.SelectMany(t => t.Page.AccessRules).ToFuture();
                }

                var contentHistory = contentQuery.SelectMany(t => t.Content.History).ToFuture();
                var contentOptions = contentQuery.SelectMany(t => t.Content.ContentOptions).ToFuture();
                var pageOptions = contentQuery.SelectMany(t => t.Options).ToFuture();
                contentQuery = contentQuery.Fetch(t => t.Content).FetchMany(t=>t.Options).ThenFetch(t=>t.CustomOption);
                var pageContent = contentQuery.ToFuture().FirstOrDefault();

                if (pageContent != null)
                {
                    pageContent.Content.History = contentHistory.ToList();
                    pageContent.Content.ContentOptions = contentOptions.ToList();
                    pageContent.Options = pageOptions.ToList();
                    var contentToProject = pageContent.Content;
                    if (contentToProject.Status != ContentStatus.Draft)
                    {
                        var draftContent = contentToProject.History.FirstOrDefault(c => c.Status == ContentStatus.Draft);
                        if (draftContent != null)
                        {
                            contentToProject = draftContent;
                        }
                    }

                    model.OptionValues = OptionService.GetMergedOptionValuesForEdit(contentToProject.ContentOptions, pageContent.Options);
                    model.CustomOptions = OptionService.GetCustomOptions();

                    if (CmsConfiguration.Security.AccessControlEnabled)
                    {
                        SetIsReadOnly(model, accessRules.Cast<IAccessRule>().ToList());
                    }
                }
            }

            return model;
        }        
    }
}