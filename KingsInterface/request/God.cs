﻿using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    class God
    {
        private const string CMD_godActToStren = "God.godActToStren";
        private const string CMD_godStrenInfo = "God.godStrenInfo";
        private const string CMD_godStrenOrAdvance = "God.godStrenOrAdvance";

        public static RequestReturnObject godStrenInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_godStrenInfo);
        }


    }
}
