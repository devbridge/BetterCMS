using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BetterCms.Module.Api.Operations
{
    [DataContract]
    [Serializable]
    public class OptionTranslationModel
    {
        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string LanguageId { get; set; }
    }
}
