// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LanguagesService.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Helpers;
using BetterCms.Module.Api.Infrastructure;
using BetterCms.Module.Api.Operations.Root.Languages.Language;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Languages
{
    /// <summary>
    /// Language service for languages list handling.
    /// </summary>
    public class LanguagesService : Service, ILanguagesService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IRepository repository;

        /// <summary>
        /// The language service.
        /// </summary>
        private readonly ILanguageService languageService;

        public LanguagesService(IRepository repository, ILanguageService languageService)
        {
            this.repository = repository;
            this.languageService = languageService;
        }

        public GetLanguagesResponse Get(GetLanguagesRequest request)
        {
            request.Data.SetDefaultOrder("Name");

            var listResponse = repository
                .AsQueryable<Module.Root.Models.Language>()
                .Select(language => new LanguageModel
                    {
                        Id = language.Id,
                        Version = language.Version,
                        CreatedBy = language.CreatedByUser,
                        CreatedOn = language.CreatedOn,
                        LastModifiedBy = language.ModifiedByUser,
                        LastModifiedOn = language.ModifiedOn,

                        Name = language.Name,
                        Code = language.Code
                    })
                .ToDataListResponse(request);

            return new GetLanguagesResponse
                       {
                           Data = listResponse
                       };
        }

        public PostLanguageResponse Post(PostLanguageRequest request)
        {
            var result =
                languageService.Put(
                    new PutLanguageRequest
                    {
                        Data = request.Data,
                        User = request.User
                    });

            return new PostLanguageResponse { Data = result.Data };
        }
    }
}