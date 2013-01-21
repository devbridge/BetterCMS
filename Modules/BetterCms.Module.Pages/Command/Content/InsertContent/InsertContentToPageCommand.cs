using System.Linq;

using BetterCms.Core.Exceptions;
using BetterCms.Core.Models;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Models;
using BetterCms.Module.Root.Mvc;

using NHibernate.Linq;

namespace BetterCms.Module.Pages.Command.Content.InsertContent
{
    public class InsertContentToPageCommand : CommandBase, ICommand<InsertContentToPageRequest, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public bool Execute(InsertContentToPageRequest request)
        {
            UnitOfWork.BeginTransaction();
            
            var page = Repository.AsProxy<Root.Models.Page>(request.PageId);            
            var region = Repository.AsProxy<Region>(request.RegionId);
            var content = Repository.AsQueryable<Root.Models.Content>()
                            .Where(f => f.Id == request.ContentId)
                            .FetchMany(f => f.ContentOptions)
                            .ToList()
                            .FirstOrDefault();

            if (content == null)
            {
                throw new CmsException(string.Format("Content was not found by id={0}.", request.ContentId));
            }

            var pageContent = new PageContent
            {
                Page = page,
                Content = content,
                Region = region,

                // TODO: set correct status
                Status = ContentStatus.Published
            };

            foreach (var contentOption in content.ContentOptions)
            {
                PageContentOption pageContentOption = new PageContentOption();
                pageContentOption.ContentOption = contentOption;
                pageContentOption.PageContent = pageContent;
                pageContentOption.Value = contentOption.DefaultValue;

                Repository.Save(pageContentOption);
            }

            Repository.Save(pageContent);
            UnitOfWork.Commit();

            return true;
        }
    }
}