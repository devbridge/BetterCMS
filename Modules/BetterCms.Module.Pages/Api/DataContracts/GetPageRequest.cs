using System;

using BetterCms.Module.Pages.DataContracts.Enums;

namespace BetterCms.Module.Pages.Api.DataContracts
{
    public class GetPageRequest
    {
        public GetPageRequest(Guid id, PageLoadableChilds loadChilds = PageLoadableChilds.All)
        {
            Id = id;
            LoadChilds = loadChilds;
        }

        public Guid Id { get; set; }

        public PageLoadableChilds LoadChilds { get; set; }
    }
}