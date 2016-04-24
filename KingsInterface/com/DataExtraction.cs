﻿using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace KingsInterface
{
    public static partial class com
    {

        public static string CleanUpResponse(string responseText, int minLength = 7)
        {
            if (responseText == null) return null;

            string jsonString = null;

            string[] data = responseText.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < data.Length; i++)
            {
                if (jsonString == null)
                {
                    // Not yet started, JSON string will start will "{", ignore all others
                    if (data[i].StartsWith("{")) jsonString = data[i];
                }
                else
                {
                    // study studying the behavious, it seesm that the dummy row has only few characters (hexdecimal value?), but not exactly the length
                    // The default 7 is just a rough estimate based on previous catpured packets
                    if (data[i].Length >= minLength) jsonString += data[i];
                }
            }
            return jsonString;
        }

        public static string GetResponseText(Session oS)
        {
            string responseText = Encoding.UTF8.GetString(oS.responseBodyBytes);
            return Microsoft.VisualBasic.Strings.StrConv(responseText, Microsoft.VisualBasic.VbStrConv.TraditionalChinese, 0);
        }

        public static dynamic getJsonFromResponse(string responseText, bool cleanUp = true)
        {
            dynamic json = null;
            try
            {
                string jsonString = (cleanUp ? CleanUpResponse(responseText) : responseText);
                json = Json.Decode(jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting response:\n{0}", ex.Message);
            }
            return json;
        }

        public static dynamic getJsonFromResponse(Session oS, bool cleanUp = true)
        {
            string responseText = GetResponseText(oS);
            return getJsonFromResponse(responseText, cleanUp);
        }

    }

}
