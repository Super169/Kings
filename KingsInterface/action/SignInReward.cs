using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface
{
    public partial class action
    {
        public static string goSignIn (HTTPRequestHeaders oH, string sid)
        {
            string info = "";
            try
            {
                RequestReturnObject rro = com.SendGenericRequest(oH, sid, CMD_SignInReward_signIn);
                if (rro.success)
                {
                    info = com.GetResponseText(rro.session);
                } else
                {
                    info = rro.msg;
                }
            }
            catch (Exception ex)
            {
                info = ex.Message;
            }

            return info;
        }
    }
}

