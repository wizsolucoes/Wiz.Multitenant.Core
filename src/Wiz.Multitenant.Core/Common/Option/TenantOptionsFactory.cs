using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using Wiz.Multitenant.Core.Common.Service;

namespace Wiz.Multitenant.Core.Common.Option
{
    /// <summary>
    /// Create a new options instance with configuration applied
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <typeparam name="T"></typeparam>
    internal class TenantOptionsFactory<TOptions, T> : IOptionsFactory<TOptions>
        where TOptions : class, new()
        where T : Tenant
    {

        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;
        private readonly IEnumerable<IPostConfigureOptions<TOptions>> _postConfigures;
        private readonly Action<TOptions, T> _tenantConfig;
        private readonly TenantAccessService<T> _tenantService;

        public TenantOptionsFactory(
            IEnumerable<IConfigureOptions<TOptions>> setups,
            IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, Action<TOptions, T> tenantConfig, TenantAccessService<T> tenantService)
        {
            _setups = setups;
            _postConfigures = postConfigures;
            _tenantService = tenantService;
            _tenantConfig = tenantConfig;
        }

        /// <summary>
        /// Create a new options instance
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TOptions Create(string name)
        {
            var options = new TOptions();

            //Apply options setup configuration
            foreach (var setup in _setups)
            {
                if (setup is IConfigureNamedOptions<TOptions> namedSetup)
                {
                    namedSetup.Configure(name, options);
                }
                else
                {
                    setup.Configure(options);
                }
            }

            //Apply tenant specifc configuration (to both named and non-named options)
            T tenant = _tenantService.GetTenantAsync().GetAwaiter().GetResult();

            if (tenant != null)
                _tenantConfig(options, tenant);

            //Apply post configuration
            foreach (var postConfig in _postConfigures)
            {
                postConfig.PostConfigure(name, options);
            }

            return options;
        }
    }
}
