using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketData
{
    [Serializable]
    public class MyPacket
    {
        public DateTime actionTime { get; set; }
        public string monitorIp { get; set; }
        public byte[] data;

        public MyPacket(string ip, byte[] raw)
        {
            actionTime = DateTime.Now;
            monitorIp = ip;
            data = raw;
        }

    }
}
