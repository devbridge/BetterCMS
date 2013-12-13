using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Page.AssignMainCulturePage
{
    public class AssignMainCulturePageCommand : CommandBase, ICommand<AssignMainCulturePageCommandRequest, AssignMainCulturePageCommandResponse>
    {
        public AssignMainCulturePageCommandResponse Execute(AssignMainCulturePageCommandRequest request)
        {
            // Get main page
            var mainPage = Repository
                .AsQueryable<Root.Models.Page>()
                .Where(p => p.Id == request.MainCulturePageId)
                .Select(p => new { MainCulturePageId = p.MainCulturePage != null ? p.MainCulturePage.Id : (System.Guid?)null })
                .FirstOne();
            var mainPageCultureId = mainPage.MainCulturePageId.HasValue && !mainPage.MainCulturePageId.Value.HasDefaultValue()
                                        ? mainPage.MainCulturePageId.Value
                                        : request.MainCulturePageId;

            var page = Repository.AsQueryable<Root.Models.Page>(p => p.Id == request.PageId).FirstOne();

            ValidatePage(page, request);

            page.MainCulturePage = Repository.AsProxy<Root.Models.Page>(mainPageCultureId);
            page.Culture = Repository.AsProxy<Root.Models.Culture>(request.CultureId);

            Repository.Save(page);
            UnitOfWork.Commit();

            return new AssignMainCulturePageCommandResponse
                       {
                           MainCulturePageId = mainPageCultureId,
                           Title = page.Title,
                           PageUrl = page.PageUrl
                       };
        }

        private void ValidatePage(Root.Models.Page page, AssignMainCulturePageCommandRequest request)
        {
            if (page.Culture == null || page.Culture.Id != request.CultureId)
            {
                // TODO: translate
                var message = "Wrong culture";

                throw new ValidationException(() => message, message);
            }
        }
    }
}