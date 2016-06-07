using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    public static class OneYearEntry
    {
        private const string CMD_draw = "OneYearEntry.draw";
        private const string CMD_getOneYearEntryInfo = "OneYearEntry.getOneYearEntryInfo";

        public static RequestReturnObject draw(HTTPRequestHeaders oH, string sid, string type)
        {
            string body = string.Format("{{\"type\":{0}}}", type);
            return com.SendGenericRequest(oH, sid, CMD_draw, true, body);
        }

        public static RequestReturnObject getOneYearEntryInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getOneYearEntryInfo);
        }
    }
}
