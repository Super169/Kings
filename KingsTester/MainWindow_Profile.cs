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
        List<GameAccountProfile> gameAccountProfiles = new List<GameAccountProfile>();

        [Serializable]
        public class GameAccountProfile
        {
            public string Account { get; set; }
            public int[] BossWarHeros = new int[7];
            public int BossWarChiefIdx = -1;
            public string BossWarBody = "";

            public void fromGameAccount(GameAccount oGA)
            {
                this.Account = oGA.account;
                for (int i = 0; i < 7; i++) this.BossWarHeros[i] = oGA.BossWarHeros[i];
                this.BossWarChiefIdx = oGA.BossWarChiefIdx;
                this.BossWarBody = oGA.BossWarBody;
            }

            public void toGameAccount(GameAccount oGA)
            {
                if (oGA.account == this.Account)
                {
                    for (int i = 0; i < 7; i++) oGA.BossWarHeros[i] = this.BossWarHeros[i];
                    oGA.BossWarChiefIdx = this.BossWarChiefIdx;
                    oGA.BossWarBody = this.BossWarBody;
                }
            }

        }

        private void SaveProfile()
        {
            if (gameAccounts.Count == 0) return;

            foreach (GameAccount oGA in gameAccounts)
            {
                GameAccountProfile oGAP = gameAccountProfiles.SingleOrDefault(x => x.Account == oGA.account);
                if (oGAP == null)
                {
                    oGAP = new GameAccountProfile();
                    oGAP.fromGameAccount(oGA);
                    gameAccountProfiles.Add(oGAP);
                }
                else
                {
                    oGAP.fromGameAccount(oGA);
                }
            }

            FileStream fs = null;
            try
            {
                fs = new FileStream("myKing.dat", FileMode.OpenOrCreate);
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, gameAccountProfiles);
                UpdateResult("資料儲存成功", true);
            }
            catch (Exception ex)
            {
                UpdateResult("Error saving data:\n" + ex.Message, true);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        private void RestoreProfile()
        {
            gameAccountProfiles.Clear();

            FileStream fs = null;
            try
            {
                fs = new FileStream("myKing.dat", FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                gameAccountProfiles = (List<GameAccountProfile>)formatter.Deserialize(fs);
            }
            catch (Exception ex)
            {
                UpdateResult("Error reading profile file:\n" + ex.Message, true);
                return;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        private bool RestoreProfile(GameAccount oGA)
        {
            GameAccountProfile oGAP = gameAccountProfiles.SingleOrDefault(x => x.Account == oGA.account);
            if (oGAP == null)
            {
                return false;
            }
            oGAP.toGameAccount(oGA);
            UpdateInfo(string.Format("{0} - {1} : 取得神將設定", oGA.serverTitle, oGA.nickName));
            return true;
        }

    }
}
