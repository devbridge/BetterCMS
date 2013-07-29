using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Contents.Content.HtmlContent
{
    public class HtmlContentService : Service, IHtmlContentService
    {
        private readonly IRepository repository;

        public HtmlContentService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetHtmlContentResponse Get(GetHtmlContentRequest request)
        {
            var model = repository
                .AsQueryable<Module.Pages.Models.HtmlContent>(content => content.Id == request.ContentId)
                .Select(content => new HtmlContentModel
                    {
                        Id = content.Id,
                        Version = content.Version,
                        CreatedBy = content.CreatedByUser,
                        CreatedOn = content.CreatedOn,
                        LastModifiedBy = content.ModifiedByUser,
                        LastModifiedOn = content.ModifiedOn,

                        Name = content.Name,
                        ActivationDate = content.ActivationDate,
                        ExpirationDate = content.ExpirationDate,
                        Html = content.Html,
                        CustomCss = content.CustomCss,
                        UseCustomCss = content.UseCustomCss,
                        CustomJavaScript = content.CustomJs,
                        UseCustomJavaScript = content.UseCustomJs,
                        IsPublished = content.Status == ContentStatus.Published,
                        PublishedByUser = content.Status == ContentStatus.Published ? content.PublishedByUser : null,
                        PublishedOn = content.Status == ContentStatus.Published ? content.PublishedOn : null
                    })
                .FirstOne();

            return new GetHtmlContentResponse { Data = model };
        }
    }
}