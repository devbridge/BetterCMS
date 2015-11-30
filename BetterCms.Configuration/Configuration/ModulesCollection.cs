// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModulesCollection.cs" company="Devbridge Group LLC">
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
using System.Configuration;

namespace BetterCms.Configuration
{
    [ConfigurationCollection(typeof(ModuleElement), AddItemName = "module")]
    public class ModulesCollection : ConfigurationElementCollection, IEnumerable<ModuleElement>
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ModuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ModuleElement)(element)).Name;
        }

        public ModuleElement this[int idx]
        {
            get { return (ModuleElement)BaseGet(idx); }
        }

        public ModuleElement GetByName(string moduleName)
        {
            var enumerator = GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (enumerator.Current.Name == moduleName) return enumerator.Current;
            }

            return null;
        }


        public new IEnumerator<ModuleElement> GetEnumerator()
        {
            var count = Count;
            for (var i = 0; i < count; i++)
            {
                yield return BaseGet(i) as ModuleElement;
            }
        }
    }
}
