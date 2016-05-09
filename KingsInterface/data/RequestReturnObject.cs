using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.data
{
    public struct RequestReturnObject
    {
        public bool success;
        public int ok;
        public string style;
        public string prompt;
        public int returnCode;
        public string msg;
        public Session session;
        public string requestText;
        public string responseText;
        public dynamic responseJson;

        public bool SuccessWithJson(string key1 = null, string key2 = null)
        {
            if (!success) return false;
            if (responseJson == null) return false;
            if ((key1 == null) || (key1 == "")) return true;
            if (responseJson[key1] == null) return false;
            if ((key2 == null) || (key2 == "")) return true;
            if (responseJson[key1][key2] == null) return false;
            return true;
        }
    }

}
