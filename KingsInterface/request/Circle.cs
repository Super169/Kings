using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.request
{
    class Circle
    {
        private const string CMD_challenge = "Circle.challenge";
        private const string CMD_drawPassRewards = "Circle.drawPassRewards";
        private const string CMD_getHuarongRoadInfo = "Circle.getHuarongRoadInfo";
        private const string CMD_restartHuarongRoad = "Circle.restartHuarongRoad";
        private const string CMD_turnOverHeroCard = "Circle.turnOverHeroCard";

        public static RequestReturnObject challenge(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_challenge);
        }

        public static RequestReturnObject drawPassRewards(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_drawPassRewards);
        }

        public static RequestReturnObject getHuarongRoadInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getHuarongRoadInfo);
        }

        public static RequestReturnObject restartHuarongRoad(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_restartHuarongRoad);
        }


    }
}
