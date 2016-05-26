using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Travel
    {
        private const string CMD_arriveStep = "Travel.arriveStep";
        private const string CMD_attack = "Travel.attack";
        private const string CMD_checkOut = "Travel.checkOut";
        private const string CMD_controlDice = "Travel.controlDice";
        private const string CMD_dice = "Travel.dice";
        private const string CMD_escape = "Travel.escape";
        private const string CMD_getMapInfo = "Travel.getMapInfo";
        private const string CMD_getStatus = "Travel.getStatus";
        private const string CMD_restartTravel = "Travel.restartTravel";
        private const string CMD_viewStep = "Travel.viewStep";


        public static RequestReturnObject arriveStep(HTTPRequestHeaders oH, string sid, int step)
        {
            string body = "{\"step\":" + step.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_arriveStep, true, body);
        }


        public static RequestReturnObject attack(HTTPRequestHeaders oH, string sid, int step)
        {
            string body = "{\"step\":" + step.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_attack, true, body);
        }

        public static RequestReturnObject checkOut(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_checkOut);
        }

        public static RequestReturnObject controlDice(HTTPRequestHeaders oH, string sid, int num)
        {
            string body = "{\"num\":" + num.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_controlDice, true, body);
        }


        public static RequestReturnObject dice(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_dice);
        }

        public static RequestReturnObject escape(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_escape);
        }

        public static RequestReturnObject getMapInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getMapInfo);
        }

        public static RequestReturnObject getStatus(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getStatus);
        }

        public static RequestReturnObject restartTravel(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_restartTravel);
        }


        public static RequestReturnObject viewStep(HTTPRequestHeaders oH, string sid, int step)
        {
            string body = "{\"step\":" + step.ToString() + "}";
            return com.SendGenericRequest(oH, sid, CMD_viewStep, true, body);
        }


    }
}
