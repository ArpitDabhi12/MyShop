using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.WebUI.Tests.Mocks
{
    public class MockHttpContext : HttpContextBase
    {
        private MockRequest MockRequest;
        private MockResponse MockResponse;
        private HttpCookieCollection HttpCookieCollection;
        private IPrincipal MockUser;

        public override IPrincipal User
        {
            get { return this.MockUser; }
            set { this.MockUser = value; }
        }



        public MockHttpContext()
        {
            this.HttpCookieCollection = new HttpCookieCollection();
            this.MockRequest = new MockRequest(HttpCookieCollection);
            this.MockResponse = new MockResponse(HttpCookieCollection);
        }

        public override HttpRequestBase Request
        {
            get { return MockRequest; }
        }

        public override HttpResponseBase Response
        {
            get { return MockResponse; }
        }
    }

    public class MockResponse : HttpResponseBase
    {
        private readonly HttpCookieCollection httpCookieCollection;

        public MockResponse(HttpCookieCollection httpCookieCollection)
        {
            this.httpCookieCollection = httpCookieCollection;
        }

        public override HttpCookieCollection Cookies
        {
            get { return httpCookieCollection; }
        }
    }

    public class MockRequest : HttpRequestBase
    {
        private readonly HttpCookieCollection httpCookieCollection;

        public MockRequest(HttpCookieCollection httpCookieCollection)
        {
            this.httpCookieCollection = httpCookieCollection;
        }

        public override HttpCookieCollection Cookies
        {
            get { return httpCookieCollection; }
        }
    }
}
