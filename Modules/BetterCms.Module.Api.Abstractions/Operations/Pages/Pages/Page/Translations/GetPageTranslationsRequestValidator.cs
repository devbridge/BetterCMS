// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetPageTranslationsRequestValidator.cs" company="Devbridge Group LLC">
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
using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Translations
{
    public class GetPageTranslationsRequestValidator : AbstractValidator<GetPageTranslationsRequest>
    {
        public GetPageTranslationsRequestValidator()
        {
            RuleFor(request => request.PageId).Must(PagePropertiesNameMustBeNullIfPagePropertiesIdProvided).WithMessage("A PageUrl field must be null if PageId is provided.");
            RuleFor(request => request.PageUrl).Must(AtLeastOneFieldShouldBeProvided).WithMessage("A PageId or PageUrl should be provided.");
        }

        private bool AtLeastOneFieldShouldBeProvided(GetPageTranslationsRequest request, string pageUrl)
        {
            return request.PageId != null || !string.IsNullOrEmpty(request.PageUrl);
        }

        private bool PagePropertiesNameMustBeNullIfPagePropertiesIdProvided(GetPageTranslationsRequest request, System.Guid? pageId)
        {
            return pageId != null && string.IsNullOrEmpty(request.PageUrl) ||
                   pageId == null && !string.IsNullOrEmpty(request.PageUrl);
        }
    }
}