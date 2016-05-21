﻿using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    class StarGazing
    {
        private const string CMD_activityInfo = "StarGazing.activityInfo";
        private const string CMD_myFirecrackerInfo = "StarGazing.myFirecrackerInfo";

        public static RequestReturnObject activityInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_activityInfo);
        }

        public static RequestReturnObject myFirecrackerInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_myFirecrackerInfo);
        }

    }
}
