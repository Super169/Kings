using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsInterface.data
{
    public class TravelMapInfo
    {
        public bool ready;
        public int playerLevel { get; set; }
        public int currStep { get; set; }
        public int remainSteps { get; set; }
        public int diceNum { get; set; }
        public int nextDicePrice { get; set; }
        public int chipsNum { get; set; }
        public int avaBuyDice { get; set; }
        public int total { get; set; }
        public string[] simpleMap { get; set; }
        public int[] boxInfo { get; set; }
        public int mapSize { get; set; }

        public TravelMapInfo()
        {
            this.ready = false;
        }

        public TravelMapInfo(dynamic json)
        {
            this.ready = false;
            if (json == null) return;
            if ((json["simpleMap"] == null) || (json["simpleMap"].GetType() != typeof(DynamicJsonObject))) return;

            this.playerLevel = JSON.getInt(json, "playerLevel");
            this.currStep = JSON.getInt(json, "currStep");
            this.remainSteps = JSON.getInt(json, "remainSteps");
            this.diceNum = JSON.getInt(json, "diceNum");
            this.nextDicePrice = JSON.getInt(json, "nextDicePrice");
            this.chipsNum = JSON.getInt(json, "chipsNum");
            this.avaBuyDice = JSON.getInt(json, "avaBuyDice");
            this.total = JSON.getInt(json, "total");

            DynamicJsonObject djo = json["simpleMap"];
            this.mapSize = djo.GetDynamicMemberNames().Count() - 1;
            this.simpleMap = new String[mapSize + 1];
            for (int i = 0; i <= mapSize; i++)
            {
                this.simpleMap[i] = JSON.getString(json["simpleMap"], i.ToString(), "");
            }
            boxInfo = new int[mapSize + 1];
            this.ready = true;

        }
    }
}
