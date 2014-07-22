using System;

namespace BetterCms.Core.DataContracts
{
    public interface ICustomOption
    {
        Guid Id { get; set; }

        string Title { get; set; }
        
        string Identifier { get; set; }
    }
}
