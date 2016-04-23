using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsInterface.data
{
    public class HeroInfo
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

        public HeroInfo(dynamic hero)
        {
            try
            {
                this.idx = hero.idx;
                this.nm = hero.nm;
                this.army = hero.army;
                this.lv = hero.lv;
                this.power = hero.power;
                this.cfd = hero.cfd;
                this.intl = hero.intl;
                this.strg = hero.strg;
                this.chrm = hero.chrm;
                this.attk = hero.attk;
                this.dfnc = hero.dfnc;
                this.spd = hero.spd;

                if (hero.amftLvs is DynamicJsonArray)
                {
                    DynamicJsonArray s = (DynamicJsonArray)hero.amftLvs;
                    for (int i = 0; i < 5; i++) this.amftLvs[i] = (int) s.ElementAt(i);
                }
            } catch
            {
                idx = 0;
            }
        }
    }
}
