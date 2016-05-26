using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MyUtil
{
    public class GFR
    {
        [Serializable]
        public class GenericFileRecord
        {
            public string key { get; set; }
            private Dictionary<string, object> _ObjectData = new Dictionary<string, object>();

            public GenericFileRecord(string key)
            {
                this.key = key;
            }

            public void saveObject(string key, object value)
            {
                if (value != null) _ObjectData.Add(key, value);
            }

            public object getObject(string key)
            {
                if (_ObjectData.ContainsKey(key)) return _ObjectData[key];
                return null;
            }
        }


        public static bool saveGFR(string fileName, List<GenericFileRecord> data)
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


        public static bool restoreGFR(string fileName, ref List<GenericFileRecord> data)
        {
            FileStream fs = null;
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                fs = new FileStream(fileName, FileMode.Open);
                data = (List<GenericFileRecord>) formatter.Deserialize(fs);
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


    }
}
