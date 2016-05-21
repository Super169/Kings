using Fiddler;
using KingsInterface.data;
using KingsInterface.request;
using MyUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        public static bool goDice(HTTPRequestHeaders oH, string sid, ref TravelMapInfo mapInfo, ref int[] boxInfo, ref int actStep, ref int nextStep, DelegateUpdateInfo updateInfo)
        {
            RequestReturnObject rro;
            int goStep = -1;
            int checkPos = mapInfo.currStep;
            for (int i = 1; i <= 6; i++)
            {
                checkPos++;
                if (checkPos > mapInfo.mapSize) checkPos -= mapInfo.mapSize;
                if (((mapInfo.diceNum <= 25) && (boxInfo[checkPos] == 2)) || 
                    ((mapInfo.diceNum <= 15) && (boxInfo[checkPos] == 1)))
                {
                    goStep = i;
                    break;
                }
            }
            if ((goStep >= 1) && (goStep <= 6))
            {
                rro = Travel.controlDice(oH, sid, goStep);
                if (rro.ok != 1) return false;
                nextStep = mapInfo.currStep + goStep;
                if (nextStep > mapInfo.mapSize) nextStep -= mapInfo.mapSize;
                actStep = nextStep;
            }
            else
            {
                rro = Travel.dice(oH, sid);
                rro = Travel.dice(oH, sid);
                if (!rro.SuccessWithJson()) return false;
                mapInfo.diceNum--;
                int num1 = JSON.getInt(rro.responseJson, "num1");
                int num2 = JSON.getInt(rro.responseJson, "num2");
                actStep = mapInfo.currStep + num1 + num2;
                if (actStep > mapInfo.mapSize) actStep -= mapInfo.mapSize;
                nextStep = JSON.getInt(rro.responseJson, "nextStep");
                updateInfo(string.Format("擲出 {0} {1}, 將會前進到 {2} - {3}", num1, num2, nextStep, actStep));
                if (nextStep < 0)
                {
                    // Invalid dice (i.e. not allow die at this moment, e.g. dice before battle completed
                    return false;
                }
                if (nextStep > mapInfo.mapSize) return false;
            }
            return true;
        }


        public static int goOneStep(HTTPRequestHeaders oH, string sid, ref TravelMapInfo mapInfo, ref int[] boxInfo, string heros, int[] heroList, bool escapeBattle, DelegateUpdateInfo updateInfo)
        {
            RequestReturnObject rro;
            int nextStep = mapInfo.currStep;
            int actStep = nextStep + mapInfo.remainSteps;
            if (actStep > mapInfo.mapSize) actStep -= mapInfo.mapSize;

            string stepType;
            stepType = mapInfo.simpleMap[nextStep];

            if (stepType == "ZHANDOU")
            {
                // Need to finish or escapte this ZHANDOU first; so go to ZHANDOU the same as after dice
            }
            else
            {
                /*
                rro = Travel.dice(oH, sid);
                if (!rro.SuccessWithJson()) return -1;
                mapInfo.diceNum--;
                int num1 = JSON.getInt(rro.responseJson, "num1");
                int num2 = JSON.getInt(rro.responseJson, "num2");
                actStep = mapInfo.currStep + num1 + num2;
                if (actStep > mapInfo.mapSize) actStep -= mapInfo.mapSize;
                nextStep = JSON.getInt(rro.responseJson, "nextStep");
                if (nextStep < 0)
                {
                    // Invalid dice (i.e. not allow die at this moment, e.g. dice before battle completed
                    return -1;
                }
                if (nextStep >= mapInfo.simpleMap.Length) return -1;
                */
                if (!goDice(oH, sid, ref mapInfo, ref boxInfo, ref actStep, ref nextStep, updateInfo)) return -1;
                stepType = mapInfo.simpleMap[nextStep];
            }

            if (stepType == "ZHANDOU")
            {
                if (escapeBattle)
                {
                    if (updateInfo != null) updateInfo(string.Format("逃避 {0} - {1}", nextStep, stepType));
                    rro = Travel.escape(oH, sid);
                    if (rro.ok != 1) return nextStep;

                    if (mapInfo.remainSteps > 0)
                    {
                        nextStep = actStep;
                        rro = Travel.viewStep(oH, sid, nextStep);
                        stepType = mapInfo.simpleMap[nextStep];
                    }
                    else
                    {
                        if (!goDice(oH, sid, ref mapInfo, ref boxInfo, ref actStep, ref nextStep, updateInfo)) return -1;
                        stepType = mapInfo.simpleMap[nextStep];

                        /*
                        rro = Travel.dice(oH, sid);
                        if (!rro.SuccessWithJson()) return -1;
                        mapInfo.diceNum--;
                        int num1 = JSON.getInt(rro.responseJson, "num1");
                        int num2 = JSON.getInt(rro.responseJson, "num2");
                        actStep = mapInfo.currStep + num1 + num2;
                        if (actStep > mapInfo.mapSize) actStep -= mapInfo.mapSize;
                        nextStep = JSON.getInt(rro.responseJson, "nextStep");
                        if (nextStep >= mapInfo.simpleMap.Length) return -1;
                        stepType = mapInfo.simpleMap[nextStep];
                        */

                    }
                }
                else
                {
                    if (updateInfo != null) updateInfo(string.Format("在 {0} - {1}", nextStep, stepType));
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
                    Thread.Sleep(2000);
                    rro = Travel.viewStep(oH, sid, nextStep);
                    Thread.Sleep(1000);
                    rro = Travel.attack(oH, sid, nextStep);
                    rro = Campaign.getAttFormation(oH, sid, "TRAVEL");
                    Thread.Sleep(1000);
                    rro = Campaign.nextEnemies(oH, sid);
                    Thread.Sleep(1000);
                    rro = Campaign.saveFormation(oH, sid, heros);
                    Thread.Sleep(1000);
                    rro = Campaign.fightNext(oH, sid);
                    Thread.Sleep(1000);
                    // rro = Hero.getScoreHero(oH, sid, heroBody);
                    rro = Campaign.quitCampaign(oH, sid);
                    rro = Travel.getMapInfo(oH, sid);
                    rro = Travel.arriveStep(oH, sid, nextStep);
                    if ((!rro.success) || (rro.prompt == PROMPT_NOT_SUPPORT))
                    {
                        // Battle failed
                        return nextStep;
                    }
                    nextStep = actStep;
                    rro = Travel.viewStep(oH, sid, nextStep);
                    stepType = mapInfo.simpleMap[nextStep];

                }
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
                        boxInfo[nextStep] = 0;
                    }
                    break;
                case "FANPAI":
                    Thread.Sleep(1000);
                    rro = Travel.arriveStep(oH, sid, nextStep);
                    Thread.Sleep(1000);
                    rro = TurnCardReward.getTurnCardRewards(oH, sid);
                    Thread.Sleep(1000);
                    rro = TurnCardReward.getTurnCardRewards(oH, sid);
                    break;
                case "SHANGDIAN":
                    rro = Shop.getTravelShopInfo(oH, sid);
                    rro = Shop.buyTravelShopItem(oH, sid, 0);
                    rro = Shop.buyTravelShopItem(oH, sid, 1);
                    rro = Shop.buyTravelShopItem(oH, sid, 2);
                    break;

            }
            return nextStep;
        }


        public static bool goTravel(GameAccount oGA, DelegateUpdateInfo updateInfo)
        {
            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;
            TravelMapInfo mapInfo = goGetMapInfo(oH, sid);
            if (!mapInfo.ready) return false;
            if (mapInfo.simpleMap.Length == 0) return false;

            int[] boxInfo = new int[mapInfo.mapSize+1];

            // Read BAOXIANG
            RequestReturnObject rro;
            for (int i = 1; i <= mapInfo.mapSize; i++)
            {
                if (mapInfo.simpleMap[i] == "BAOXIANG")
                {
                    rro = Travel.viewStep(oH, sid, i);
                    if (rro.SuccessWithJson("step", "stepType") && (rro.responseJson["step"]["stepType"] == "BAOXIANG") && (rro.responseJson["step"]["reward"] != null))
                    {
                        dynamic reward = rro.responseJson["step"]["reward"];
                        if ((reward["itemDefs"] != null) && (reward["itemDefs"].GetType() == typeof(DynamicJsonArray)))
                        {
                            DynamicJsonArray itemDefs = reward["itemDefs"];
                            foreach (dynamic o in itemDefs)
                            {
                                string name = JSON.getString(o, "name", "");
                                if (name.EndsWith("金色寶物包") || name.EndsWith("金色宝物包"))
                                {
                                    boxInfo[i] = 2;
                                }
                                else if (name.EndsWith("色寶物包") || name.EndsWith("色宝物包"))
                                {
                                    boxInfo[i] = 1;
                                }

                            }
                        }

                    }
                }
            }

            int nextStep = 0;
            int loseBattle = 0;
            while (mapInfo.diceNum > 0)
            {
                nextStep = goOneStep(oH, sid, ref mapInfo, ref boxInfo, oGA.BossWarBody, oGA.BossWarHeros, false, updateInfo);

                if (nextStep == -1)
                {
                    // Fail in battle
                    loseBattle++;
                    if (loseBattle >= 3)
                    {
                        nextStep = goOneStep(oH, sid, ref mapInfo, ref boxInfo, oGA.BossWarBody, oGA.BossWarHeros, true, updateInfo);
                    }
                }
                else
                {
                    loseBattle = 0;
                }
                // For safety, get MapInfo everytime; may remove it later if the program is well tested
                mapInfo = goGetMapInfo(oH, sid);
            }
            return true;
        }

    }
}
