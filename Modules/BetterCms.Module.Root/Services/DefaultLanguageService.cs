// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultLanguageService.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;
using System.Linq;

using BetterModules.Core.DataAccess;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Models;

using NHibernate.Linq;

namespace BetterCms.Module.Root.Services
{
    public class DefaultLanguageService : ILanguageService
    {
        /// <summary>
        /// The repository
        /// </summary>
        private IRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLanguageService" /> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public DefaultLanguageService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Gets the list of languages.
        /// </summary>
        /// <returns>
        /// List of language lookup values.
        /// </returns>
        public IEnumerable<LookupKeyValue> GetLanguagesLookupValues()
        {
            return repository
                .AsQueryable<Language>()
                .OrderBy(c => c.Code)
                .Select(c => new LookupKeyValue
                                 {
                                     Key = c.Id.ToString().ToLowerInvariant(),
                                     Value = c.Name
                                 })
                .ToFuture();
        }

        /// <summary>
        /// Gets the list of languages.
        /// </summary>
        /// <returns>
        /// List of languages.
        /// </returns>
        public IEnumerable<Language> GetLanguages()
        {
            return repository
                .AsQueryable<Language>()
                .OrderBy(c => c.Code)
                .ToFuture();
        }

        /// <summary>
        /// Gets the invariant language model.
        /// </summary>
        /// <returns>
        /// Invariant language model
        /// </returns>
        public LookupKeyValue GetInvariantLanguageModel()
        {
            return new LookupKeyValue(System.Guid.Empty.ToString(), RootGlobalization.InvariantLanguage_Title);
        }
    }
}