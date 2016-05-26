﻿using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class MonthSignIn
    {
        private const string CMD_getOpenInfo = "MonthSignIn.getOpenInfo";

        public static RequestReturnObject getOpenInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getOpenInfo);
        }


    }
}
