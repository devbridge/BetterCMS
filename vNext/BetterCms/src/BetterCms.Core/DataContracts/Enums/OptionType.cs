using System;

namespace BetterCms.Core.DataContracts.Enums
{
    [Serializable]
    public enum OptionType
    {
        Text = 1,

        Integer = 2,

        Float = 3,

        DateTime = 4,

        Boolean = 5,

        MultilineText = 21,

        JavaScriptUrl = 51,
        
        CssUrl = 52,

        Custom = 99
    }
}