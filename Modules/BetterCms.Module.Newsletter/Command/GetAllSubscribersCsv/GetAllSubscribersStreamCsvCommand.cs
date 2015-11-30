// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GetAllSubscribersStreamCsvCommand.cs" company="Devbridge Group LLC">
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
using System.Linq;
using System.Text;

using BetterCms.Module.Newsletter.Models;
using BetterCms.Module.Root.Mvc;

using BetterModules.Core.Web.Mvc.Commands;

namespace BetterCms.Module.Newsletter.Command.GetAllSubscribersCsv
{
    public class GetAllSubscribersStreamCsvCommand : CommandBase, ICommand<Guid?, Stream>
    {
        public Stream Execute(Guid? id)
        {
            var query = Repository.AsQueryable<Subscriber>().Where(t => !t.IsDeleted);
            if (id.HasValue)
            {
                query = query.Where(t => t.Id == id);
            }
            var subscribers = query.ToList();
            var sb = new StringBuilder();
            sb.Append("E-mail");
            sb.Append("\n");
            foreach (var subscriber in subscribers)
            {
                sb.Append(EscapeCsvField(subscriber.Email));
                sb.Append("\n");
            }
            return GenerateStreamFromString(sb.ToString());
        }

        private Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private string EscapeCsvField(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }
            s = s.Replace("\"", "\"\"");
            return string.Concat("\"", s, "\"");
        }
    }
}