using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface
{
    public partial class util
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
                _ObjectData.Add(key, value);
            }

            public object getObject(string key)
            {
                if (_ObjectData.ContainsKey(key)) return _ObjectData[key];
                return null;
            }
        }
    }
}
