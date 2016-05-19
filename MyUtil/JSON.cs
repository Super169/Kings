using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace MyUtil
{
    public class JSON
    {

        public static dynamic Empty()
        {
            return Json.Decode("{}");
        }

        #region "Numeric Data"

        public static byte getByte(dynamic json, string key, byte defValue = 0)
        {
            if (json == null) return defValue;
            return getByte(json[key], defValue);
        }

        public static byte getByte(object json, byte defValue = 0)
        {
            if (json == null) return defValue;
            byte retValue = defValue;
            try
            {
                retValue = Convert.ToByte(json);
            }
            catch { }
            return retValue;
        }

        public static int getInt(dynamic json, string key, int defValue = -1)
        {
            if (json == null) return defValue;
            return getInt(json[key], defValue);
        }

        public static int getInt(object json, int defValue = -1)
        {
            if (json == null) return defValue;
            int retValue = defValue;
            try
            {
                retValue = Convert.ToInt32(json);
            }
            catch { }
            return retValue;
        }

        public static long getLong(dynamic json, string key, long defValue = -1)
        {
            if (json == null) return defValue;
            return getLong(json[key], defValue);
        }

        public static long getLong(object json, long defValue = -1)
        {
            if (json == null) return defValue;
            long retValue = defValue;
            try
            {
                retValue = Convert.ToInt64(json);
            }
            catch { }
            return retValue;
        }

        public static double getDouble(dynamic json, string key, double defValue = -1.0)
        {
            if (json == null) return defValue;
            return getDouble(json[key], defValue);
        }

        public static double getDouble(object json, double defValue = -1.0)
        {
            if (json == null) return defValue;
            double retValue = defValue;
            try
            {
                retValue = Convert.ToDouble(json);
            }
            catch { }
            return retValue;
        }

        #endregion

        #region "Boolean data"

        public static bool getBool(dynamic json, string key, bool defValue = false)
        {
            if (json == null) return defValue;
            return getBool(json[key], defValue);
        }

        public static bool getBool(object json, bool defValue = false)
        {
            if (json == null) return defValue;
            bool retValue = defValue;
            try
            {
                retValue = Convert.ToBoolean(json);
            }
            catch { }
            return retValue;

        }

        #endregion

        #region "Date / Time data"

        public static DateTime getDateTime(dynamic json, string key)
        {
            return getDateTime(json, key, DateTime.Now);
        }
        public static DateTime getDateTime(dynamic json, string key, DateTime defValue)
        {
            if (json == null) return defValue;
            return getDateTime(json[key], defValue);
        }

        public static DateTime getDateTime(object json)
        {
            return getDateTime(json, DateTime.Now);
        }
        public static DateTime getDateTime(object json, DateTime defValue)
        {
            if (json == null) return defValue;
            DateTime retValue = defValue;
            try
            {
                retValue = Convert.ToDateTime(json);
            }
            catch { }
            return retValue;
        }

        public static DateTime? getDateTimeX(dynamic json, string key, DateTime? defValue = null)
        {
            if (json == null) return defValue;
            return getDateTimeX(json[key], defValue);
        }

        public static DateTime? getDateTimeX(object json, DateTime? defValue = null)
        {
            if (json == null) return defValue;
            DateTime? retValue = defValue;
            try
            {
                retValue = Convert.ToDateTime(json);
            }
            catch { }
            return retValue;
        }

        public static TimeSpan getTimeSpan(dynamic json, string key)
        {
            return getTimeSpan(json, key, TimeSpan.Zero);
        }
        public static TimeSpan getTimeSpan(dynamic json, string key, TimeSpan defValue)
        {
            if (json == null) return defValue;
            return getTimeSpan(json[key], defValue);
        }

        public static TimeSpan getTimeSpan(dynamic json)
        {
            return getTimeSpan(json, TimeSpan.Zero);
        }
        public static TimeSpan getTimeSpan(dynamic json, TimeSpan defValue)
        {
            if (json == null) return defValue;
            if (json.GetType() == typeof(TimeSpan)) return new TimeSpan(json);
            if (json.GetType() == typeof(TimeSpan?)) return new TimeSpan(json);
            if (json["Ticks"] == null) return defValue;
            long Ticks = getLong(json, "Ticks", 0);
            return new TimeSpan(Ticks);
        }

        public static TimeSpan? getTimeSpanX(dynamic json, string key, TimeSpan? defValue = null)
        {
            if (json == null) return defValue;
            return getTimeSpanX(json[key], defValue);
        }

        public static TimeSpan? getTimeSpanX(dynamic json, TimeSpan? defValue = null)
        {
            if (json == null) return defValue;
            if (json.GetType() == typeof(TimeSpan?)) return new TimeSpan?(json);
            if (json.GetType() == typeof(TimeSpan)) return new TimeSpan?(json);
            if (json["Ticks"] == null) return defValue;
            long Ticks = getLong(json, "Ticks", 0);
            return new TimeSpan?(new TimeSpan(Ticks));
        }


        #endregion


        #region "String data"

        public static string getString(dynamic json, string key, string defValue)
        {
            if (json == null) return defValue;
            return getString(json[key], defValue);
        }

        public static string getString(object json, string defValue = null)
        {
            if (json == null) return defValue;
            String retValue = defValue;
            try
            {
                retValue = Convert.ToString(json);
            }
            catch { }
            return retValue;
        }

        #endregion

        #region "Get Data List"

        public static List<byte> getByteList(dynamic json, string key, byte defValue = 0)
        {
            if (json == null) return new List<byte>();
            return getByteList(json[key], defValue);
        }
        public static List<byte> getByteList(dynamic json, byte defValue = 0)
        {
            if (json == null) return new List<byte>();
            if (json.GetType() == typeof(List<byte>)) return (List<byte>)json;
            List<byte> retList = new List<byte>();
            if (json.GetType() == typeof(DynamicJsonArray))
            {
                foreach (dynamic o in json)
                {
                    retList.Add(getByte(o, defValue));
                }
            }
            return retList;
        }

        public static List<int> getIntList(dynamic json, string key, int defValue = -1)
        {
            if (json == null) return new List<int>();
            return getIntList(json[key], defValue);
        }
        public static List<int> getIntList(dynamic json, int defValue = -1)
        {
            if (json == null) return new List<int>();
            if (json.GetType() == typeof(List<int>)) return (List<int>)json;
            List<int> retList = new List<int>();
            if (json.GetType() == typeof(DynamicJsonArray))
            {
                foreach (dynamic o in json)
                {
                    retList.Add(getInt(o, defValue));
                }
            }
            return retList;
        }

        public static List<long> getLongList(dynamic json, string key, long defValue = -1)
        {
            if (json == null) return new List<long>();
            return getLongList(json[key], defValue);
        }
        public static List<long> getLongList(dynamic json, long defValue = -1)
        {
            if (json == null) return new List<long>();
            if (json.GetType() == typeof(List<long>)) return (List<long>)json;
            List<long> retList = new List<long>();
            if (json.GetType() == typeof(DynamicJsonArray))
            {
                foreach (dynamic o in json)
                {
                    retList.Add(getLong(o, defValue));
                }
            }
            return retList;
        }

        public static List<bool> getBoolList(dynamic json, string key, bool defValue = false)
        {
            if (json == null) return new List<bool>();
            return getBoolList(json[key], defValue);
        }
        public static List<bool> getBoolList(dynamic json, bool defValue = false)
        {
            if (json == null) return new List<bool>();
            if (json.GetType() == typeof(List<bool>)) return (List<bool>)json;
            List<bool> retList = new List<bool>();
            if (json.GetType() == typeof(DynamicJsonArray))
            {
                foreach (dynamic o in json)
                {
                    retList.Add(getBool(o, defValue));
                }
            }
            return retList;
        }

        public static List<DateTime> getDateTimeList(dynamic json, string key)
        {
            return getDateTimeList(json, key, DateTime.Now);
        }
        public static List<DateTime> getDateTimeList(dynamic json, string key, DateTime defValue)
        {
            if (json == null) return new List<DateTime>();
            return getDateTimeList(json[key], defValue);
        }
        public static List<DateTime> getDateTimeList(dynamic json)
        {
            return getDateTimeList(json, DateTime.Now);
        }
        public static List<DateTime> getDateTimeList(dynamic json, DateTime defValue)
        {
            if (json == null) return new List<DateTime>();
            if (json.GetType() == typeof(List<DateTime>)) return (List<DateTime>)json;
            List<DateTime> retList = new List<DateTime>();
            if (json.GetType() == typeof(List<DateTime?>))
            {
                foreach (DateTime? o in json)
                {
                    retList.Add(o.GetValueOrDefault(defValue));
                }
                return retList;
            }
            if (json.GetType() == typeof(DynamicJsonArray))
            {
                foreach (dynamic o in json)
                {
                    retList.Add(getDateTime(o, defValue));
                }
            }
            return retList;
        }

        public static List<TimeSpan> getTimeSpanList(dynamic json, string key)
        {
            return getTimeSpanList(json, key, TimeSpan.Zero);
        }
        public static List<TimeSpan> getTimeSpanList(dynamic json, string key, TimeSpan defValue)
        {
            if (json == null) return new List<TimeSpan>();
            return getTimeSpanList(json[key], defValue);
        }
        public static List<TimeSpan> getTimeSpanList(dynamic json)
        {
            return getTimeSpanList(json, TimeSpan.Zero);
        }
        public static List<TimeSpan> getTimeSpanList(dynamic json, TimeSpan defValue)
        {
            if (json == null) return new List<TimeSpan>();
            if (json.GetType() == typeof(List<TimeSpan>)) return (List<TimeSpan>)json;
            List<TimeSpan> retList = new List<TimeSpan>();
            if (json.GetType() == typeof(List<TimeSpan?>))
            {
                foreach (TimeSpan? o in json)
                {
                    retList.Add(o.GetValueOrDefault(defValue));
                }
                return retList;
            }
            if (json.GetType() == typeof(DynamicJsonArray))
            {
                foreach (dynamic o in json)
                {
                    retList.Add(getTimeSpan(o, defValue));
                }
            }
            return retList;
        }

        public static List<string> getStringList(dynamic json, string key)
        {
            if (json == null) return new List<string>();
            return getStringList(json[key]);
        }
        public static List<string> getStringList(dynamic json)
        {
            if (json == null) return new List<string>();
            if (json.GetType() == typeof(List<string>)) return (List<string>)json;
            List<string> retList = new List<string>();
            if (json.GetType() == typeof(DynamicJsonArray))
            {
                foreach (dynamic o in json)
                {
                    retList.Add(getString(o, null));
                }
            }
            return retList;
        }

        #endregion

        #region "Array Data -> converted from List Data"

        public static int[] getIntArray(dynamic json, string key, int defValue = -1)
        {
            if (json == null) return null;
            return getIntArray(json[key], defValue);
        }
        public static int[] getIntArray(dynamic json, int defValue = -1)
        {
            if (json == null) return null;

            List<int> intList = getIntList(json, defValue);
            return intList.ToArray();
        }


        public static string[] getStringArray(dynamic json, string key)
        {
            if (json == null) return null;
            return getStringArray(json[key]);
        }
        public static string[] getStringArray(dynamic json)
        {
            if (json == null) return null;

            List<string> strList = getStringList(json);
            return strList.ToArray();
        }

        #endregion



    }
}
