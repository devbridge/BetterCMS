using System;
using System.Collections.Generic;

namespace BetterCms.Core.DataContracts
{
    public interface IRenderPage : IPage
    {
        IEnumerable<IOptionValue> Options { get; }
        
        IDictionary<string, IOptionValue> OptionsAsDictionary { get; }
        
        string LanguageCode { get; set; }

        Guid LanguageId { get; set; }
    }
}
