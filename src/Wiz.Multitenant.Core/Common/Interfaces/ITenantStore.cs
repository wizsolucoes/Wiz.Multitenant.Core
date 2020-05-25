using System.Threading.Tasks;

namespace Wiz.Multitenant.Core.Common.Interfaces
{
    public interface ITenantStore<T> where T : Tenant
    {
        Task<T> GetTenantAsync(string identifier);
    }
}
