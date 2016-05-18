using Fiddler;
using KingsInterface.data;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsInterface
{

    public partial class action
    {

        public static PlayerProperties goGetPlayerProperties(HTTPRequestHeaders oH, string sid)
        {
            PlayerProperties pp = new PlayerProperties() { ready = false };
            RequestReturnObject rro = go_Player_getProperties(oH, sid);
            if (rro.SuccessWithJson("pvs"))
            {
                DynamicJsonArray pvs = rro.responseJson.pvs;
                foreach (dynamic pv in pvs)
                {
                    string p = pv.p;
                    switch (p)
                    {
                        case "EXP":
                            pp.EXP = JSON.getInt(pv, "v");
                            break;
                        case "UNDERGO_EXP":
                            pp.UNDERGO_EXP = JSON.getInt(pv, "v");
                            break;
                        case "LEVEL_UP_EXP":
                            pp.LEVEL_UP_EXP = JSON.getInt(pv, "v");
                            break;
                        case "LEVEL":
                            pp.LEVEL = JSON.getInt(pv, "v");
                            break;
                        case "VIP_LEVEL":
                            pp.VIP_LEVEL = JSON.getInt(pv, "v");
                            break;
                        case "GOLD":
                            pp.GOLD = JSON.getInt(pv, "v");
                            break;
                        case "SILVER":
                            pp.SILVER = JSON.getInt(pv, "v");
                            break;
                        case "FOOD":
                            pp.FOOD = JSON.getInt(pv, "v");
                            break;
                        case "EXPLOIT":
                            pp.EXPLOIT = JSON.getInt(pv, "v");
                            break;
                        case "ARENA_COIN":
                            pp.ARENA_COIN = JSON.getInt(pv, "v");
                            break;
                        case "XIYU_COIN":
                            pp.XIYU_COIN = JSON.getInt(pv, "v");
                            break;
                        case "MAX_FOOD":
                            pp.MAX_FOOD = JSON.getInt(pv, "v");
                            break;
                        case "MAX_SILVER":
                            pp.MAX_SILVER = JSON.getInt(pv, "v");
                            break;
                        case "MAX_IRON":
                            pp.MAX_IRON = JSON.getInt(pv, "v");
                            break;
                        case "CORPS_NAME":
                            pp.CORPS_NAME = pv["v"];
                            break;
                        case "IRON":
                            pp.IRON = JSON.getInt(pv, "v");
                            break;
                        case "ICON":
                            pp.ICON = JSON.getInt(pv, "v");
                            break;
                        case "PLATFORM_MARK":
                            pp.PLATFORM_MARK = JSON.getInt(pv, "v");
                            break;
                        case "LONGMARCH_COIN":
                            pp.LONGMARCH_COIN = JSON.getInt(pv, "v");
                            break;
                        case "CSKING_COIN":
                            pp.CSKING_COIN = JSON.getInt(pv, "v");
                            break;
                        case "FIGHTING_SPIRIT":
                            pp.FIGHTING_SPIRIT = JSON.getInt(pv, "v");
                            break;
                        case "CONTRIBUTION":
                            pp.CONTRIBUTION = JSON.getInt(pv, "v");
                            break;
                        case "GOLD_TICKET":
                            pp.GOLD_TICKET = JSON.getInt(pv, "v");
                            break;

                    }
                }
                pp.ready = true;
            }
            return pp;
        }


    }
}
