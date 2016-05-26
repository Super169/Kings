using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Country
    {
        private const string CMD_corpsCountry = "Country.corpsCountry";
        private const string CMD_viewCountry = "Country.viewCountry";


        public static RequestReturnObject corpsCountry(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_corpsCountry);
        }

        public static RequestReturnObject viewCountry(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_viewCountry);
        }

    }
}
