using System.Collections.Generic;

namespace Wiz.Multitenant.Core.Common
{
    /// <summary>
    /// Tenant information
    /// </summary>
    public class Tenant
    {
        /// <summary>
        /// The tenant Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The tenant DNS
        /// </summary>
        public string Dns { get; set; }

        /// <summary>
        /// Tenant items
        /// </summary>
        public Dictionary<string, object> Items { get; private set; } = new Dictionary<string, object>();
    }
}
