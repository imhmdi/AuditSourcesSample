using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Reflection;

namespace AuditSourcesSample
{
    public class CustomeAuditSourcesProvider : AuditSourcesProvider
    {
 
        public CustomeAuditSourcesProvider(IHttpContextAccessor httpContextAccessor):base(httpContextAccessor)
        {

        }

        protected override string GetUserAgent(HttpContext httpContext)
        {
            return null;
        }

        protected override string GetApplicationName(HttpContext httpContext)
        {
            return "MyAplication";
        }

    }
}