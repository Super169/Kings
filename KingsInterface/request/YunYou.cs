﻿using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    class YunYou
    {
        private const string CMD_getYunYouInfo = "YunYou.getYunYouInfo";

        public static RequestReturnObject getYunYouInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getYunYouInfo);
        }

    }
}
