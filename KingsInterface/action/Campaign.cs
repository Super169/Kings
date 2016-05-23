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
        public static int goEliteBuyTime(GameAccount oGA, DelegateUpdateInfo updateInfo)
        {
            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;

            RequestReturnObject rro;
            rro = request.Campaign.getLeftTimes(oH, sid);
            if (!rro.SuccessWithJson("eliteBuyTimes")) return 0;
            int eliteBuyTimes = JSON.getInt(rro.responseJson, "eliteBuyTimes");
            if (eliteBuyTimes > 0) return 0;
            rro = request.Campaign.eliteBuyTime(oH, sid);
            if (!rro.SuccessWithJson("eliteBuyTimes")) return 0;
            eliteBuyTimes = JSON.getInt(rro.responseJson, "eliteBuyTimes");
            if ((eliteBuyTimes > 0) && (updateInfo != null)) updateInfo(string.Format("{0}討伐: 購買{1}次", oGA.msgPrefix, eliteBuyTimes));
            return eliteBuyTimes;
        }
    }
}
