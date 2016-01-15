// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChildContent.cs" company="Devbridge Group LLC">
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
using System.Collections.Generic;

using BetterCms.Core.DataContracts;

using BetterModules.Core.Models;

namespace BetterCms.Module.Root.Models
{
    [Serializable]
    public class ChildContent : EquatableEntity<ChildContent>, IChildContent
    {
        public virtual Content Parent { get; set; }

        public virtual Content Child { get; set; }
        
        public virtual Guid AssignmentIdentifier { get; set; }

        public virtual IList<ChildContentOption> Options { get; set; }

        IContent IChildContent.ChildContent
        {
            get
            {
                return Child;
            }
            set
            {
                Child = (Content)value;
            }
        }

        IEnumerable<IOptionValueEntity> IChildContent.Options
        {
            get
            {
                return Options;
            }
        }

        public virtual ChildContent Clone()
        {
            return CopyDataTo(new ChildContent());
        }

        public virtual ChildContent CopyDataTo(ChildContent targetChildContent, bool copyOptions = true)
        {
            targetChildContent.Id = Id;
            targetChildContent.Version = Version;
            targetChildContent.Child = Child;
            targetChildContent.Parent = Parent;
            targetChildContent.AssignmentIdentifier = AssignmentIdentifier;

            if (copyOptions && Options != null)
            {
                if (targetChildContent.Options == null)
                {
                    targetChildContent.Options = new List<ChildContentOption>();
                }

                foreach (var childContentOption in Options)
                {
                    var clonedOption = childContentOption.Clone();
                    clonedOption.ChildContent = targetChildContent;

                    targetChildContent.Options.Add(clonedOption);
                }
            }

            return targetChildContent;
        }
    }
}