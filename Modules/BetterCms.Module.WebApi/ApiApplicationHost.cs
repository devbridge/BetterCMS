using System;

using Autofac;

using BetterCms.Core.Dependencies;

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
			//Set JSON web services to return idiomatic JSON camelCase properties
			ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;
		
			//Uncomment to change the default ServiceStack configuration
			SetConfig(new EndpointHostConfig {
			});

            container.Adapter = new AutofacContainerAdapter(containerProvider);
		}

		/* Uncomment to enable ServiceStack Authentication and CustomUserSession
		private void ConfigureAuth(Funq.Container container)
		{
			var appSettings = new AppSettings();

			//Default route: /auth/{provider}
			Plugins.Add(new AuthFeature(() => new CustomUserSession(),
				new IAuthProvider[] {
					new CredentialsAuthProvider(appSettings), 
					new FacebookAuthProvider(appSettings), 
					new TwitterAuthProvider(appSettings), 
					new BasicAuthProvider(appSettings), 
				})); 

			//Default route: /register
			Plugins.Add(new RegistrationFeature()); 

			//Requires ConnectionString configured in Web.Config
			var connectionString = ConfigurationManager.ConnectionStrings["AppDb"].ConnectionString;
			container.Register<IDbConnectionFactory>(c =>
				new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider));

			container.Register<IUserAuthRepository>(c =>
				new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));

			var authRepo = (OrmLiteAuthRepository)container.Resolve<IUserAuthRepository>();
			authRepo.CreateMissingTables();
		}
		*/
	}
}