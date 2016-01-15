// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseRefresher.cs" company="Devbridge Group LLC">
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
using System.IO;

namespace BetterCms.Api.Tests.Tools
{
    public class DatabaseRefresher
    {
        private const string OriginalDatabaseFilename = "BetterCmsTestsDataSet";

        private const string ConnectionStringPattern = @"Data Source=(LocalDb)\v11.0; Initial Catalog={0}; Integrated Security=SSPI; AttachDBFilename=|DataDirectory|\Temp\{0}.mdf";

        private readonly string originalDatabasePath;

        private readonly string tempBasePath;

        public string CurrentConnectionString
        {
            get; private set;
        }

        public DatabaseRefresher(string originalDatabasePath, string tempBasePath)
        {
            this.tempBasePath = tempBasePath;
            this.originalDatabasePath = originalDatabasePath;
        }

        public void BindNewDatabase()
        {            
            string newName = string.Format("BetterCmsTestsDataSet_Temp_{0}", DateTime.Now.Ticks);
            CurrentConnectionString = string.Format(ConnectionStringPattern, newName);

            File.Copy(Path.Combine(originalDatabasePath, OriginalDatabaseFilename + ".mdf"), Path.Combine(tempBasePath, newName + ".mdf"));
            File.Copy(Path.Combine(originalDatabasePath, OriginalDatabaseFilename + "_log.ldf"), Path.Combine(tempBasePath, newName + "_log.ldf"));            
        }

        public void ClearTempDatabases()
        {
        }
    }
}