using System;
using System.Linq;

using BetterCms.Core.DataAccess;
using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.DataContracts.Enums;
using BetterCms.Core.Exceptions.Api;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Contents.Content;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Exists;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties;
using BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations;

using BetterCms.Module.MediaManager.Services;
using BetterCms.Module.Pages.Services;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.Mvc.Helpers;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page
{
    public class PageService : Service, IPageService
    {
        private readonly IPagePropertiesService pagePropertiesService;

        private readonly IPageExistsService pageExistsService;

        private readonly IPageContentsService pageContentsService;

        private readonly IPageContentService pageContentService;
        
        private readonly IPageTranslationsService pageTranslationsService;

        private readonly IMasterPageService masterPageService;

        private readonly ICmsConfiguration cmsConfiguration;

        private readonly Module.Pages.Services.IPageService pageService;

        private readonly IRepository repository;

        private readonly IUnitOfWork unitOfWork;

        private readonly IUrlService urlService;

        private readonly IMediaFileUrlResolver fileUrlResolver;

        public PageService(
            IRepository repository,
            IUnitOfWork unitOfWork,
            IPagePropertiesService pagePropertiesService,
            IPageExistsService pageExistsService,
            IPageContentsService pageContentsService,
            IPageContentService pageContentService,
            IUrlService urlService,
            IMediaFileUrlResolver fileUrlResolver,
            IPageTranslationsService pageTranslationsService,
            IMasterPageService masterPageService,
            ICmsConfiguration cmsConfiguration,
            Module.Pages.Services.IPageService pageService)
        {
            this.pageContentsService = pageContentsService;
            this.pageContentService = pageContentService;
            this.pagePropertiesService = pagePropertiesService;
            this.pageExistsService = pageExistsService;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.urlService = urlService;
            this.fileUrlResolver = fileUrlResolver;
            this.pageTranslationsService = pageTranslationsService;
            this.masterPageService = masterPageService;
            this.cmsConfiguration = cmsConfiguration;
            this.pageService = pageService;
        }

        public GetPageResponse Get(GetPageRequest request)
        {
            var query = repository.AsQueryable<Module.Pages.Models.PageProperties>();
            
            if (request.PageId.HasValue)
            {
                query = query.Where(page => page.Id == request.PageId.Value);
            }
            else
            {
                var url = urlService.FixUrl(request.PageUrl);
                query = query.Where(page => page.PageUrlHash == url.UrlHash());
            }

            var model = query
                .Select(page => new PageModel
                    {
                        Id = page.Id,
                        Version = page.Version,
                        CreatedBy = page.CreatedByUser,
                        CreatedOn = page.CreatedOn,
                        LastModifiedBy = page.ModifiedByUser,
                        LastModifiedOn = page.ModifiedOn,

                        PageUrl = page.PageUrl,
                        Title = page.Title,
                        Description = page.Description,
                        IsPublished = page.Status == PageStatus.Published,
                        PublishedOn = page.PublishedOn,
                        LayoutId = page.Layout != null && !page.Layout.IsDeleted ? page.Layout.Id : (Guid?)null,
                        MasterPageId = page.MasterPage != null && !page.MasterPage.IsDeleted ? page.MasterPage.Id : (Guid?)null,
                        CategoryId = page.Category != null && !page.Category.IsDeleted ? page.Category.Id : (Guid?)null,
                        CategoryName = page.Category != null && !page.Category.IsDeleted ? page.Category.Name : null,
                        MainImageId = page.Image != null && !page.Image.IsDeleted ? page.Image.Id : (Guid?)null,
                        MainImageUrl = page.Image != null && !page.Image.IsDeleted ? page.Image.PublicUrl : null,
                        MainImageThumbnauilUrl = page.Image != null && !page.Image.IsDeleted ? page.Image.PublicThumbnailUrl : null,
                        MainImageCaption = page.Image != null && !page.Image.IsDeleted ? page.Image.Caption : null,
                        IsArchived = page.IsArchived,
                        IsMasterPage = page.IsMasterPage,
                        LanguageId = page.Language != null ? page.Language.Id : (Guid?)null,
                        LanguageCode = page.Language != null ? page.Language.Code : null,
                        LanguageGroupIdentifier = page.LanguageGroupIdentifier
                    })
                .FirstOne();

            model.MainImageUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageUrl);
            model.MainImageThumbnauilUrl = fileUrlResolver.EnsureFullPathUrl(model.MainImageThumbnauilUrl);

            return new GetPageResponse { Data = model };
        }

        public PutPageResponse Put(PutPageRequest request)
        {
            Module.Pages.Models.PageProperties page = null;
            if (request.PageId.HasValue && !request.PageId.Value.HasDefaultValue())
            {
                page = repository.AsQueryable<Module.Pages.Models.PageProperties>().FirstOrDefault(p => p.Id == request.PageId.Value);
            }

            if (page == null)
            {
                page = new Module.Pages.Models.PageProperties();

                if (request.PageId.HasValue && !request.PageId.Value.HasDefaultValue())
                {
                    page.Id = request.PageId.Value;
                }
            }

            var pageUrl = request.Data.PageUrl;
            if (pageUrl == null && !string.IsNullOrWhiteSpace(request.Data.Title))
            {
                pageUrl = pageService.CreatePagePermalink(request.Data.Title, null);
            }
            else
            {
                pageUrl = urlService.FixUrl(pageUrl);
                pageService.ValidatePageUrl(pageUrl, request.PageId);
            }

            page.PageUrl = pageUrl;
            page.PageUrlHash = pageUrl.UrlHash();
            page.Title = request.Data.Title;
            page.Description = request.Data.Description;
            page.Status = request.Data.IsMasterPage || request.Data.IsPublished ? PageStatus.Published : PageStatus.Unpublished;
            page.PublishedOn = request.Data.PublishedOn;
            page.IsArchived = request.Data.IsArchived;
            page.IsMasterPage = request.Data.IsMasterPage;

            if (request.Data.MasterPageId.HasValue && !request.Data.MasterPageId.Value.HasDefaultValue())
            {
                page.MasterPage = repository.AsProxy<Module.Root.Models.Page>(request.Data.MasterPageId.Value);
                masterPageService.SetPageMasterPages(page, request.Data.MasterPageId.Value);
            }
            else
            {
                page.Layout = repository.AsProxy<Module.Root.Models.Layout>(request.Data.LayoutId.Value);
            }

            if (request.Data.CategoryId.HasValue && !request.Data.CategoryId.Value.HasDefaultValue())
            {
                page.Category = repository.AsProxy<Module.Root.Models.Category>(request.Data.CategoryId.Value);
            }

            if (request.Data.MainImageId.HasValue && !request.Data.MainImageId.Value.HasDefaultValue())
            {
                page.Image = repository.AsProxy<Module.MediaManager.Models.MediaImage>(request.Data.MainImageId.Value);
            }

            if (cmsConfiguration.EnableMultilanguage)
            {
                if (request.Data.LanguageId.HasValue && !request.Data.LanguageId.Value.HasDefaultValue())
                {
                    page.Language = repository.AsProxy<Module.Root.Models.Language>(request.Data.LanguageId.Value);
                }
            }

            // TODO: save options.
            // page.Options = optionService.SaveOptionValues(request.OptionValues, null, () => new PageOption { Page = page });

            page.SaveUnsecured = true;

            unitOfWork.BeginTransaction();
            repository.Save(page);
            unitOfWork.Commit();

            return new PutPageResponse { Data = Get(new GetPageRequest { PageId = request.Data.Id }).Data };
        }

        public DeletePageResponse Delete(DeletePageRequest request)
        {
            var page = repository.First<Module.Pages.Models.PageProperties>(request.PageId.GetValueOrDefault());

            if (page.IsMasterPage && repository.AsQueryable<Module.Root.Models.MasterPage>(mp => mp.Master == page).Any())
            {
                var logMessage = string.Format("Failed to delete page. Page is selected as master page. Id: {0} Url: {1}", page.Id, page.PageUrl);
                throw new CmsApiValidationException(logMessage);
            }

            page.SaveUnsecured = true;

            unitOfWork.BeginTransaction();

            if (page.PageTags != null)
            {
                foreach (var pageTag in page.PageTags)
                {
                    repository.Delete(pageTag);
                }
            }

            if (page.PageContents != null)
            {
                foreach (var pageContent in page.PageContents)
                {
                    repository.Delete(pageContent);
                }
            }

            if (page.Options != null)
            {
                foreach (var option in page.Options)
                {
                    repository.Delete(option);
                }
            }

            if (page.AccessRules != null)
            {
                var rules = page.AccessRules.ToList();
                rules.ForEach(page.RemoveRule);
            }

            if (page.MasterPages != null)
            {
                foreach (var master in page.MasterPages)
                {
                    repository.Delete(master);
                }
            }

            repository.Delete<Module.Root.Models.Page>(page);

            unitOfWork.Commit();

            return new DeletePageResponse { Data = true };
        }

        IPagePropertiesService IPageService.Properties
        {
            get
            {
                return pagePropertiesService;
            }
        }

        PageExistsResponse IPageService.Exists(PageExistsRequest request)
        {
            return pageExistsService.Get(request);
        }

        IPageContentsService IPageService.Contents
        {
            get
            {
                return pageContentsService;
            }
        }

        IPageContentService IPageService.Content
        {
            get
            {
                return pageContentService;
            }
        }

        IPageTranslationsService IPageService.Translations
        {
            get
            {
                return pageTranslationsService;
            }
        }
    }
}