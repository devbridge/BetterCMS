using System;
using System.Runtime.Serialization;

using BetterCms.Module.Api.Infrastructure;

namespace BetterCms.Module.Api.Operations.Root.Tags.Tag
{
    [DataContract]
    public class PutTagResponse : ResponseBase<Guid?>
    {
    }
}