using System;
using System.Linq;

using BetterCms.Core.DataAccess.DataContext;
using BetterCms.Core.Mvc.Commands;
using BetterCms.Module.Pages.Models;
using BetterCms.Module.Root.Mvc;

namespace BetterCms.Module.Pages.Command.Content.GetChildContentType
{
    public class GetChildContentTypeCommand : CommandBase, ICommand<Guid, GetChildContentTypeCommandResponse>
    {
        public GetChildContentTypeCommandResponse Execute(Guid request)
        {
            var result =  Repository
                .AsQueryable<Root.Models.ChildContent>(w => w.Id == request)
                .Select(w => new
                             {
                                 Id = w.Child.Id,
                                 Type = w.Child.GetType()
                             })
                 .FirstOne();

            var response = new GetChildContentTypeCommandResponse { Id = result.Id };
            if (typeof(ServerControlWidget).IsAssignableFrom(result.Type))
            {
                response.Type = WidgetType.ServerControl.ToString();
            }
            else if (typeof(HtmlContentWidget).IsAssignableFrom(result.Type))
            {
                response.Type = WidgetType.HtmlContent.ToString();
            }

            return response;
        }
    }
}