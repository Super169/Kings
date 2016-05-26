using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.monitor
{
    class KingsPacket
    {
        public const char PACKET_SPLITTER = '|';
        public const string KINGS_DATA_KEYWORD = ".icantw.com";

        public bool valid { get; }
        public string data { get; }
        private int headLength;
        private int totalLength;
        public IPAddress srcIPAddress { get; }
        public IPAddress desIPAddress { get; }
        public int srcPort { get; }
        public int desPort { get; }

        public KingsPacket(byte[] raw)
        {
            valid = false;

            if (raw == null) return;

            // In genral, a Kings Packet should have more than 32 bytes, so over 64 including headers
            // More enlarge the value for better filtering
            if (raw.Length < 64) return;

            // Check for valid header length
            if ((raw[0] & 0x0F) < 5) return;

            // KingsPacket must be TCP protocol
            if (raw[9] != 6) return;

            headLength = (raw[0] & 0x0F) * 4;
            totalLength = raw[2] * 256 + raw[3];

            srcIPAddress = new IPAddress(BitConverter.ToUInt32(raw, 12));
            desIPAddress = new IPAddress(BitConverter.ToUInt32(raw, 16));

            srcPort = raw[headLength] * 256 + raw[headLength + 1];
            desPort = raw[headLength + 2] * 256 + raw[headLength + 3];

            // Filter port 80 for http (8080 for proxy), this filter may cause problem if the proxy is using other port
            if ((srcPort != 80) && (srcPort != 8080) && (desPort != 80) && (desPort != 8080)) return;

            StringBuilder sb = new StringBuilder();

            // For TCP packet, data started at 20 bytes after end of header 
            for (int i = this.headLength + 20; i < this.totalLength; i++)
            {
                if (raw[i] > 31 && raw[i] < 128)
                    sb.Append((char)raw[i]);
                else
                    sb.Append(PACKET_SPLITTER);
            }
            data = sb.ToString();

            // don't check icantw.com/m.do as it cannot be found for direct connection
            if (!data.Contains(KINGS_DATA_KEYWORD)) return;
            valid = true;
        }
    }

}
