using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using ServiceStack.Configuration;
using ServiceStack.OrmLite;
using ServiceStack.Common;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.WebHost.Endpoints;

namespace BetterCms.Module.Api
{
	//Request DTO
	public class Hello
	{
		public string Name { get; set; }
	}

	//Response DTO
	public class HelloResponse
	{
		public string Result { get; set; }

		public ResponseStatus ResponseStatus { get; set; } //Where Exceptions get auto-serialized
	}

	//Can be called via any endpoint or format, see: http://servicestack.net/ServiceStack.Hello/
	public class HelloService : Service
	{
		public object Any(Hello request)
		{
			return new HelloResponse { Result = "Hello, " + request.Name };
		}
	}
}
