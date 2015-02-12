using System;

using Devbridge.Platform.Core.Exceptions;

namespace Devbridge.Platform.Core.Web.Exceptions.Host
{
    [Serializable]
    public class RestartApplicationException : PlatformException
    {
        public RestartApplicationException(string message)
            : base(message)
        {
        }

        public RestartApplicationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
