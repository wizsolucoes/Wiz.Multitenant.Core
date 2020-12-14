using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Wiz.Multitenant.Core.Common.Interfaces;

namespace Wiz.Multitenant.Core.Strategies
{
    /// <summary>
    /// Resolve the host to a tenant identifier
    /// </summary>
    public class HeaderOrHostTenantResolutionStrategy : ITenantResolutionStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HeaderOrHostTenantResolutionStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get the tenant identifier
        /// </summary>
        /// <returns>host</returns>
        public async Task<string> GetTenantIdentifierAsync()
        {

            if (_httpContextAccessor.HttpContext is null)
            {
                return null;
            }
            else
            {
                string id = _httpContextAccessor.HttpContext.Request.Headers["x-tenant"];
                if(string.IsNullOrWhiteSpace(id)){
                    id = _httpContextAccessor.HttpContext.Request.Host.Host;
                }

                return await Task.FromResult(id);
            }
        }
    }
}
