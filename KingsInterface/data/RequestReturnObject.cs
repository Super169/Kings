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
        public string msg;
        public Session session;
    }
}
