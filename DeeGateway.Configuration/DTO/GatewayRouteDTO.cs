using System;
using System.Collections.Generic;
using System.Text;

namespace DeeGateway.Configuration.DTO
{
    public class GatewayRouteDTO
    {
        public string Url
        {
            get;
            set;
        }

        public string HashPattern
        {
            get;
            set;
        }

        public IEnumerable<GatewayRouteServerDTO> Servers
        {
            get;
            set;
        }
    }
}
