using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    public static class Major
    {
        private const string CMD_getMyMajorInfo = "Major.getMyMajorInfo";

        public static RequestReturnObject getMyMajorInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getMyMajorInfo);
        }

    }
}
