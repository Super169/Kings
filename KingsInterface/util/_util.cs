using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface
{
    public partial class util
    {

        public static int getInt(dynamic o, string key, int defValue = -1)
        {
            if (o[key] == null) return defValue;
            return getInt(o[key], defValue);
        }

        public static int getInt(object o, int defValue = -1)
        {
            int retValue = defValue;
            try
            {
                retValue = Convert.ToInt32(o);
            }
            catch { }
            return retValue;
        }

        public static double getDouble(dynamic o, string key, double defValue = -1.0)
        {
            if (o[key] == null) return defValue;
            return getDouble(o[key], defValue);
        }

        public static double getDouble(object o, double defValue = -1.0)
        {
            double retValue = defValue;
            try
            {
                retValue = Convert.ToDouble(o);
            }
            catch { }
            return retValue;
        }

        public static bool getBool(dynamic o, string key, bool defValue = false)
        {
            if (o[key] == null) return defValue;
            return getBool(o[key], defValue);
        }

        public static bool getBool(object o, bool defValue = false)
        {
            bool retValue = defValue;
            try
            {
                retValue = Convert.ToBoolean(o);
            }
            catch { }
            return retValue;

        }


        public static DateTime getDateTime(dynamic o, string key)
        {
            if (o[key] == null) return DateTime.Now;
            return getDateTime(o[key], DateTime.Now);
        }

        public static DateTime getDateTime(dynamic o, string key, DateTime defValue)
        {
            if (o[key] == null) return defValue;
            return getDateTime(o[key], defValue);
        }

        public static DateTime getDateTime(object o)
        {
            return getDateTime(o, DateTime.Now);
        }

        public static DateTime getDateTime(object o, DateTime defValue)
        {
            DateTime retValue = defValue;
            try
            {
                retValue = Convert.ToDateTime(o);
            }
            catch { }
            return retValue;
        }

        public static string getString(dynamic o, string key, string defValue = null)
        {
            if (o[key] == null) return defValue;
            return getString(o[key], defValue);
        }

        public static string getString(object o, string defValue = null)
        {
            String retValue = defValue;
            try
            {
                retValue = Convert.ToString(o);
            }
            catch { }
            return retValue;
        }
        

        public static bool saveGenericFileRecords(string fileName, List<util.GenericFileRecord> data)
        {
            FileStream fs = null;
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                fs = new FileStream(fileName, FileMode.OpenOrCreate);
                formatter.Serialize(fs, data);
            }
            catch (Exception) {
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
            catch (Exception) {
                return false;
            }
            finally
            {
                if (fs != null) fs.Close();
            }
            return true;
        }

    }
}
