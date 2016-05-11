using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsInterface.data
{
    public class DecreeInfo : IInfoObject
    {
        static string[] DECREE_NAME = { "", "騎馳", "戍卒", "口賦", "軍屯", "射禦", "戰陣", "列校", "女騎", "兵戶", "鱗甲", "冶煉", "??" };
        public string name()
        {
            return DECREE_NAME[decId];
        }

        public int decId { get; set; }
        public int[] heroIdx { get; set; } = new int[5];
        public string[] heroName { get; set; } = new string[5];

        public DecreeInfo()
        {

        }

        public DecreeInfo(dynamic json)
        {
            try
            {
                this.decId = json.decId;

                if (json.heroIdx is DynamicJsonArray)
                {
                    heroIdx = util.getInts(json.heroIdx);
                }
                if (json.heroName is DynamicJsonArray)
                {
                    heroIdx = util.getStrings(json.heroName);
                }
            }
            catch
            {
            }
        }
        
        public  dynamic toJson()
        {
            dynamic json = Json.Decode("{}");
            try
            {
                json.decId = this.decId;
                json.heroIdx = this.heroIdx;
                json.heroName = this.heroName;
            }
            catch (Exception) { }
            return json;
        }

    }
}
