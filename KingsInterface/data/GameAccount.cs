using Fiddler;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsInterface.data
{
    public enum AccountStatus { Online, Offline, Unknown }

    public static class KEY
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

    public class GameAccount
    {
        public string sid { get; set; }
        public string account { get; set; }
        public AccountStatus status { get; set; }
        public int timeAdjust { get; set; }
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
        public List<auto.TaskInfo> AutoTasks { get; set; }

        #region "Constructors"

        public GameAccount(LoginInfo li, HTTPRequestHeaders oH)
        {
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
                this.currHeader = oH;
                this.lastUpdateDTM = DateTime.Now;
                this.Heros = new List<HeroInfo>();
                this.DecreeHeros = new List<DecreeInfo>();
                for (int idx = 0; idx < 7; idx++) this.BossWarHeros[idx] = 0;
                this.BossWarChiefIdx = -1;
                this.BossWarBody = "";
                this.BossWarCount = 0;
                this.AutoTasks = new List<auto.TaskInfo>();
            }
        }

        public GameAccount(util.GenericFileRecord gfr)
        {
            if (gfr == null) return;
            if ((this.account != null) && (gfr.key != this.account)) return;

            this.sid = JSON.getString(gfr.getObject(KEY.sid));
            this.account = JSON.getString(gfr.getObject(KEY.account));
            // Set to unknown and wait for first call to check status
            // this.status = (AccountStatus)JSON.getInt(gfr.getObject(KEY.status));
            this.status = AccountStatus.Unknown;
            this.timeAdjust = JSON.getInt(gfr.getObject(KEY.timeAdjust));
            this.server = JSON.getString(gfr.getObject(KEY.server));
            this.serverTitle = JSON.getString(gfr.getObject(KEY.serverTitle));
            this.nickName = JSON.getString(gfr.getObject(KEY.nickName));
            this.corpsName = JSON.getString(gfr.getObject(KEY.corpsName));
            this.level = JSON.getString(gfr.getObject(KEY.level));
            this.vipLevel = JSON.getString(gfr.getObject(KEY.vipLevel));
            this.currHeader = util.headerFromJsonString(JSON.getString(gfr.getObject(KEY.currHeader)));
            this.lastUpdateDTM = JSON.getDateTime(gfr.getObject(KEY.lastUpdateDTM));


            // this.Heros = JSON.getString(gfr.getObject(KEY.Heros));
            // this.DecreeHeros = JSON.getString(gfr.getObject(KEY.DecreeHeros));
            // this.BossWarHeros = JSON.getIntArray(JSON.getString(gfr.getObject(KEY.BossWarHeros)));
            this.BossWarChiefIdx = JSON.getInt(gfr.getObject(KEY.BossWarChiefIdx));
            this.BossWarBody = JSON.getString(gfr.getObject(KEY.BossWarBody));
            this.BossWarCount = JSON.getInt(gfr.getObject(KEY.BossWarCount));
            //this.AutoTasks = JSON.getString(gfr.getObject(KEY.AutoTasks));

            dynamic json = null;
            DynamicJsonArray dja = null;
            string jsonString = "";


            this.Heros = new List<HeroInfo>();
            try
            {
                jsonString = JSON.getString(gfr.getObject(KEY.Heros));
                json = Json.Decode(jsonString);
                if ((json["data"] != null) && (json["data"].GetType() == typeof(DynamicJsonArray))) {
                    foreach (dynamic o in json["data"])
                    {
                        this.Heros.Add(new HeroInfo(o));
                    }
                }
            }
            catch
            {
                this.Heros = new List<HeroInfo>();
            }


            this.DecreeHeros = new List<DecreeInfo>();
            try
            {
                jsonString = JSON.getString(gfr.getObject(KEY.DecreeHeros));
                json = Json.Decode(jsonString);
                if ((json["data"] != null) && (json["data"].GetType() == typeof(DynamicJsonArray)))
                {
                    foreach (dynamic o in json["data"])
                    {
                        this.DecreeHeros.Add(new DecreeInfo(o));
                    }
                }
            }
            catch
            {
                this.DecreeHeros = new List<DecreeInfo>();
            }

            try
            {
                jsonString = JSON.getString(gfr.getObject(KEY.BossWarHeros));
                json = Json.Decode(jsonString);
                this.BossWarHeros = JSON.getIntArray(json, "data");
            }
            catch
            {
                this.BossWarHeros = new int[7];
            }

            this.Heros = new List<HeroInfo>();
            try
            {
                jsonString = JSON.getString(gfr.getObject(KEY.Heros));
                json = Json.Decode(jsonString);
                dja = json.data;
                foreach (dynamic o in dja)
                {
                    this.Heros.Add(new HeroInfo(o));
                }
            } catch
            {
                // reset all data for any error
                this.Heros = new List<HeroInfo>();
            }

            this.DecreeHeros = new List<DecreeInfo>();
            try
            {
                jsonString = JSON.getString(gfr.getObject(KEY.DecreeHeros));
                json = Json.Decode(jsonString);
                dja = json.data;
                foreach (dynamic o in dja)
                {
                    this.DecreeHeros.Add(new DecreeInfo(o));
                }
            }
            catch
            {
                // reset all data for any error
                this.DecreeHeros = new List<DecreeInfo>();
            }



        }

        #endregion

        public string accInfo()
        {
            return string.Format("{0} - {1}", this.serverTitle, this.nickName);
        }

        public string msgPrefix()

        {
            return string.Format("【{0} - {1}】", this.serverTitle, this.nickName);
        }

        public AccountStatus CheckStatus(bool forceCheck = false)
        {
            // Offline cannot be changed to online without new login detected
            if ((!forceCheck) &&(this.status == AccountStatus.Offline)) return AccountStatus.Offline;

            RequestReturnObject rro = action.go_System_ping(currHeader, sid);
            if (rro.prompt == action.PROMPT_RELOGIN)
            {
                this.status = AccountStatus.Offline;
            }
            else if (rro.SuccessWithJson("clientTime") && rro.SuccessWithJson("serverTime"))
            {
                this.status = AccountStatus.Online;
                Int64 clientTime = Convert.ToInt64(rro.responseJson["clientTime"]);
                Int64 serverTime = Convert.ToInt64(rro.responseJson["serverTime"]);
                this.timeAdjust = (int)(serverTime - clientTime);
            }
            else
            {
                this.status = AccountStatus.Offline;
            }
            return this.status;
        }

        public bool IsOnline()
        {
            return (status == AccountStatus.Online);
        }

        public util.GenericFileRecord toGenericFileRecord()
        {
            util.GenericFileRecord gfr = new util.GenericFileRecord(this.account);
            gfr.saveObject(KEY.sid, this.sid);
            gfr.saveObject(KEY.account, this.account);
            gfr.saveObject(KEY.status, this.status);
            gfr.saveObject(KEY.timeAdjust, this.timeAdjust);
            gfr.saveObject(KEY.server, this.server);
            gfr.saveObject(KEY.serverTitle, this.serverTitle);
            gfr.saveObject(KEY.nickName, this.nickName);
            gfr.saveObject(KEY.corpsName, this.corpsName);
            gfr.saveObject(KEY.level, this.level);
            gfr.saveObject(KEY.vipLevel, this.vipLevel);
            gfr.saveObject(KEY.currHeader, util.header2JsonString(this.currHeader));
            gfr.saveObject(KEY.lastUpdateDTM, this.lastUpdateDTM);
            gfr.saveObject(KEY.BossWarChiefIdx, this.BossWarChiefIdx);
            gfr.saveObject(KEY.BossWarBody, this.BossWarBody);
            gfr.saveObject(KEY.BossWarCount, this.BossWarCount);

            // Data with array 
            // List<object> jsonArray = null;
            dynamic json = null;

            json = Json.Decode("{}");
            json.data = this.BossWarHeros;
            gfr.saveObject(KEY.BossWarHeros, Json.Encode(json));

            /*
            json = Json.Decode("{}");
            jsonArray = new List<dynamic>();
            foreach (HeroInfo hi in this.Heros)
            {
                jsonArray.Add(hi.toJson());
            }
            json.data = new DynamicJsonArray(jsonArray.ToArray());
            gfr.saveObject(KEY.Heros, Json.Encode(json));
            */
            /*
            json = Json.Decode("{}");
            jsonArray = new List<dynamic>();
            foreach (DecreeInfo hi in this.DecreeHeros)
            {
                jsonArray.Add(hi.toJson());
            }
            json.data = new DynamicJsonArray(jsonArray.ToArray());
            gfr.saveObject(KEY.DecreeHeros, Json.Encode(json));
            */
            gfr.saveObject(KEY.Heros, util.infoBaseListToJsonString(this.Heros.ToArray()));

            gfr.saveObject(KEY.DecreeHeros, util.infoBaseListToJsonString(this.DecreeHeros.ToArray()));

            //gfr.saveObject(KEY.DecreeHeros, this.DecreeHeros);
            //gfr.saveObject(KEY.AutoTasks, this.AutoTasks);
            return gfr;
        }

    }
}
