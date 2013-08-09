using System;

using BetterCms.Core.DataAccess;

using ServiceStack.ServiceInterface;

namespace BetterCms.Module.Api.Operations.Root.Layouts.Layout.Options
{
    public class LayoutOptionsService : Service, ILayoutOptionsService
    {
        private readonly IRepository repository;

        public LayoutOptionsService(IRepository repository)
        {
            this.repository = repository;
        }

        public GetLayoutOptionsResponse Get(GetLayoutOptionsRequest request)
        {
            throw new NotImplementedException("TODO: implement service");
        }
    }
}