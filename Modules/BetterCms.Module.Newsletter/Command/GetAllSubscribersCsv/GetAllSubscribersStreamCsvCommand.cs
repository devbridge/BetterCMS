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