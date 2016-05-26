using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class City
    {
        private const string CMD_buyProduct = "City.buyProduct";
        private const string CMD_getIndustryInfo = "City.getIndustryInfo";
        private const string CMD_getPlayerCityInfo = "City.getPlayerCityInfo";

        public static RequestReturnObject getPlayerCityInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getPlayerCityInfo);
        }


    }
}
