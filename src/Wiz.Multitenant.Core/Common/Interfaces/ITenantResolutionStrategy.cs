using System.Threading.Tasks;

namespace Wiz.Multitenant.Core.Common.Interfaces
{
    public interface ITenantResolutionStrategy
    {
        Task<string> GetTenantIdentifierAsync();
    }
}
