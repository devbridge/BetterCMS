// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CmsCacheConfigurationElement.cs" company="Devbridge Group LLC">
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
using System.Configuration;

namespace BetterCms.Configuration
{
    [ConfigurationCollection(typeof(KeyValueElement), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class CmsCacheConfigurationElement : ConfigurationElementCollection, ICmsCacheConfiguration
    {
        private const string CacheTypeAttribute = "cacheType";

        private const string EnabledAttribute = "enabled";

        private const string TimeoutAttribute = "timeout";

        [ConfigurationProperty(EnabledAttribute, IsRequired = false, DefaultValue = true)]
        public bool Enabled
        {
            get { return (bool)this[EnabledAttribute]; }
            set { this[EnabledAttribute] = value; }
        }

        [ConfigurationProperty(TimeoutAttribute, IsRequired = false, DefaultValue = "0:10:0")]
        public TimeSpan Timeout
        {
            get { return (TimeSpan)this[TimeoutAttribute]; }
            set { this[TimeoutAttribute] = value; }
        }

        [ConfigurationProperty(CacheTypeAttribute, IsRequired = false, DefaultValue = CacheServiceType.Auto)]
        public CacheServiceType CacheType
        {
            get { return (CacheServiceType)this[CacheTypeAttribute]; }
            set { this[CacheTypeAttribute] = value; }
        }    

        public KeyValueElement this[int index]
        {
            get
            {
                return (KeyValueElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new KeyValueElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as KeyValueElement).Key;
        }

        public string GetValue(string key)
        {
            var element = (KeyValueElement)BaseGet(key);
            return element == null ? null : element.Value;
        }
    }
}