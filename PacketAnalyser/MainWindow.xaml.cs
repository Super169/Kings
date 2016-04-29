using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using PacketData;
using System.Net;
using System.Reflection;

namespace PacketAnalyer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<MyPacket> packets;
        String fileName = "MyPackets.dat";
        ObservableCollection<HeaderInfo> headers = new ObservableCollection<HeaderInfo>();

        enum ProtocolType
        {
            GGP = 3,
            ICMP = 1,
            IDP = 22,
            IGMP = 2,
            IP = 4,
            ND = 77,
            PUP = 12,
            TCP = 6,
            UDP = 17,
            OTHERS = -1
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Title = "PacketAnalyser  v" + Assembly.GetExecutingAssembly().GetName().Version;

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Data file not found");
                return;
            }

            FileStream fs;
            BinaryFormatter formatter = new BinaryFormatter();

            try
            {

                fs = new FileStream(fileName, FileMode.Open);
                packets = (List<MyPacket>)formatter.Deserialize(fs);
                fs.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Failto read data\n" + ex.Message);
                return;
            }

            lvPackets.ItemsSource = packets;
            lblPacketCount.Content = packets.Count;
            lvPackets.SelectedIndex = -1;
            lvHeader.ItemsSource = headers;

        }


        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            List<MyPacket> filterPackets = packets;
            packets = new List<MyPacket>();
            for (int i = 0; i < filterPackets.Count; i++)
            {
                MyPacket p = filterPackets.ElementAt(i);
                string s = Encoding.UTF8.GetString(p.data);
                if (s.Contains(".icantw.com"))
                {
                    packets.Add(p);
                }
            }
            lvPackets.ItemsSource = packets;
            lvPackets.SelectedIndex = -1;
            headers.Clear();
            txtHexDump.Text = "";
            txtStrDump.Text = "";
        }

        private void lvPackets_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MyPacket p = (MyPacket)lvPackets.SelectedItem;
            if (p == null) return;
            ShowHeader(p);
            ShowHexDump(p);
            ShowStrDump(p);
        }


        private void ShowHeader(MyPacket p)
        {
            headers.Clear();
            byte[] raw = p.data;
            IPAddress src_IPAddress;
            IPAddress des_IPAddress;
            int src_Port;
            int des_Port;

            if (raw == null)
            {
                headers.Add(new HeaderInfo() { key = "Error", value = "No data" });
                return;
            }
            headers.Add(new HeaderInfo() { key = "packetSize", value = raw.Length.ToString() });

            if (raw.Length < 20)
            {
                headers.Add(new HeaderInfo() { key = "Error", value = "Lenght < 20" });
                return;
            }

            int dataLength = raw[2] * 256 + raw[3];
            headers.Add(new HeaderInfo() { key = "dataLength", value = dataLength.ToString() });

            if (dataLength != raw.Length)
            {
                headers.Add(new HeaderInfo() { key = "Error", value = "dataLength not match" });
                return;
            }

            int headLength = (raw[0] & 0x0F) * 4;
            headers.Add(new HeaderInfo() { key = "headLegth", value = headLength.ToString() });
            if ((raw[0] & 0x0F) < 5)
            {
                headers.Add(new HeaderInfo() { key = "Error", value = "Wrong size" });
                return;
            }

            ProtocolType protocolType;
            ///get the type of the packet;
            if (Enum.IsDefined(typeof(ProtocolType), (int)raw[9]))
                protocolType = (ProtocolType)raw[9];
            else
                protocolType = ProtocolType.OTHERS;
            headers.Add(new HeaderInfo() { key = "Protocol", value = protocolType.ToString() });

            src_IPAddress = new IPAddress(BitConverter.ToUInt32(raw, 12));
            des_IPAddress = new IPAddress(BitConverter.ToUInt32(raw, 16));

            if (protocolType == ProtocolType.TCP || protocolType == ProtocolType.UDP)
            {
                src_Port = raw[headLength] * 256 + raw[headLength + 1];
                des_Port = raw[headLength + 2] * 256 + raw[headLength + 3];
                if (protocolType == ProtocolType.TCP)
                {
                    headLength += 20;
                }
                else if (protocolType == ProtocolType.UDP)
                {
                    headLength += 8;
                }
                headers.Add(new HeaderInfo() { key = "Src. IP", value = src_IPAddress.ToString() });
                headers.Add(new HeaderInfo() { key = "Src. Port", value = src_Port.ToString() });
                headers.Add(new HeaderInfo() { key = "Des. IP", value = des_IPAddress.ToString() });
                headers.Add(new HeaderInfo() { key = "Des. Port", value = des_Port.ToString() });
            }
            else
            {
                headers.Add(new HeaderInfo() { key = "Src. IP", value = src_IPAddress.ToString() });
                headers.Add(new HeaderInfo() { key = "Des. IP", value = des_IPAddress.ToString() });
            }

        }

        private void ShowHexDump(MyPacket p)
        {
            txtHexDump.Text = "";
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < p.data.Length; i += 32)
            {
                for (int j = i; j < p.data.Length && j < i + 32; j++)
                {
                    sb.Append(p.data[j].ToString("X2") + " ");
                    if (j % 8 == 7) sb.Append(" ");
                    if (j % 16 == 15) sb.Append(" ");
                }
                sb.Append("\n");
            }
            txtHexDump.Text = sb.ToString();

        }

        private void ShowStrDump(MyPacket p)
        {
            txtStrDump.Text = "";
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < p.data.Length; i++)
            {
                if (p.data[i] > 31 && p.data[i] < 128)
                {
                    sb.Append((char)p.data[i]);
                }
                else
                {
                    sb.Append('.');
                }
            }
            txtStrDump.Text = sb.ToString();

            if (txtStrDump.Text.Contains(".icantw.com")) {
                headers.Add(new HeaderInfo() { key = "Kings Info", value = "Found packet" });
            }

        }

        private class HeaderInfo
        {
            public string key { get; set; }
            public string value { get; set; }
        }

    }
}
