using Fiddler;
using KingsInterface.data;
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

        public static bool sendNavalTroops(GameAccount oGA, int cityId, string heros)
        {
            RequestReturnObject rro;

            // For safety, can always leave war before any action, there has no checking required, error can be ignored.
            rro = go_Naval_leaveWar(oGA.currHeader, oGA.sid);

            try
            {

                rro = go_Naval_inMissionHeros(oGA.currHeader, oGA.sid);
                if (!rro.SuccessWithJson("alives", typeof(DynamicJsonArray))) return false;
                DynamicJsonArray oAlives, oDeads, oDeadHero;
                oAlives = rro.responseJson["alives"];
                oDeads = rro.responseJson["deads"];
                oDeadHero = rro.responseJson["deadhero"];
                // The task should be executed before start, so may only need to check alives
                // If any of them contain data, that means troops already sent, no action required.
                if ((oAlives.Length + oDeads.Length + oDeadHero.Length) > 0) return true;

                rro = go_Naval_enterWar(oGA.currHeader, oGA.sid, cityId);
                if (!rro.SuccessWithJson("disp")) return false;
                rro = go_Naval_sendTroops(oGA.currHeader, oGA.sid, cityId, heros);
                if (!rro.SuccessWithJson("troops")) return false;

            } catch (Exception)
            {
                return false;
            }

            rro = go_Naval_leaveWar(oGA.currHeader, oGA.sid);

            return true;
        }
    }
}
