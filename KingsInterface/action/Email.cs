using Fiddler;
using KingsInterface.data;
using System;
using System.Web.Helpers;

namespace KingsInterface
{
    public partial class action
    {
        public static int goEmailReadAll(HTTPRequestHeaders oH, string sid)
        {
            int readCnt = 0;
            RequestReturnObject rro = go_Email_openInBox(oH, sid);
            if ((!rro.success) || (rro.responseJson == null) || (rro.responseJson["emails"] == null)) return 0;
            DynamicJsonArray emails = rro.responseJson["emails"];
            foreach (dynamic email in emails)
            {
                try
                {
                    if (email["status"]=="NR")
                    {
                        int emailId = getInt(email, "id", -1);
                        if (emailId > 0)
                        {
                            // No need to check for fail as there has nothing can do
                            rro = go_Email_read(oH, sid, emailId);
                            rro = go_Email_getAttachment(oH, sid, emailId);
                            if (rro.ok == 1) readCnt++;
                        }
                    }
                }
                catch (Exception) { }
            }
            return readCnt;
        }
    }
}