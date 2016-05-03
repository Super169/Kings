using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KingsInterface.data
{
    public enum AiReturnCode
    {
        NORMAL, NO_ACTIVITY, COMPLETED, ERROR
    }


    public class ArcheryInfo
    {
        public bool success { get; set; }
        public AiReturnCode returnCode { get; set; }
        public string msg { get; set; }
        public string body { get; set; }
        public string requestBody { get; set; }
        public string responseBody { get; set; }
        public int tRing { get; set; }
        public int arr { get; set; }
        public int wind { get; set; }
        public int goX { get; set; }
        public int goY { get; set; }
        public int atX { get; set; }
        public int atY { get; set; }
        public int ring { get; set; }
        public int nWind { get; set; }
    }
}
