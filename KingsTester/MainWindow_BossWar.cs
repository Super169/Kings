using KingsInterface;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KingsTester
{
    public partial class MainWindow : Window
    {
        private void GoBossWarOnce()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                if ((oGA.BossWarBody == null) || (oGA.BossWarBody == ""))
                {
                    UpdateResult(string.Format("{0} : 神將無雙尚未設定", oGA.accInfo()));
                } else
                {
                    BossWarInfo bwi = action.goBossWarOnce(oGA.currHeader, oGA.sid, oGA.BossWarBody, UpdateInfoHandler);
                    string info = string.Format("{0} : ", oGA.accInfo());
                    if (bwi.enterFail)
                    {
                        info += "進入戰場失敗";
                    } else if (!bwi.bossAvailable)
                    {
                        info += "沒有神將無雙";
                    } else
                    {
                        info += string.Format("神將 HP: {0}, 第 {1} 次出兵: ", bwi.bossHP, bwi.beforeCnt + 1);
                        if (bwi.sendFail)
                        {
                            info += "失敗";

                        } else
                        {
                            info += "成功";
                            if (bwi.leavelFail)
                            {
                                info += ", 但離開時出錯";
                            }
                        }
                    }

                    UpdateResult(info);
                }
            }
        }
    }
}
