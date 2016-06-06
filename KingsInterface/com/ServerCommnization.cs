using Fiddler;
using KingsInterface.data;
using MyUtil;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsInterface
{
    public static partial class com
    {

        public static RequestReturnObject SendRequest(HTTPRequestHeaders oH, string requestText)
        {
            int goType = 2;
            RequestReturnObject rro;
            if (goType == 1)
            {
                rro = SendRequest1(oH, requestText);
            }
            else
            {
                rro = SendRequest2(oH, requestText);
            }
            return rro;
        }

        public static RequestReturnObject SendRequest2(HTTPRequestHeaders oH, string requestText)
        {
            RequestReturnObject rro = new RequestReturnObject();
            rro.success = false;
            rro.ok = -1;
            rro.msg = "";
            rro.session = null;
            rro.requestText = requestText;

            try
            {
                string result = "";
                try
                {
                    // string Uri = oH["Host"];
                    string uri = "http://" + oH["Host"];
                    string FullPath = uri + "/m.do";

                    HttpContent _Body = new StringContent(requestText);
                    _Body.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpClient client = new HttpClient();
                    HttpClientHandler handler = new HttpClientHandler();

                    string cookiesStr = JSON.getString(oH["Cookie"], null);
                    int cookiesCnt = 0;
                    if (cookiesStr != null)
                    {
                        handler.CookieContainer = new CookieContainer();
                        string[] cookies = cookiesStr.Split(';');
                        foreach (string cookie in cookies)
                        {
                            string[] c = cookie.Split('=');
                            if (c.Length == 2)
                            {
                                handler.CookieContainer.Add(new Uri(uri), new Cookie(c[0], c[1]));
                                cookiesCnt++;
                            }
                        }
                    }
                    if (cookiesCnt > 0)
                    {
                        client = new HttpClient(handler);
                    }
                    else
                    {
                        client = new HttpClient();
                    }

                    string proxyAuthorization = JSON.getString(oH["Proxy-Authorization"]);
                    if ((proxyAuthorization != null) && (proxyAuthorization != ""))
                    {
                        client.DefaultRequestHeaders.Add("Proxy-Authorization", proxyAuthorization);
                    }

                    HttpResponseMessage response = client.PostAsync(FullPath, _Body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        HttpContent content = response.Content;
                        result = content.ReadAsStringAsync().Result;
                        rro.success = true;
                        rro.responseText = result;
                        rro.responseJson = com.getJsonFromResponse(rro.responseText, true);
                        if (rro.responseJson != null)
                        {
                            if (rro.responseJson["ok"] != null) rro.ok = (int)rro.responseJson["ok"];
                            if (rro.responseJson["style"] != null) rro.style = rro.responseJson["style"];
                            if (rro.responseJson["prompt"] != null) rro.prompt = rro.responseJson["prompt"];
                        }

                    }

                }
                catch (Exception ex)
                {
                    result = ex.Message;
                }

            }
            catch (Exception ex)
            {
                rro.success = false;
                rro.msg = ex.Message;
            }

            return rro;
        }

        public static RequestReturnObject SendRequest1(HTTPRequestHeaders oH, string requestText)
        {
            RequestReturnObject rro = new RequestReturnObject();
            rro.success = false;
            rro.ok = -1;
            rro.msg = "";
            rro.session = null;
            rro.requestText = requestText;

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

            // Use other try catch after succes to avoid misleading of fail in communization
            try
            {
                rro.responseText = com.GetResponseText(rro.session);
                rro.responseJson = com.getJsonFromResponse(rro.responseText, true);
                if (rro.responseJson != null)
                {
                    if (rro.responseJson["ok"] != null) rro.ok = (int)rro.responseJson["ok"];
                    if (rro.responseJson["style"] != null) rro.style = rro.responseJson["style"];
                    if (rro.responseJson["prompt"] != null) rro.prompt = rro.responseJson["prompt"];
                }
            }
            catch (Exception ex)
            {
                // In this case, communization is still success, but the result may be not a json object
                rro.msg = ex.Message;
            }

            return rro;
        }

        public static RequestReturnObject SendGenericRequest(HTTPRequestHeaders oH, string sid, string act, bool addSId = true, string body = null)
        {
            dynamic json;
            string requestText = "";
            RequestReturnObject rro;

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
                rro = new RequestReturnObject();
                rro.success = false;
                rro.ok = -1;
                rro.msg = ex.Message;
                rro.session = null;
            }
            return rro;
        }

    }
}
