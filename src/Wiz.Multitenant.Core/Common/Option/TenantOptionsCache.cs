using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using Wiz.Multitenant.Core.Common.Interfaces;
using Wiz.Multitenant.Core.Common.Service;

namespace Wiz.Multitenant.Core.Common.Option
{
    /// <summary>
    /// Tenant aware options cache
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <typeparam name="TTenant"></typeparam>
    public class TenantOptionsCache<TOptions, TTenant> : IOptionsMonitorCache<TOptions>, ITenantConfigurationMonitor
        where TOptions : class
        where TTenant : Tenant
    {
        private readonly IConfiguration _config;
        private readonly string _id;
        private readonly TenantOptionsCacheDictionary<TOptions> _tenantSpecificOptionsCache =
            new TenantOptionsCacheDictionary<TOptions>();

        public TenantOptionsCache(IConfiguration config, TenantAccessService<TTenant> tenantService)
        {
            this._id = tenantService.GetTenantAsync().GetAwaiter().GetResult().Id;
            this._config = config;

            ChangeToken.OnChange<ITenantConfigurationMonitor>(
                () => _config.GetReloadToken(),
                InvokeChanged,
                this);
        }

        public bool NeedUpdate { get; set; } = false;

        public void Clear()
        {
            _tenantSpecificOptionsCache.Get(this._id).Clear();
        }

        public TOptions GetOrAdd(string name, Func<TOptions> createOptions)
        {

            if (this.NeedUpdate)
            {
                bool removed = this.TryRemove(name);
                if (removed)
                    this.NeedUpdate = false;

                return this._tenantSpecificOptionsCache.Get(this._id)
                    .GetOrAdd(name, createOptions);
            }
            else
            {
                return _tenantSpecificOptionsCache.Get(this._id)
                    .GetOrAdd(name, createOptions);
            }

        }

        public bool TryAdd(string name, TOptions options)
        {
            return _tenantSpecificOptionsCache.Get(this._id)
                .TryAdd(name, options);
        }

        public bool TryRemove(string name)
        {
            return _tenantSpecificOptionsCache.Get(this._id)
                .TryRemove(name);
        }

        private void InvokeChanged(ITenantConfigurationMonitor state)
        {
            state.NeedUpdate = true;
        }
    }
}
