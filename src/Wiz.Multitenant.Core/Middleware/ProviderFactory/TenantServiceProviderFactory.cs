using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using Wiz.Multitenant.Core.Common;
using Wiz.Multitenant.Core.Common.Container;

namespace Wiz.Multitenant.Core.Middleware.ProviderFactory
{
    public class TenantServiceProviderFactory<T> : IServiceProviderFactory<ContainerBuilder> where T : Tenant
    {

        public Action<T, ContainerBuilder, IHttpContextAccessor> _tenantSerivcesConfiguration;

        public TenantServiceProviderFactory(Action<T, ContainerBuilder, IHttpContextAccessor> tenantSerivcesConfiguration)
        {
            _tenantSerivcesConfiguration = tenantSerivcesConfiguration;
        }

        /// <summary>
        /// Create a builder populated with global services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            var builder = new ContainerBuilder();

            builder.Populate(services);

            return builder;
        }

        /// <summary>
        /// Create our serivce provider
        /// </summary>
        /// <param name="containerBuilder"></param>
        /// <returns></returns>
        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            TenantContainer<T> container = null;

            Func<TenantContainer<T>> containerAccessor = () =>
            {
                return container;
            };

            containerBuilder
                .RegisterInstance(containerAccessor)
                .SingleInstance();

            container = new TenantContainer<T>(containerBuilder.Build(), _tenantSerivcesConfiguration);

            return new AutofacServiceProvider(containerAccessor());
        }
    }
}
