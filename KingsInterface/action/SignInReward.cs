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
                RequestReturnObject rro;
                rro = go_SignInReward_getInfo(oH, sid);
                if (rro.success)
                {

                }

                rro = go_SignInReward_signIn(oH, sid);
                if (rro.success)
                {
                    info = rro.responseText;
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

