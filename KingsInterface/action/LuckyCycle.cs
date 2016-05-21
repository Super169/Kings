using Fiddler;
using KingsInterface.data;
using MyUtil;
using System;
using System.Web.Helpers;

namespace KingsInterface
{
    public partial class action
    {
        public static int goLuckyCycle(GameAccount oGA)
        {
            int drawCount = 0;
            RequestReturnObject rro;
            rro = request.LuckyCycle.info(oGA.currHeader, oGA.sid);
            if (!rro.SuccessWithJson("remainCount")) return 0;
            int remainCount = JSON.getInt(rro.responseJson, "remainCount");
            while (remainCount > 0)
            {
                rro = request.LuckyCycle.draw(oGA.currHeader, oGA.sid);
                if (!rro.SuccessWithJson("id")) break;
                drawCount++;
                remainCount--;
                rro = request.Player.getProperties(oGA.currHeader, oGA.sid);
            }
            return drawCount;
        }
    }
}
