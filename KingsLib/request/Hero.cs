using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Hero
    {
        private const string CMD_assessScore = "Hero.assessScore";
        private const string CMD_getConvenientFormations = "Hero.getConvenientFormations";
        private const string CMD_getCurrRecommendActivityInfo = "Hero.getCurrRecommendActivityInfo";
        private const string CMD_getFeastInfo = "Hero.getFeastInfo";
        private const string CMD_getHeroIconInfo = "Hero.getHeroIconInfo";
        private const string CMD_getPlayerHeroList = "Hero.getPlayerHeroList";
        private const string CMD_getScoreHero = "Hero.getScoreHero";
        private const string CMD_getVisitHeroInfo = "Hero.getVisitHeroInfo";
        private const string CMD_getWineInfo = "Hero.getWineInfo";
        private const string CMD_matchHero = "Hero.matchHero";


        public static RequestReturnObject assessScore(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_assessScore);
        }

        public static RequestReturnObject getConvenientFormations(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getConvenientFormations);
        }

        public static RequestReturnObject getCurrRecommendActivityInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getCurrRecommendActivityInfo);
        }

        public static RequestReturnObject getFeastInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getFeastInfo);
        }

        public static RequestReturnObject getHeroIconInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getHeroIconInfo);
        }

        public static RequestReturnObject getPlayerHeroList(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getPlayerHeroList);
        }

        public static RequestReturnObject getScoreHero(HTTPRequestHeaders oH, string sid, string body)
        {
            return com.SendGenericRequest(oH, sid, CMD_getScoreHero, true, body);
        }

        public static RequestReturnObject getWineInfo(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_getWineInfo);
        }




    }
}
