using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Wiz.Multitenant.Core.Common;
using Wiz.Multitenant.Core.Common.Container;

namespace Wiz.Multitenant.Core.Middleware
{
    class TenantMiddleware<T> where T : Tenant
    {
        private readonly RequestDelegate next;

        public TenantMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, Func<TenantContainer<T>> multiTenantContainerAccessor)
        {
            //Set to current tenant container.
            //Begin new scope for request as ASP.NET Core standard scope is per-request
            context.RequestServices =
                new AutofacServiceProvider(multiTenantContainerAccessor()
                        .GetCurrentTenantScope().BeginLifetimeScope());
            await next.Invoke(context);
        }
    }
}
