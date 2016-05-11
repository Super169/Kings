using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsInterface.data
{
    public class HeroInfo : IInfoObject
    {
        public int idx { get; set;  }
        public string nm { get; set; }
        public string army { get; set; }
        public int lv { get; set; }
        public int power { get; set; }
        public int cfd { get; set; }
        public int intl { get; set; }
        public int strg { get; set; }
        public int chrm { get; set; }
        public int attk { get; set; }
        public int dfnc { get; set; }
        public int spd { get; set; }
        public int[] amftLvs { get; set; } = new int[5];

        public HeroInfo()
        {
            idx = 0;
        }

        public HeroInfo(dynamic json)
        {
            try
            {
                this.idx = json.idx;
                this.nm = json.nm;
                this.army = json.army;
                this.lv = json.lv;
                this.power = json.power;
                this.cfd = json.cfd;
                this.intl = json.intl;
                this.strg = json.strg;
                this.chrm = json.chrm;
                this.attk = json.attk;
                this.dfnc = json.dfnc;
                this.spd = json.spd;

                if (json.amftLvs is DynamicJsonArray)
                {
                    // DynamicJsonArray s = (DynamicJsonArray)json.amftLvs;
                    // for (int i = 0; i < 5; i++) this.amftLvs[i] = (int) s.ElementAt(i);
                    this.amftLvs = util.getInts(json.amftLvs);
                }
            } catch
            {
                idx = 0;
            }
        }

        public  dynamic toJson()
        {
            dynamic json = Json.Decode("{}");
            try
            {
                json.idx = this.idx;
                json.nm = this.nm;
                json.army = this.army;
                json.lv = this.lv;
                json.power = this.power;
                json.cfd = this.cfd;
                json.intl = this.intl;
                json.strg = this.strg;
                json.chrm = this.chrm;
                json.attk = this.attk;
                json.dfnc = this.dfnc;
                json.spd = this.spd;
                json.amftLvs = this.amftLvs;
            }
            catch (Exception) { }
            return json;
        }

    }
}
