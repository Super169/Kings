using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.data
{
    public class Industry
    {
        // If use enum for type, may have error when the type is not defined;
        // so for safety until all types found, use const instead

        public const string TYPE_MARKET = "MARKET";

        public int industryId { get; set; }
        public string city { get; set; }
        public string type { get; set;  }

        // ignore reward here as it's meaningless

        public bool isMarket()
        {
            return (type == TYPE_MARKET);
        }
    }
}
