using Fiddler;
using KingsLib.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.request
{
    public static class Email
    {
        private const string CMD_getAttachment = "Email.getAttachment";
        private const string CMD_openInBox = "Email.openInBox";
        private const string CMD_read = "Email.read";

        public static RequestReturnObject openInBox(HTTPRequestHeaders oH, string sid)
        {
            return com.SendGenericRequest(oH, sid, CMD_openInBox);
        }


    }
}
