using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Wiz.Multitenant.Core.Common.Interfaces;

namespace Wiz.Multitenant.Core.Strategies
{
    /// <summary>
    /// Resolve the host to a tenant identifier
    /// </summary>
    public class HeaderTenantResolutionStrategy : ITenantResolutionStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HeaderTenantResolutionStrategy(IHttpContextAccessor httpContextAccessor)
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
                return await Task.FromResult(_httpContextAccessor.HttpContext.Request.Headers["x-tenant"]);
            }
        }
    }
}
