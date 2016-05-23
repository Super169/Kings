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

        /*
           System Flow:
           - goTravel
               - goOneStep
                 - goPassAttack
                   - goHandleMove (if remainStep > 0)
                 - goDice
                 - (ZHANDOU ? goPassAttack : goDice)

        */

        private const string MAP_ZHANDOU = "ZHANDOU";
        private const string MAP_KONG = "KONG";
        private const string MAP_BAOXIANG = "BAOXIANG";
        private const string MAP_FANPAI = "FANPAI";
        private const string MAP_SHANGDIAN = "SHANGDIAN";

        private static TravelMapInfo goGetMapInfo(HTTPRequestHeaders oH, string sid)
        {
            RequestReturnObject rro = Travel.getMapInfo(oH, sid);
            if (!rro.SuccessWithJson("simpleMap")) return new TravelMapInfo();
            return new TravelMapInfo(rro.responseJson);
        }

        public static bool goDice(int mode, GameAccount oGA, ref TravelMapInfo mapInfo, ref int[] boxInfo, ref int actStep, ref int nextStep, DelegateUpdateInfo updateInfo)
        {
            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;
            RequestReturnObject rro;
            int goStep = -1;
            int checkPos = mapInfo.currStep;
            for (int i = 1; i <= 6; i++)
            {
                checkPos++;
                if (checkPos > mapInfo.mapSize) checkPos -= mapInfo.mapSize;
                if (mode == 0)
                {
                    if (((mapInfo.diceNum <= 25) && (boxInfo[checkPos] == 2)) ||
                        ((mapInfo.diceNum <= 20) && (boxInfo[checkPos] == 1)))
                    {
                        goStep = i;
                        break;
                    }

                } else
                {
                    if (mapInfo.simpleMap[checkPos] == MAP_SHANGDIAN)
                    {
                        goStep = i;
                        break;
                    }
                }
            }
            if ((goStep >= 1) && (goStep <= 6))
            {
                rro = Travel.controlDice(oH, sid, goStep);
                if (rro.ok != 1) return false;
                nextStep = mapInfo.currStep + goStep;
                if (nextStep > mapInfo.mapSize) nextStep -= mapInfo.mapSize;
                if (updateInfo != null) updateInfo(string.Format("{0}周遊: 餘下{1} 次, 指定擲出 {2}, 將會前進到 {3}", oGA.msgPrefix, mapInfo.diceNum, goStep, nextStep));
                actStep = nextStep;
            }
            else
            {
                rro = Travel.dice(oH, sid);
                if (!rro.SuccessWithJson()) return false;
                mapInfo.diceNum--;
                int num1 = JSON.getInt(rro.responseJson, "num1");
                int num2 = JSON.getInt(rro.responseJson, "num2");
                actStep = mapInfo.currStep + num1 + num2;
                if (actStep > mapInfo.mapSize) actStep -= mapInfo.mapSize;
                nextStep = JSON.getInt(rro.responseJson, "nextStep");
                if (updateInfo != null) updateInfo(string.Format("{0}周遊: 餘下{1} 次, 擲出 {2} {3}, 將會前進到 {4} - {5}", oGA.msgPrefix, mapInfo.diceNum, num1, num2, nextStep, actStep));
                if (nextStep < 0)
                {
                    // Invalid dice (i.e. not allow die at this moment, e.g. dice before battle completed
                    return false;
                }
                if (nextStep > mapInfo.mapSize) return false;
            }
            return true;
        }


        public static int goOneStep(int mode, GameAccount oGA, ref TravelMapInfo mapInfo, ref int[] boxInfo, string heros, int[] heroList, bool escapeBattle, DelegateUpdateInfo updateInfo)
        {
            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;

            int nextStep = mapInfo.currStep;
            int actStep = nextStep + mapInfo.remainSteps;
            if (actStep > mapInfo.mapSize) actStep -= mapInfo.mapSize;

            string stepType;
            stepType = mapInfo.simpleMap[nextStep];

            AttackResult attackResult = goPassAttack(oGA, ref mapInfo, ref boxInfo, heros, updateInfo);

            if (attackResult == AttackResult.error) return -1;

            // Try even failEscape

            if (!goDice(mode, oGA, ref mapInfo, ref boxInfo, ref actStep, ref nextStep, updateInfo)) return -1;
            stepType = mapInfo.simpleMap[nextStep];

            if (stepType == "ZHANDOU")
            {
                mapInfo.currStep = nextStep;
                attackResult = goPassAttack(oGA, ref mapInfo, ref boxInfo, heros, updateInfo);
                if (attackResult == AttackResult.error) return -1;
            }
            else
            {
                goHandleMove(oGA, ref mapInfo, ref boxInfo, nextStep, updateInfo);
            }
            return nextStep;
        }


        public static bool goTravel(GameAccount oGA, DelegateUpdateInfo updateInfo)
        {
            DateTime now = DateTime.Now;
            int mode = 0;
            int hour = now.Hour;
            if (hour < 5) mode = 0;
            else if (hour < 9) mode = 1;
            else if (hour < 12) mode = 2;
            else if (hour < 16) mode = 3;
            else mode = 0;

            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;
            TravelMapInfo mapInfo = goGetMapInfo(oH, sid);
            if (!mapInfo.ready) return false;
            if (mapInfo.simpleMap.Length == 0) return false;


            int[] boxInfo = new int[mapInfo.mapSize + 1];

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
            int errCount = 0;
            if (mode == 0)
            {
                // Play mode
                while (mapInfo.diceNum > 0)
                {
                    nextStep = goOneStep(0, oGA, ref mapInfo, ref boxInfo, oGA.BossWarBody, oGA.BossWarHeros, false, updateInfo);

                    if (nextStep == -1)
                    {
                        errCount++;
                        if (errCount > 0)
                        {
                            if (updateInfo != null) updateInfo(string.Format("{0}周遊: 出現多次錯誤離開", oGA.msgPrefix));
                            break;
                        }
                    }
                    mapInfo = goGetMapInfo(oH, sid);
                }
            }
            else
            {
                // Go shopping only
                if (mapInfo.simpleMap[mapInfo.currStep] == MAP_SHANGDIAN)
                {
                    goHandleMove(oGA, ref mapInfo, ref boxInfo, mapInfo.currStep, updateInfo);
                }
                else
                {
                    int tryCnt = 0;
                    while ((mapInfo.simpleMap[mapInfo.currStep] != MAP_SHANGDIAN) && (mapInfo.diceNum > 0))
                    {
                        // need to move to shop
                        nextStep = goOneStep(mode, oGA, ref mapInfo, ref boxInfo, oGA.BossWarBody, oGA.BossWarHeros, false, updateInfo);
                        if (nextStep == -1)
                        {
                            errCount++;
                            if (errCount > 0)
                            {
                                if (updateInfo != null) updateInfo(string.Format("{0}周遊: 出現多次錯誤, 暫時放棄", oGA.msgPrefix));
                                break;
                            }
                        } else
                        {
                            if (mapInfo.simpleMap[nextStep] != MAP_SHANGDIAN)
                            {
                                tryCnt++;
                                if (tryCnt > 5)
                                {
                                    if (updateInfo != null) updateInfo(string.Format("{0}周遊: 未能到達商店, 暫時放棄", oGA.msgPrefix));
                                    break;
                                }
                            }
                        }
                        mapInfo = goGetMapInfo(oH, sid);
                    }
                }
            }

            return true;
        }


        private enum AttackResult
        {
            error, failEscape, ready
        }

        private static AttackResult goPassAttack(GameAccount oGA, ref TravelMapInfo mapInfo, ref int[] boxInfo, string heros, DelegateUpdateInfo updateInfo)
        {
            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;

            mapInfo = goGetMapInfo(oH, sid);
            if (!mapInfo.ready) return AttackResult.error;

            int nextStep = mapInfo.currStep;
            if (mapInfo.simpleMap[nextStep] != MAP_ZHANDOU) return AttackResult.ready;

            if (updateInfo != null) updateInfo(string.Format("{0}周遊: 在 {1} 進行戰鬥", oGA.msgPrefix, nextStep));
            RequestReturnObject rro;

            bool goBattle = true;
            int failCount = 0;
            while (goBattle)
            {
                Thread.Sleep(1000);
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
                    failCount++;
                    if (failCount >= 3)
                    {
                        if (updateInfo != null) updateInfo(string.Format("{0}周遊: 逃避 {1} 的戰鬥", oGA.msgPrefix, nextStep));
                        rro = Travel.escape(oH, sid);
                        // unexpected fail, nothing can do if it cannot escape
                        if (rro.ok != 1) return AttackResult.failEscape;
                        goBattle = false;
                    }
                }
                else
                {
                    if (updateInfo != null) updateInfo(string.Format("{0}周遊: 在 {1} 的戰鬥中獲勝", oGA.msgPrefix, nextStep));
                    mapInfo.simpleMap[nextStep] = MAP_KONG;
                    goBattle = false;
                }
            }
            // Battle win or escaped
            if (mapInfo.remainSteps > 0)
            {
                nextStep = nextStep + mapInfo.remainSteps;
                if (nextStep > mapInfo.mapSize) nextStep -= mapInfo.mapSize;
                // TODO: go handle next step
                goHandleMove(oGA, ref mapInfo, ref boxInfo, nextStep, updateInfo);
                mapInfo.currStep = nextStep;
                mapInfo.remainSteps = 0;
            }
            return AttackResult.ready;
        }

        private static bool goHandleMove(GameAccount oGA, ref TravelMapInfo mapInfo, ref int[] boxInfo, int nextStep, DelegateUpdateInfo updateInfo)
        {
            HTTPRequestHeaders oH = oGA.currHeader;
            string sid = oGA.sid;
            RequestReturnObject rro;
            string stepType = mapInfo.simpleMap[nextStep];

            if (updateInfo != null) updateInfo(string.Format("{0}周遊: 到達 {1} - {2}", oGA.msgPrefix, nextStep, stepType));
            switch (stepType)
            {
                case MAP_KONG:
                    break;
                case MAP_BAOXIANG:
                    Thread.Sleep(1000);
                    rro = Travel.arriveStep(oH, sid, nextStep);
                    if (!rro.SuccessWithJson("step", "stepType")) return false;
                    if (rro.responseJson["step"]["stepType"] == "BAOXIANG")
                    {
                        mapInfo.simpleMap[nextStep] = "KONG";
                        boxInfo[nextStep] = 0;
                    }
                    break;
                case MAP_FANPAI:
                    Thread.Sleep(1000);
                    rro = Travel.arriveStep(oH, sid, nextStep);
                    Thread.Sleep(1000);
                    rro = TurnCardReward.getTurnCardRewards(oH, sid);
                    Thread.Sleep(1000);
                    rro = TurnCardReward.getTurnCardRewards(oH, sid);
                    break;
                case "SHANGDIAN":
                    Thread.Sleep(1000);
                    int buyCnt = 0;
                    rro = Shop.getTravelShopInfo(oH, sid);
                    if (rro.SuccessWithJson("items", typeof(DynamicJsonArray)))
                    {
                        DynamicJsonArray items = rro.responseJson["items"];
                        for (int idx = 0; idx < 3; idx++)
                        {
                            dynamic o = items.ElementAt(idx);
                            if (!JSON.getBool(o, "sold"))
                            {
                                int config = JSON.getInt(o, "config");
                                if (isBuyConfig(config))
                                {
                                    buyCnt++;
                                    rro = Shop.buyTravelShopItem(oH, sid, idx);
                                }
                            }
                        }
                    }
                    if ((buyCnt > 0) && (updateInfo != null)) updateInfo(string.Format("{0}周遊: 在 {1} 買了 {2} 件物品", oGA.msgPrefix, nextStep, buyCnt));
                    break;
            }
            mapInfo.currStep = nextStep;
            return true;
        }

        private static bool isBuyConfig(int config)
        {
            // Known poor item: 
            //   經驗書: 3, 4, 8, 9 , 14 ; 拜帖: 17 ; 烈酒: 16, 21, 26 ; 虎符: 69 ; 
            if (config > 310) return false;  // seems gold item are in this range, need further testing
            int[] poorItem = { 4, 8, 9, 14, 17, 21, 26, 69 };
            int findItem = poorItem.FirstOrDefault(x => x == config);
            if (findItem > 0) return false;
            return true;
        }

    }
}
