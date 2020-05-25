using Autofac;
using Microsoft.Extensions.Options;
using System;
using Wiz.Multitenant.Core.Common;
using Wiz.Multitenant.Core.Common.Option;

namespace Wiz.Multitenant.Core.Extensions
{
    public static class ContainerBuilderExtension
    {
        /// <summary>
        /// Register tenant specific options
        /// </summary>
        /// <typeparam name="TOptions">Type of options we are apply configuration to</typeparam>
        /// <param name="tenantOptionsConfiguration">Action to configure options for a tenant</param>
        /// <returns></returns>
        public static ContainerBuilder RegisterTenantOptions<TOptions, T>(this ContainerBuilder builder, Action<TOptions, T> tenantConfig) where TOptions : class, new() where T : Tenant
        {
            builder.RegisterType<TenantOptionsCache<TOptions, T>>()
                .As<IOptionsMonitorCache<TOptions>>()
                .SingleInstance();

            builder.RegisterType<TenantOptionsFactory<TOptions, T>>()
                .As<IOptionsFactory<TOptions>>()
                .WithParameter(new TypedParameter(typeof(Action<TOptions, T>), tenantConfig))
                .SingleInstance();


            builder.RegisterType<TenantOptions<TOptions>>()
                .As<IOptionsSnapshot<TOptions>>()
                .SingleInstance();

            builder.RegisterType<TenantOptions<TOptions>>()
                .As<IOptions<TOptions>>()
                .SingleInstance();

            return builder;
        }
    }
}
