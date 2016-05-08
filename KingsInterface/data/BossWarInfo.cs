using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.data
{
    public class BossWarInfo
    {
        public bool enterFail { get; set; } = true;
        public bool bossAvailable { get; set; } = true;
        public bool sendFail { get; set; } = true;
        public bool leavelFail { get; set; } = true;
        public int beforeCnt { get; set; } = 0;
        public int bossHP { get; set; } = 0;
    }
}
