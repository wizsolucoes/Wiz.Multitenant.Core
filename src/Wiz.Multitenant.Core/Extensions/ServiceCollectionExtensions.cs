using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using Wiz.Multitenant.Core.Common;
using Wiz.Multitenant.Core.Common.Builder;
using Wiz.Multitenant.Core.Common.Container;

namespace Wiz.Multitenant.Core.Extensions
{
    /// <summary>
    /// Nice method to create the tenant builder
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the services (application specific tenant class)
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static TenantBuilder<T> AddMultiTenancy<T>(this IServiceCollection services) where T : Tenant
            => new TenantBuilder<T>(services);

        /// <summary>
        /// Add the services (default tenant class)
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static TenantBuilder<Tenant> AddMultiTenancy(this IServiceCollection services)
            => new TenantBuilder<Tenant>(services);


        //Provavel deprecated
        public static IServiceProvider UseMultiTenantServiceProvider<T>(this IServiceCollection services, Action<T, ContainerBuilder, IHttpContextAccessor> registerServicesForTenant) where T : Tenant
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();

            //Declare our container and create a accessor function
            //This is to support the Func<MultiTenantContainer<T>> multiTenantContainerAccessor parameter in the middleware
            TenantContainer<T> container = null;
            Func<TenantContainer<T>> containerAccessor = () =>
            {
                return container;
            };
            services.AddSingleton(containerAccessor);

            //Add all the application level services to the builder
            containerBuilder.Populate(services);

            //Create and assign the new multiteant container
            container = new TenantContainer<T>(containerBuilder.Build(), registerServicesForTenant);

            //Return the new IServiceProvider which will be used to replace the standard one
            return new AutofacServiceProvider(containerAccessor());
        }
    }
}
