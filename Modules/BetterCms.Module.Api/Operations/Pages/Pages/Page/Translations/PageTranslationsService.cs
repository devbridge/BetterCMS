// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PageTranslationsService.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System.Linq;

using BetterModules.Core.DataAccess;
using BetterCms.Core.DataContracts.Enums;

using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Pages.Services;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations
{
    public class PageTranslationsService : Service, IPageTranslationsService
    {
        private readonly IRepository repository;
        
        private readonly IUrlService urlService;

        public PageTranslationsService(IRepository repository, IUrlService urlService)
        {
            this.repository = repository;
            this.urlService = urlService;
        }

        public GetPageTranslationsResponse Get(GetPageTranslationsRequest request)
        {
            var languageGroupIdentifier = GetPageLanguageGroupIdentifier(request);

            // Page has no translations
            if (!languageGroupIdentifier.HasValue)
            {
                return new GetPageTranslationsResponse { Data = new DataListResponse<PageTranslationModel>() };
            }

            request.Data.SetDefaultOrder("Title");

            // Load translations
            var query = repository
                .AsQueryable<Module.Pages.Models.PageProperties>()
                .Where(p => p.LanguageGroupIdentifier == languageGroupIdentifier.Value);

            var dataListResult = query.Select(p => new PageTranslationModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    PageUrl = p.PageUrl,
                    LanguageId = p.Language != null ? p.Language.Id : (System.Guid?)null,
                    LanguageCode = p.Language != null ? p.Language.Code : null,
                    IsPublished = p.Status == PageStatus.Published,
                    PublishedOn = p.PublishedOn
                }).ToDataListResponse(request);

            return new GetPageTranslationsResponse { Data = dataListResult };
        }

        private System.Guid? GetPageLanguageGroupIdentifier(GetPageTranslationsRequest request)
        {
            // Get page language group identifier
            var query = repository.AsQueryable<Module.Pages.Models.PageProperties>();
            if (request.PageId.HasValue)
            {
                query = query.Where(p => p.Id == request.PageId.Value);
            }
            else
            {
                var url = urlService.FixUrl(request.PageUrl);
                query = query.Where(p => p.PageUrl == url);
            }
            
            return query.Select(p => p.LanguageGroupIdentifier).FirstOrDefault();
        }
    }
}