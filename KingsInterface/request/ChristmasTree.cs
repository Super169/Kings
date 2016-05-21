﻿using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    class ChristmasTree
    {
        private const string CMD_time = "ChristmasTree.time";

        public static RequestReturnObject time(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_time);
        }

    }
}
