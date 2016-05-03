using Fiddler;
using KingsInterface.data;
using System;

namespace KingsInterface
{
    public partial class action
    {
        public static ArcheryInfo getArcheryInfo(HTTPRequestHeaders oH, string sid) {
            ArcheryInfo ai = new ArcheryInfo() { success = false, returnCode = AiReturnCode.NORMAL};
            RequestReturnObject rro = action.go_Archery_getArcheryInfo(oH, sid);
            if (!rro.success)
            {
                ai.msg = "讀取 百步穿楊 資訊失敗\n" + rro.msg;
                return ai;
            }

            try
            {
                if (rro.responseJson == null)
                {
                    ai.msg = "讀取 百步穿楊 資訊失敗\n資料空白";
                    return ai;
                }
                if (rro.responseJson["style"] == "ERROR")
                {
                    if (rro.responseJson["prompt"] == "ACTIVITY_IS_NOT_OPEN")
                    {
                        ai.returnCode = AiReturnCode.NO_ACTIVITY;
                    } else
                    {
                        ai.returnCode = AiReturnCode.ERROR;
                        ai.msg = "ERROR: " + rro.responseJson["prompt"];
                    }
                } else
                {
                    ai.tRing = rro.responseJson.tRing;
                    ai.arr = rro.responseJson.arr;
                    ai.wind = rro.responseJson.wind;
                    ai.success = true;
                }
            }
            catch (Exception ex)
            {
                ai.msg = "讀取 百步穿楊 資訊失敗\n" + ex.Message;
                ai.msg += "\n\n" + rro.responseText;
            }
            return ai;
        }

        public static bool goArcheryShoot(HTTPRequestHeaders oH, string sid, ref ArcheryInfo ai)
        {
            if ((ai == null) || (!ai.success))
            {
                // ArcheryInfo is not yet retrieved, go retrieve first
                ai = getArcheryInfo(oH, sid);
            }
            if (!ai.success) return false;

            if (ai.arr == 0)
            {
                ai.returnCode = AiReturnCode.COMPLETED;
                return false;
            }

            ai.success = false;
            // TODO: Adjust for abs(ai.wind) > 500
            ai.goX = (Math.Abs(ai.wind) < 100 ? 0 : (ai.wind < 0 ? (ai.wind + 100) / -10 : (100 - ai.wind) / 10));
            ai.goY = 11;
            ai.msg += string.Format("瞄準位置: ( {0} , {1} )\n", ai.goX, ai.goY);
            RequestReturnObject rro = action.go_Archery_shoot(oH, sid, ai.goX, ai.goY);
            if (!rro.success)
            {
                ai.msg += "\n執行射擊失敗:\n" + rro.msg;
                return false;
            }

            try
            {
                if (rro.responseJson == null)
                {
                    ai.msg = "讀取穿擊結果失敗\n資料空白";
                    return false;
                }
                ai.atX = rro.responseJson.x;
                ai.atY = rro.responseJson.y;
                ai.ring = rro.responseJson.ring;
                ai.nWind = rro.responseJson.nWind;
                ai.msg += string.Format("擊中: ( {0} , {1} ), 取得 {2} 環, 下次風力為 {3}", ai.atX, ai.atY, ai.ring, ai.nWind);
                ai.success = true;
            }
            catch (Exception ex)
            {
                ai.msg = "讀取射擊結果失敗\n" + ex.Message;
                ai.msg += "\n\n" + rro.responseText;
            }
            return ai.success;
        }

    }
}
