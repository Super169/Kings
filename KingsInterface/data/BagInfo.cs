using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.data
{
    public class BagInfo
    {
        public int idx { get; set; }
        public string nm { get; set; }
        public int n { get; set; }
        public bool us { get; set; }

        public bool AutoUseItem()
        {
            if (this.idx < 0) return false;
            if (!this.us) return false;
            if (this.n <= 0) return false;
            if (nm == "喇叭") return true;
            if (nm.EndsWith("色寶物包")) return true;
            if (nm.EndsWith("色軍械包")) return true;
            return false;
        }
    }

}
