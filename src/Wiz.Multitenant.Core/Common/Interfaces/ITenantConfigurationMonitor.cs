namespace Wiz.Multitenant.Core.Common.Interfaces
{
    public interface ITenantConfigurationMonitor
    {
        public bool NeedUpdate { get; set; }
    }
}
