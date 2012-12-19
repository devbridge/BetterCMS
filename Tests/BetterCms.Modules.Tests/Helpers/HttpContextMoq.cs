using System;
using System.Collections.Specialized;
using System.IO;
using System.Security.Principal;
using System.Web;
using Moq;

namespace BetterCms.Tests.Helpers
{
    public class HttpContextMoq
    {
       public Mock<HttpContextBase> MockContext { get; set; }

        public Mock<HttpRequestBase> MockRequest { get; set; }

        public Mock<HttpResponseBase> MockResponse { get; set; }

        public Mock<HttpSessionStateBase> MockSession { get; set; }

        public Mock<HttpServerUtilityBase> MockServer { get; set; }

        public Mock<IPrincipal> MockUser { get; set; }

        public Mock<IIdentity> MockIdentity { get; set; }
        
        public HttpContextBase HttpContextBase { get; set; }

        public HttpRequestBase HttpRequestBase { get; set; }

        public HttpResponseBase HttpResponseBase { get; set; }

        public HttpContextMoq()
        {
            CreateBaseMocks();
            SetupNormalRequestValues();
        }

        public HttpContextMoq CreateBaseMocks()
        {
            MockContext = new Mock<HttpContextBase>();
            MockRequest = new Mock<HttpRequestBase>();
            MockResponse = new Mock<HttpResponseBase>();
            MockSession = new Mock<HttpSessionStateBase>();
            MockServer = new Mock<HttpServerUtilityBase>();
            
            MockContext.Setup(ctx => ctx.Request).Returns(MockRequest.Object);
            MockContext.Setup(ctx => ctx.Response).Returns(MockResponse.Object);
            MockContext.Setup(ctx => ctx.Session).Returns(MockSession.Object);
            MockContext.Setup(ctx => ctx.Server).Returns(MockServer.Object);
            

            HttpContextBase = MockContext.Object;
            HttpRequestBase = MockRequest.Object;
            HttpResponseBase = MockResponse.Object;

            return this;
        }


        public HttpContextMoq SetupNormalRequestValues()
        {
            var MockUser = new Mock<IPrincipal>();
            var MockIdentity = new Mock<IIdentity>();
            
            MockContext.Setup(context => context.User).Returns(MockUser.Object);
            MockUser.Setup(context => context.Identity).Returns(MockIdentity.Object);
            
            MockRequest.Setup(request => request.InputStream).Returns(new MemoryStream());
            MockRequest.Setup(request => request.Url).Returns(new Uri("http://localhost/"));
            MockRequest.Setup(request => request.PathInfo).Returns(string.Empty);
            MockRequest.Setup(request => request.ServerVariables).Returns(new NameValueCollection());
            MockResponse.Setup(response => response.OutputStream).Returns(new MemoryStream());

            return this;
        }
    }
}