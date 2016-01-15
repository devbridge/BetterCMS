// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListExtensions.cs" company="Devbridge Group LLC">
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

using BetterModules.Core.DataContracts;

namespace BetterCms.Module.Root.Models.Extensions
{
    public static class ListExtensions
    {
        public static void Add(this List<KeyValuePair<string, string>> list, string key, string value)
        {
             list.Add(new KeyValuePair<string, string>(key, value));
        }

        public static void RemoveDuplicates<T>(this IList<T> list, Comparison<T> comparison)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = list.Count - 1; j > i; j--)
                {
                    if (comparison(list[i], list[j]) == 0)
                    {
                        list.RemoveAt(j);
                    }
                }
            }
        }  
     
        public static void RemoveDuplicateEntities<T>(this IList<T> list) where T : IEntity
        {
            list.RemoveDuplicates((a, b) => a.Id == b.Id ? 0 : -1);
        }
    }
}