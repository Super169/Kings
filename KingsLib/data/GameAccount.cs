using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib
{
    public class data
    {

        public enum AccountStatus { Online, Offline, Unknown }

        public class GameAccount
        {
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
        }

    }
}
