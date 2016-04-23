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
        public static List<HeroInfo> Hero_getPlayerHeroList(HTTPRequestHeaders oH, string sid)
        {
            List<HeroInfo> heroList = new List<HeroInfo>();
            try
            {
                RequestReturnObject rro = com.SendGenericRequest(oH, sid, CMD_Hero_getPlayerHeroList);
                if (rro.success)
                {
                    dynamic json = com.getJsonFromResponse(rro.session, true);
                    DynamicJsonArray heros = json.heros;

                    foreach (dynamic hero in heros)
                    {
                        HeroInfo hi = new HeroInfo(hero);
                        if (hi.idx > 0) heroList.Add(hi);
                    }
                }
            }
            catch (Exception ex)
            {
                heroList = null;
            }

            return heroList;
        }
    }
}
