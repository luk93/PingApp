using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PingApp.Tools
{
    public static class Converters
    {
        public static IPAddress? ConvertStrToIpAddress(string? ipString)
        {
            if (ipString == null) return null;
            try
            {
                return IPAddress.Parse(ipString);
            }
            catch
            {
                return null;
            }
        }
    }
}
