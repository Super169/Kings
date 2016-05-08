using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.data
{
    public class DecreeInfo
    {
        static string[] DECREE_NAME = { "", "騎馳", "戍卒", "口賦", "軍屯", "射禦", "戰陣", "列校", "女騎", "兵戶", "鱗甲", "冶煉", "??" };
        public int decId { get; set; }
        public int[] heroIdx { get; set; } = new int[5];
        public string[] heroName { get; set; } = new string[5];

        public string name()
        {
            return DECREE_NAME[decId];
        }

    }
}
