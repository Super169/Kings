using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    public static class GmActivity
    {
        private const string CMD_superPackageInfo = "GmActivity.superPackageInfo";

        public static RequestReturnObject superPackageInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_superPackageInfo);
        }

    }
}
