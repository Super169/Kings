using Fiddler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.data
{


    public class GameAccount
    {

        public enum AccountStatus { Online, Offline, Unknown }

        private static class GA_KEY
        {
            public const string sid = "sid";
            public const string account = "account";
            public const string status = "status";
            public const string timeAdjust = "timeAdjust";
            public const string server = "server";
            public const string serverTitle = "serverTitle";
            public const string nickName = "nickName";
            public const string corpsName = "corpsName";
            public const string level = "level";
            public const string vipLevel = "vipLevel";
            public const string currHeader = "currHeader";
            public const string lastUpdateDTM = "lastUpdateDTM";
            public const string Heros = "Heros";
            public const string DecreeHeros = "DecreeHeros";
            public const string BossWarHeros = "BossWarHeros";
            public const string BossWarChiefIdx = "BossWarChiefIdx";
            public const string BossWarBody = "BossWarBody";
            public const string BossWarCount = "BossWarCount";
            public const string AutoTasks = "AutoTasks";
        }

        public bool ready { get; set; } = false;
        public AccountStatus status { get; set; }
        public string sid { get; set; }
        public string account { get; set; }
        public string server { get; set; }
        public string serverTitle { get; set; }
        public string serverCode { get; set; }
        public int timeAdjust { get; set; }
        public string nickName { get; set; }
        public string corpsName { get; set; }
        public string level { get; set; }
        public string vipLevel { get; set; }

        public void refreshRecord()
        {
            // For pubgame accounts:
            string[] parts = serverTitle.Split(' ');
            if ((parts[0].Length < 2) || (parts[0].Length > 4)) serverCode = parts[0];
            try
            {
                int serverId = Convert.ToInt32(parts[0].Substring(1));
                serverId = (serverId > 9 ? serverId - 9 : serverId);
                this.serverCode = parts[0].Substring(0, 1) + serverId.ToString();
            }
            catch
            {
                this.serverCode = parts[0];
            }
        }

        public GameAccount(string account)
        {
            this.account = account;
            this.ready = false;
            refreshRecord();
        }

        public GameAccount(GFR.GenericFileRecord gfr)
        {
            this.ready = false;
            if (gfr == null) return;
            if ((this.account != null) && (gfr.key != this.account)) return;
            this.account = gfr.key;
            this.status = AccountStatus.Unknown;
            this.timeAdjust = JSON.getInt(gfr.getObject(GA_KEY.timeAdjust));
            this.server = JSON.getString(gfr.getObject(GA_KEY.server));
            this.serverTitle = JSON.getString(gfr.getObject(GA_KEY.serverTitle));
            this.nickName = JSON.getString(gfr.getObject(GA_KEY.nickName));
            this.corpsName = JSON.getString(gfr.getObject(GA_KEY.corpsName));
            this.level = JSON.getString(gfr.getObject(GA_KEY.level));
            this.vipLevel = JSON.getString(gfr.getObject(GA_KEY.vipLevel));
            this.ready = true;
            refreshRecord();
        }

        public GameAccount(LoginInfo li, HTTPRequestHeaders oH)
        {
            this.ready = false;
            if (li.ready)
            {
                this.sid = li.sid;
                this.account = li.account;
                this.status = AccountStatus.Online;
                // timeAjust will be set later using system.ping
                this.timeAdjust = 0;
                this.server = li.server;
                this.serverTitle = li.serverTitle;
                this.nickName = li.nickName;
                this.corpsName = li.CORPS_NAME;
                this.level = li.LEVEL;
                this.vipLevel = li.VIP_LEVEL;
                this.ready = true;
                refreshRecord();
            }
        }

        public GFR.GenericFileRecord ToGFR()
        {
            GFR.GenericFileRecord gfr = new GFR.GenericFileRecord(this.account);
            gfr.saveObject(GA_KEY.sid, this.sid);
            gfr.saveObject(GA_KEY.account, this.account);
            gfr.saveObject(GA_KEY.status, this.status);
            gfr.saveObject(GA_KEY.timeAdjust, this.timeAdjust);
            gfr.saveObject(GA_KEY.server, this.server);
            gfr.saveObject(GA_KEY.serverTitle, this.serverTitle);
            gfr.saveObject(GA_KEY.nickName, this.nickName);
            gfr.saveObject(GA_KEY.corpsName, this.corpsName);
            gfr.saveObject(GA_KEY.level, this.level);
            gfr.saveObject(GA_KEY.vipLevel, this.vipLevel);
            return gfr;
        }

        public static bool find(List<GameAccount> gameAccounts, GameAccount oGA, ref GameAccount oFind)
        {
            return find(gameAccounts, oGA.account, ref oFind);
        }

        public static bool find(List<GameAccount> gameAccounts, string account, ref GameAccount oFind)
        {
            if ((account == null) || (account.Trim() == "")) return false;
            oFind = gameAccounts.Find(x => x.account == account);
            if (oFind == null) return false;
            return true;
        }

    }

}
