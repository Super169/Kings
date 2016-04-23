using Fiddler;
using KingsInterface.data;
using System;
using System.Text;
using System.Web.Helpers;

namespace KingsInterface
{
    static partial class com
    {

        public static requestReturnObject SendRequest(HTTPRequestHeaders oH, string requestText)
        {
            requestReturnObject rro = new requestReturnObject();
            rro.success = false;
            rro.msg = "";
            rro.session = null;

            // For safety, Fiddler should be started before sendRequest, and it should not start here
            // Othwise, there may have concern on when should Fiddler be shutdown
            if (!isStarted())
            {
                rro.msg = "Fiddler engine not yet started";
                return rro;
            }

            if (oH == null)
            {
                rro.msg = "<<No Header provider>>";
                return rro;
            }

            try
            {
                string jsonString = requestText;
                byte[] requestBodyBytes = Encoding.UTF8.GetBytes(jsonString);
                oH["Content-Length"] = requestBodyBytes.Length.ToString();

                // TODO: need to have OnStageChangeHandler for waiting method?
                // rro.oS = FiddlerApplication.oProxy.SendRequestAndWait(oS.oRequest.headers, requestBodyBytes, null, OnStageChangeHandler);
                rro.session = FiddlerApplication.oProxy.SendRequestAndWait(oH, requestBodyBytes, null, null);
                rro.success = true;
            }
            catch (Exception ex)
            {
                rro.msg = ex.Message;
            }
            return rro;
        }

        public static requestReturnObject SendGenericRequest(HTTPRequestHeaders oH, string sid, string act, bool addSId = true, string body = null)
        {
            dynamic json;
            string requestText = "";
            requestReturnObject rro;

            try
            {
                json = Json.Decode("{}");
                json.act = act;
                if (addSId) json.sid = sid;
                if (body != null) json.body = body;
                requestText = Json.Encode(json);
                rro = SendRequest(oH, requestText);
            }
            catch (Exception ex)
            {
                rro = new requestReturnObject();
                rro.success = false;
                rro.msg = ex.Message;
                rro.session = null;
            }
            return rro;
        }

    }
}
