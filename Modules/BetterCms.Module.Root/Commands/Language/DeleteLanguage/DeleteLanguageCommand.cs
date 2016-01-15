// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteLanguageCommand.cs" company="Devbridge Group LLC">
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

using BetterCms.Core.Exceptions.Mvc;

using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Language;

using BetterModules.Core.Exceptions.DataTier;
using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.Language.DeleteLanguage
{
    public class DeleteLanguageCommand : CommandBase, ICommand<LanguageViewModel, bool>
    {
        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>True</c>, if language is deleted successfully.</returns>
        public bool Execute(LanguageViewModel request)
        {
            var language = Repository.First<Models.Language>(request.Id);
            if (Repository.AsQueryable<Models.Page>(p => p.Language == language).Any())
            {
                var logMessage = string.Format("Cannot delete language {0}, because it's used in pages.", language.Name);
                var message = string.Format(RootGlobalization.DeleteLanguageCommand_PagesAreUsingLanguage_Message, language.Name);
                throw new ValidationException(() => message, logMessage);
            }

            if (language.Version != request.Version)
            {
                throw new ConcurrentDataException(language);
            }

            Repository.Delete(language);
            UnitOfWork.Commit();

            Events.RootEvents.Instance.OnLanguageDeleted(language);

            return true;
        }
    }
}