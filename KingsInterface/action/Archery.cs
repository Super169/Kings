using Fiddler;
using KingsInterface.data;
using System;

namespace KingsInterface
{
    public partial class action
    {
        public delegate void DelegateUpdateInfo(string info);

        public static ArcheryInfo getArcheryInfo(HTTPRequestHeaders oH, string sid, string type) {
            ArcheryInfo ai = new ArcheryInfo() { success = false, returnCode = AiReturnCode.NORMAL};
            RequestReturnObject rro = action.go_Archery_getArcheryInfo(oH, sid, type);
            if (!rro.success)
            {
                ai.msg = "讀取 百步穿楊 資訊失敗: " + rro.msg;
                return ai;
            }

            try
            {
                if (rro.responseJson == null)
                {
                    ai.msg = "讀取 百步穿楊 資訊失敗: 資料空白";
                    return ai;
                }
                if (rro.style == "ERROR")
                {
                    if (rro.prompt == PROMPT_ACTIVITY_NOT_OPEN)
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
                ai.msg = "讀取 百步穿楊 資訊失敗: " + ex.Message;
            }
            return ai;
        }

        public static bool goArcheryShoot(HTTPRequestHeaders oH, string sid, ref ArcheryInfo ai, string type)
        {
            if ((ai == null) || (!ai.success))
            {
                // ArcheryInfo is not yet retrieved, go retrieve first
                ai = getArcheryInfo(oH, sid, type);
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
            RequestReturnObject rro = action.go_Archery_shoot(oH, sid, ai.goX, ai.goY, type);
            if (!rro.success)
            {
                ai.msg = "執行射擊失敗: " + rro.msg;
                return false;
            }

            try
            {
                if (rro.responseJson == null)
                {
                    ai.msg = "讀取穿擊結果失敗: 資料空白";
                    return false;
                }
                if (rro.style == "ERROR")
                {
                    ai.returnCode = AiReturnCode.ERROR;
                    ai.msg = "ERROR: " + rro.responseJson["prompt"];
                }
                else
                {
                    ai.atX = rro.responseJson.x;
                    ai.atY = rro.responseJson.y;
                    ai.ring = rro.responseJson.ring;
                    ai.nWind = rro.responseJson.nWind;
                    ai.success = true;
                }
            }
            catch (Exception ex)
            {
                ai.msg = "讀取射擊結果失敗: " + ex.Message;
            }
            return ai.success;
        }


        // 現有{0}環; 餘下{1}次; 風力{2}; 目標({3},{4}); 結果({5},{6}); 得{7}環
        //
        public static bool goArcheryShootAll(HTTPRequestHeaders oH, string sid, DelegateUpdateInfo updateInfo, string type)
        {
            ArcheryInfo ai;
            bool returnCode = true;
            bool goNext = true;
            do
            {
                // Enforce to check archery info before shooting
                ai = null;
                goNext = goArcheryShoot(oH, sid, ref ai, type);
                if (goNext)
                {
                    string info = string.Format("現有{0}環; 餘下{1}次; 風力{2}; 目標({3},{4}); 結果({5},{6}); 得{7}環",
                                                ai.tRing, ai.arr, ai.wind, ai.goX, ai.goY, ai.atX, ai.atY, ai.ring);
                    if (updateInfo != null) updateInfo(info);
                }
                else
                {
                    if (ai.returnCode == AiReturnCode.NO_ACTIVITY)
                    {
                        if (updateInfo != null) updateInfo("今天沒有百步穿楊");
                    }
                    else if (ai.returnCode == AiReturnCode.COMPLETED)
                    {
                        if (updateInfo != null) updateInfo(string.Format("現有: {0} 環; 已經再沒有箭可以射了.", ai.tRing));
                    }
                    else
                    {
                        if (updateInfo != null) updateInfo(ai.msg);
                        returnCode = false;
                    }
                }

            } while (goNext);

            return returnCode;
        }

    }
}
