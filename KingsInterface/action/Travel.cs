using Fiddler;
using KingsInterface.data;
using KingsInterface.request;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsInterface
{
    public partial class action
    {
        // ZHANDOU   - 戰鬥 (except 0)
        // KONG      - 空白
        // BAOXIANG  - 寶箱
        // FANPAI    - 翻牌
        // SHANGDIAN - 商店

        private static TravelMapInfo goGetMapInfo(HTTPRequestHeaders oH, string sid)
        {
            RequestReturnObject rro = Travel.getMapInfo(oH, sid);
            if (!rro.SuccessWithJson("simpleMap")) return new TravelMapInfo();
            return new TravelMapInfo(rro.responseJson);
        }


        public static int goOneStep(HTTPRequestHeaders oH, string sid, ref TravelMapInfo mapInfo, string heros, int[] heroList, DelegateUpdateInfo updateInfo)
        {
            RequestReturnObject rro = Travel.dice(oH, sid);
            if (!rro.SuccessWithJson()) return -1;
            int num1 = JSON.getInt(rro.responseJson, "num1");
            int num2 = JSON.getInt(rro.responseJson, "num2");
            int actStep = mapInfo.currStep + num1 + num2;
            if (actStep > mapInfo.mapSize) actStep -= mapInfo.mapSize;
            int nextStep = JSON.getInt(rro.responseJson, "nextStep");
            if (nextStep >= mapInfo.simpleMap.Length) return -1;

            string stepType = mapInfo.simpleMap[nextStep];

            if (stepType == "ZHANDOU")
            {
                string heroBody = "{\"heros\":[";
                int heroCnt = 0;
                for (int i = 0; i < heroList.Length; i++)
                {
                    if (heroList[i] > 0)
                    {
                        if (heroCnt > 0) heroBody += ",";
                        heroBody += heroList[i].ToString();
                        heroCnt++;
                    }
                }
                heroBody += "]}";
                rro = Travel.viewStep(oH, sid, nextStep);
                rro = Travel.attack(oH, sid, nextStep);
                rro = Campaign.getAttFormation(oH, sid);
                rro = Campaign.nextEnemies(oH, sid);
                rro = Campaign.saveFormation(oH, sid, heros);
                rro = Campaign.fightNext(oH, sid);
                rro = Hero.getScoreHero(oH, sid, heroBody);
                rro = Campaign.quitCampaign(oH, sid);
                rro = Travel.getMapInfo(oH, sid);
                rro = Travel.viewStep(oH, sid, nextStep);
                nextStep = actStep;
                stepType = mapInfo.simpleMap[nextStep];
            }

            /*
            rro = Travel.arriveStep(oH, sid, nextStep);
            if (!rro.SuccessWithJson("step", "stepId")) return -1;
            dynamic step = rro.responseJson["step"];
            int stepId = JSON.getInt(step, "stepId");
            if (nextStep != stepId) return -1;
            */

            if (updateInfo != null) updateInfo(string.Format("到達 {0} - {1}", nextStep, stepType));

            switch (stepType)
            {
                case "KONG":
                    break;
                case "BAOXIANG":
                    rro = Travel.arriveStep(oH, sid, nextStep);
                    if (!rro.SuccessWithJson("step", "stepType")) return -1;
                    if (rro.responseJson["step"]["stepType"] == "BAOXIANG")
                    {
                        mapInfo.simpleMap[nextStep] = "KONG";
                    }
                    break;
                case "FANPAI":
                    rro = Travel.arriveStep(oH, sid, nextStep);
                    rro = TurnCardReward.getTurnCardRewards(oH, sid);
                    rro = TurnCardReward.getTurnCardRewards(oH, sid);
                    break;
                case "SHANGDIAN":
                    rro = Shop.getTravelShopInfo(oH, sid);
                    rro = Shop.buyTravelShopItem(oH, sid, 0);
                    rro = Shop.buyTravelShopItem(oH, sid, 1);
                    rro = Shop.buyTravelShopItem(oH, sid, 2);
                    break;

            }
            mapInfo.diceNum--;
            return nextStep;
        }


        public static bool goTravel(GameAccount oGA, DelegateUpdateInfo updateInfo)
        {
            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;
            TravelMapInfo mapInfo = goGetMapInfo(oH, sid);
            if (!mapInfo.ready) return false;
            if (mapInfo.simpleMap.Length == 0) return false;
            while (mapInfo.diceNum > 0)
            {
                goOneStep(oH, sid, ref mapInfo, oGA.BossWarBody, oGA.BossWarHeros, updateInfo);
            }
            return true;
        }

    }
}
