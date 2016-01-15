// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SaveTagCommand.cs" company="Devbridge Group LLC">
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
using System;

using BetterCms.Core.Exceptions.Mvc;
using BetterCms.Module.Root.Content.Resources;
using BetterCms.Module.Root.Mvc;
using BetterCms.Module.Root.ViewModels.Tags;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Root.Commands.Tag.SaveTag
{
    /// <summary>
    /// A command to save tag item.
    /// </summary>
    public class SaveTagCommand : CommandBase, ICommand<TagItemViewModel, TagItemViewModel>
    {
        /// <summary>
        /// Executes a command to save tag.
        /// </summary>
        /// <param name="tagItem">The tag item.</param>
        /// <returns>
        /// true if tag saved successfully; false otherwise.
        /// </returns>
        public TagItemViewModel Execute(TagItemViewModel tagItem)
        {
            Models.Tag tag;

            var tagName = Repository.FirstOrDefault<Models.Tag>(c => c.Name == tagItem.Name.Trim());
            if (tagName != null && tagName.Id != tagItem.Id)
            {
                var message = string.Format(RootGlobalization.SaveTag_TagExists_Message, tagItem.Name);
                var logMessage = string.Format("Tag already exists. Tag name: {0}, Id: {1}", tagItem.Name, tagItem.Id);

                throw new ValidationException(() => message, logMessage);
            }

            if (tagItem.Id == default(Guid))
            {
                tag = new Models.Tag();
            }
            else
            {
                tag = Repository.AsProxy<Models.Tag>(tagItem.Id);                
            }

            tag.Version = tagItem.Version;
            tag.Name = tagItem.Name.Trim();

            Repository.Save(tag);                       
            UnitOfWork.Commit();

            if (tagItem.Id == default(Guid))
            {
                Events.RootEvents.Instance.OnTagCreated(tag);
            }
            else
            {
                Events.RootEvents.Instance.OnTagUpdated(tag);
            }

            return new TagItemViewModel
                {
                    Id = tag.Id,
                    Version = tag.Version,
                    Name = tag.Name
                };
        }
    }
}