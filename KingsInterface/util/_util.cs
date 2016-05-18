using Fiddler;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsInterface
{
    public partial class util
    {
        public static string infoBaseListToJsonString(IInfoObject[] data)
        {
            string retJson = "{}";
            try
            {
                dynamic json = Json.Decode("{}");
                List<object> jsonArray = new List<dynamic>();
                foreach (IInfoObject ibo in data)
                {
                    jsonArray.Add(ibo.toJson());
                }
                json.data = new DynamicJsonArray(jsonArray.ToArray());
                retJson = Json.Encode(json);
            }
            catch
            {
                retJson = "{}";
            }
            return retJson;
        }

        #region "GenericFileRecords"

        public static bool saveGenericFileRecords(string fileName, List<util.GenericFileRecord> data)
        {
            FileStream fs = null;
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                fs = new FileStream(fileName, FileMode.OpenOrCreate);
                formatter.Serialize(fs, data);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
            return true;
        }


        public static bool restoreGenericFileRecords(string fileName, ref List<util.GenericFileRecord> data)
        {
            FileStream fs = null;
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                fs = new FileStream(fileName, FileMode.Open);
                data = (List<util.GenericFileRecord>)formatter.Deserialize(fs);
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
            return true;
        }

        #endregion

        #region "HTTPRequestHeader covnersion"

        public static string header2JsonString(HTTPRequestHeaders oH)
        {
            string retString = "";
            try
            {
                dynamic json = Json.Decode("{}");
                json.HTTPMethod = oH.HTTPMethod;
                json.HTTPVersion = oH.HTTPVersion;
                json.RawPath = oH.RawPath;
                json.RequestPath = oH.RequestPath;
                json.UriScheme = oH.UriScheme;

                List<object> jHeader = new List<object>();
                int c = oH.Count();
                for (int i = 0; i < c; i++)
                {
                    HTTPHeaderItem hpi = oH.ElementAt(i);
                    Console.WriteLine("{0} : {1}", hpi.Name, hpi.Value);
                    dynamic jItem = Json.Decode("{}");
                    jItem.key = hpi.Name;
                    jItem.value = hpi.Value;
                    jHeader.Add(jItem);
                }
                json.header = new DynamicJsonArray(jHeader.ToArray());

                retString = Json.Encode(json);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return retString;
        }

        public static HTTPRequestHeaders headerFromJsonString(string jsonString)
        {
            HTTPRequestHeaders oH = new HTTPRequestHeaders();
            try
            {
                dynamic json = Json.Decode(jsonString);
                oH.HTTPMethod = json.HTTPMethod;
                oH.HTTPVersion = json.HTTPVersion;
                oH.RawPath = Encoding.UTF8.GetBytes(json.RequestPath);
                oH.RequestPath = json.RequestPath;
                oH.UriScheme = json.UriScheme;

                DynamicJsonArray jHeader = json.header;
                foreach (dynamic o in jHeader)
                {
                    oH[o.key] = o.value;
                }
            }
            catch (Exception)
            {
                oH = null;
            }
            return oH;
        }

        #endregion
    }
}
