// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetPagePropertiesRequestValidator.cs" company="Devbridge Group LLC">
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

namespace BetterCms.Module.Api.Operations.Pages.Pages.Page.Properties
{
    public class GetPagePropertiesRequestValidator : AbstractValidator<GetPagePropertiesRequest>
    {
        public GetPagePropertiesRequestValidator()
        {
            RuleFor(request => request.PageId).Must(PagePropertiesNameMustBeNullIfPagePropertiesIdProvided).WithMessage("A PageUrl field must be null if PageId is provided.");
            RuleFor(request => request.PageUrl).Must(AtLeastOneFieldShouldBeProvided).WithMessage("A PageId or PageUrl should be provided.");
        }

        private bool AtLeastOneFieldShouldBeProvided(GetPagePropertiesRequest getPagePropertiesRequest, string pageUrl)
        {
            return getPagePropertiesRequest.PageId != null || !string.IsNullOrEmpty(getPagePropertiesRequest.PageUrl);
        }

        private bool PagePropertiesNameMustBeNullIfPagePropertiesIdProvided(GetPagePropertiesRequest getPagePropertiesRequest, System.Guid? pageId)
        {
            return pageId != null && string.IsNullOrEmpty(getPagePropertiesRequest.PageUrl) ||
                   pageId == null && !string.IsNullOrEmpty(getPagePropertiesRequest.PageUrl);
        }
    }
}