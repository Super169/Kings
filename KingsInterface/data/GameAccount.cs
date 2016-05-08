using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.data
{
    public class GameAccount
    {
        public string sid { get; set; }
        public string account { get; set; }
        public string server { get; set; }
        public string serverTitle { get; set; }
        public string nickName { get; set; }
        public string corpsName { get; set; }
        public string level { get; set; }
        public string vipLevel { get; set; }
        public HTTPRequestHeaders currHeader { get; set; }
        public DateTime lastUpdateDTM { get; set; }
        public List<HeroInfo> Heros { get; set; }
        public List<DecreeInfo> DecreeHeros { get; set; }
        public int[] BossWarHeros = new int[7];
        public int BossWarChiefIdx = -1;
        public string BossWarBody { get; set; }
        public int BossWarCount { get; set; }
        
        public GameAccount(LoginInfo li, HTTPRequestHeaders oH)
        {
            if (li.ready)
            {
                this.sid = li.sid;
                this.account = li.account;
                this.server = li.server;
                this.serverTitle = li.serverTitle;
                this.nickName = li.nickName;
                this.corpsName = li.CORPS_NAME;
                this.level = li.LEVEL;
                this.vipLevel = li.VIP_LEVEL;
                this.currHeader = oH;
                this.lastUpdateDTM = DateTime.Now;
                this.Heros = new List<HeroInfo>();
                this.DecreeHeros = new List<DecreeInfo>();
                for (int idx = 0; idx < 7; idx++) this.BossWarHeros[idx] = 0;
                this.BossWarChiefIdx = -1;
                this.BossWarBody = "";
                this.BossWarCount = 0;
            } 
        }
    }
}
