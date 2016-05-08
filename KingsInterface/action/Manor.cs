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
        public static List<ManorInfo> goGetManorInfo(HTTPRequestHeaders oH, string sid)
        {
            List<ManorInfo> manorInfo = new List<ManorInfo>();
            RequestReturnObject rro = go_Manor_getManorInfo(oH, sid);
            try
            {
                if ((rro.success) && (rro.responseJson["buildings"] != null))
                {
                    DynamicJsonArray buildings = rro.responseJson["buildings"];
                    foreach (dynamic building in buildings)
                    {
                        ManorInfo mi = new ManorInfo();
                        mi.field = building["field"];
                        mi.type = building["type"];
                        mi.level = building["level"];
                        mi.levelSeconds = building["levelSeconds"];
                        mi.heroIndex = building["heroIndex"];
                        mi.leftSeconds = building["leftSeconds"];
                        mi.products = building["products"];
                        mi.produceSeconds = building["produceSeconds"];
                        manorInfo.Add(mi);
                    }
                }      
            } catch (Exception)
            {

            }
            return manorInfo;
        }
    }

}
