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
        public static bool goSignIn(GameAccount oGA, DelegateUpdateInfo updateInfo = null)
        {
            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;
            try
            {
                RequestReturnObject rro;
                RequestReturnObject rroAction;
                int curNum = 0;
                rro = go_SignInReward_getInfo(oH, sid);
                if (rro.success)
                {
                    if (rro.responseJson == null)
                    {
                        if (updateInfo != null) updateInfo(oGA.msgPrefix + "帳戶尚未達到簽到要求");
                        return false;
                    }
                    if (rro.responseJson["curNum"] != null) curNum = (int)rro.responseJson["curNum"];
                    if ((bool) rro.responseJson["canSign"])
                    {
                        rroAction = go_SignInReward_signIn(oH, sid);
                        if (rroAction.ok == 1)
                        {
                            curNum++;
                            if (updateInfo != null) updateInfo(oGA.msgPrefix + string.Format("成功進行第{0}次簽到", curNum));
                        }
                        else
                        {
                            if (updateInfo != null) updateInfo(oGA.msgPrefix + "簽到失敗");
                        }
                    } else
                    {
                        // if (updateInfo != null) updateInfo(oGA.msgPrefix + "今天已經簽到了");
                    }
                } else
                {
                    if (updateInfo != null) updateInfo(oGA.msgPrefix + "讀取 簽到 資料失敗");
                    return false;
                }

                if (rro.responseJson["multipleRewards"] == null)
                {
                    if (updateInfo != null) updateInfo(oGA.msgPrefix + "讀取 多次簽到 資料失敗");
                    return false;
                }

                DynamicJsonArray multipleRewards = rro.responseJson["multipleRewards"];
                foreach (dynamic multipleReward in multipleRewards)
                {
                    if ((multipleReward["signNum"] <= curNum) && (multipleReward["status"] != "FIN"))
                    {
                        // Go sign for multiple
                        rroAction = go_SignInReward_signInMultiple(oH, sid, multipleReward["signNum"]);
                        if (rroAction.ok == 1)
                        {
                            if (updateInfo != null) updateInfo(oGA.msgPrefix + string.Format("成功領取{0}次簽到額外獎勵", multipleReward["signNum"]));
                        } else
                        {
                            if (updateInfo != null) updateInfo(oGA.msgPrefix + string.Format("領取{0}次簽到額外獎勵失敗", multipleReward["signNum"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (updateInfo != null) updateInfo(oGA.msgPrefix + "簽到失敗: " + ex.Message);
                return false;
            }

            return true;
        }
    }
}

