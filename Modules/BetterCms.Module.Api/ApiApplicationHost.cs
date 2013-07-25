using System;

using Autofac;

using BetterCms.Module.Api.Filters;

using ServiceStack.ServiceInterface.Validation;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;

namespace BetterCms.Module.Api
{
	public class ApiApplicationHost
		: AppHostBase
	{
        private readonly Func<ILifetimeScope> containerProvider;

        public ApiApplicationHost(Func<ILifetimeScope> containerProvider)
            : base("Better CMS Web API Host", typeof(ApiModuleDescriptor).Assembly)
        {
            this.containerProvider = containerProvider;
        }

		public override void Configure(Funq.Container container)
		{            
            RequestBinders.Clear();
			
            JsConfig.EmitCamelCaseNames = true;
		
            Plugins.Add(new ValidationFeature());
            
            // Add custom request filter
            RequestFilters.Add(GetRequestProcessor.DeserializeJsonFromGet);

            container.Adapter = new AutofacContainerAdapter(containerProvider);
            container.RegisterValidators(typeof(IApiFacade).Assembly);
		}
	}
}