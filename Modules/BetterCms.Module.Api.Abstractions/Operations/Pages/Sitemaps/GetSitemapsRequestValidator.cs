// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetSitemapsRequestValidator.cs" company="Devbridge Group LLC">
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
using BetterCms.Module.Api.Infrastructure;

using ServiceStack.FluentValidation;

namespace BetterCms.Module.Api.Operations.Pages.Sitemaps
{
    /// <summary>
    /// Sitemaps list get validator.
    /// </summary>
    public class GetSitemapsRequestValidator : AbstractValidator<GetSitemapsRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSitemapsRequestValidator"/> class.
        /// </summary>
        public GetSitemapsRequestValidator()
        {
            RuleFor(f => f.Data).Must(TagsListIsReadonly).WithMessage("An Tags field is a list. You can't sort or add filter by this column.");
        }

        /// <summary>
        /// Tags the list is readonly.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="data">The data.</param>
        /// <returns><c>true</c> validation passes, <c>false</c> otherwise.</returns>
        private bool TagsListIsReadonly(GetSitemapsRequest request, GetSitemapsModel data)
        {
            return !data.HasColumnInSortBySection("Tags") && !data.HasColumnInWhereSection("Tags");            
        }
    }
}
