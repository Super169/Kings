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
        public int returnCode;
        public string msg;
        public Session session;
        public string requestText;
        public string responseText;
        public dynamic responseJson;
    }

}
