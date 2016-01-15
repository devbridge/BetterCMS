// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HttpContextMoq.cs" company="Devbridge Group LLC">
// 
// Copyright (C) 2015,2016 Devbridge Group LLC
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// 
// <summary>
// Better CMS is a publishing focused and developer friendly .NET open source CMS.
// 
// Website: https://www.bettercms.com 
// GitHub: https://github.com/devbridge/bettercms
// Email: info@bettercms.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------
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