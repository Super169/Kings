using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.data
{
    public class IndustryInfo
    {
        private int[,] foodConfig = { { 42, 91, 10000 }, { 43, 161, 200000 }, { 44, 213, 30000 }, { 45, 254, 40000 } };
        private int[,] silverConfig = { { 58, 184, 10000 }, {59,322 , 20000}, {60,427, 30000}, {61, 500,40000} };


        public int config { get; set; }
        public int discount { get; set; }
        public bool sold { get; set; }

        public int getFoodCost()
        {
            for (int i = 0; i < 4; i++)
            {
                if (this.config == foodConfig[i, 0]) return foodConfig[i, 1];
            }
            return -1;
        }

        public int getSilverCost()
        {
            for (int i = 0; i < 4; i++)
            {
                if (this.config == silverConfig[i, 0]) return silverConfig[i, 1];
            }
            return -1;
        }

        public int ItemCost(int expilot, bool buyFood, bool buySilver)
        {
            if (sold || (!buyFood && !buySilver)) return 0;
            int cost = 0;
            if (buyFood)
            {
                cost = getFoodCost();
                if ((cost > 0) && (cost <= expilot)) return cost;
            }
            if (buySilver)
            {
                cost = getSilverCost();
                if ((cost > 0) && (cost <= expilot)) return cost;
            }
            return 0;
        }

    }
}
