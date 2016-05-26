using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KingsLib.monitor
{
    class KingsSocketMonitor
    {

        private const int SECURITY_BUILTIN_DOMAIN_RID = 0x20;
        private const int DOMAIN_ALIAS_RID_ADMINS = 0x220;

        private const int IOC_VENDOR = 0x18000000;
        private const int IOC_IN = -2147483648; //0x80000000;
        private const int SIO_RCVALL = IOC_IN | IOC_VENDOR | 1;
        private const int BUF_SIZE = 1024 * 1024;

        private Socket monitor_Socket;
        private IPAddress ipAddress;
        private byte[] buffer;

        public event NewPacketEventHandler newPacketEventHandler;
        public delegate void NewPacketEventHandler(KingsSocketMonitor monitor, KingsPacket p);


        public KingsSocketMonitor(IPAddress ip)
        {
            this.ipAddress = ip;
            this.buffer = new byte[BUF_SIZE];
        }

        ~KingsSocketMonitor()
        {
            Stop();
        }

        public string ip()
        {
            return this.ipAddress.ToString();
        }

        public bool Start()
        {
            bool socketReady = false;
            if (monitor_Socket == null)
            {
                try
                {

                    if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                    {
                        monitor_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, System.Net.Sockets.ProtocolType.IP);
                    }
                    else
                    {
                        monitor_Socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Raw, System.Net.Sockets.ProtocolType.IP);
                    }
                    monitor_Socket.Bind(new IPEndPoint(ipAddress, 0));
                    monitor_Socket.IOControl(SIO_RCVALL, BitConverter.GetBytes((int)1), null);
                    monitor_Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), null);
                    socketReady = true;
#if CONSOLE_DEBUG
                    Console.WriteLine("Start monitor on " + ip());
#endif

                }
                catch (Exception e)
                {
                    if (monitor_Socket != null)
                    {
                        monitor_Socket.Close();
                    }
                    monitor_Socket = null;
                    Console.WriteLine("Fail starting monitor on " + ip() + "\n" + e.ToString());
                }
            }
            return socketReady;
        }

        public void Stop()
        {
            if (monitor_Socket != null)
            {
#if CONSOLE_DEBUG
                Console.WriteLine("Stop monitor on " + ip());
#endif
                monitor_Socket.Close();
                monitor_Socket = null;
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                int len = monitor_Socket.EndReceive(ar);
                if (monitor_Socket != null)
                {
                    byte[] receivedBuffer = new byte[len];
                    Array.Copy(buffer, 0, receivedBuffer, 0, len);
                    try
                    {
                        KingsPacket packet = new KingsPacket(receivedBuffer);
                        if (packet.valid) OnNewPacket(packet);
                    }
                    catch (ArgumentNullException ane)
                    {
                        Console.WriteLine(ane.ToString());
                    }
                    catch (ArgumentException ae)
                    {
                        Console.WriteLine(ae.ToString());
                    }
                }
                monitor_Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(this.OnReceive), null);
            }
            catch
            {
                Stop();
            }
        }

        protected void OnNewPacket(KingsPacket p)
        {
            if (newPacketEventHandler != null)
            {
                newPacketEventHandler(this, p);
            }
        }

    }

}
