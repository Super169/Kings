using Fiddler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Web.Helpers;

namespace CheckResponse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int itemIdx = 1;

        public MainWindow()
        {
            InitializeComponent();
            FiddlerApplication.BeforeRequest += FiddlerApplication_BeforeRequest;
            FiddlerApplication.BeforeResponse += FiddlerApplication_BeforeResponse;
            SetButton();
        }

        private void FiddlerApplication_BeforeRequest(Session oSession)
        {
            oSession.bBufferResponse = true;
        }

        private void SetButton()
        {
            btnStart.IsEnabled = (!FiddlerApplication.IsStarted());
            btnStop.IsEnabled = (FiddlerApplication.IsStarted());
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (!FiddlerApplication.IsStarted())
            {
                updateInfo("Start FiddlerApplication");
                FiddlerApplication.Startup(8877, true, true);
                Thread.Sleep(1000);
            }
            SetButton();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (FiddlerApplication.IsStarted())
            {
                updateInfo("Stop FiddlerApplication");
                FiddlerApplication.Shutdown();
                Thread.Sleep(1000);
            }
            SetButton();
        }

        private string SystemTimePrefix()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss | ");
        }
        private void updateInfo(string info, bool addTime = true, bool resetText = false)
        {
            UpdateTextBox(txtInfo, info, addTime, resetText);
        }

        private void UpdateTextBox(TextBox tb, string info, bool addTime = true, bool resetText = false)
        {
            if (Dispatcher.FromThread(Thread.CurrentThread) == null)
            {
                // Time must be added here, otherwise, there will have longer delay
                if (addTime) info = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss | ") + info;

                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => UpdateTextBox(tb, info, false, resetText)));
                return;
            }
            if (resetText) tb.Text = "";
            if (addTime) tb.Text += SystemTimePrefix();
            tb.Text += info + "\n";
            tb.ScrollToEnd();
            txtNext.Text = itemIdx.ToString();
        }


        private void FiddlerApplication_BeforeResponse(Session oSession)
        {
            if (!oSession.uriContains("m.do")) return;
            string requestText = oSession.GetRequestBodyAsString();
            if (requestText.Contains("Shop.getTravelShopInfo"))
            {
                string items = "";
                string orgResponseText = oSession.GetResponseBodyAsString();
                string responseText = "";
                try
                {
                    dynamic json = Json.Decode(orgResponseText);

                    if (json["items"].GetType() == typeof(DynamicJsonArray))
                    {
                        DynamicJsonArray dja = json["items"];
                        foreach(dynamic o in dja)
                        {
                            items += itemIdx.ToString() + " : ";
                            o["config"] = itemIdx++;
                        }

                    }
                    updateInfo("BeforeResponse: " + items);
                    responseText = Json.Encode(json);
                }
                catch
                {
                    responseText = orgResponseText;
                }
                oSession.utilSetResponseBody(responseText);
                // oSession.utilSetResponseBody("{{\"items\":[{{\"config\":1,\"discount\":100,\"sold\":true}},{{\"config\":2,\"discount\":100,\"sold\":true}},{{\"config\":3,\"discount\":100,\"sold\":false}},{{\"config\":132,\"discount\":100,\"sold\":false}},{{\"config\":616,\"discount\":100,\"sold\":false}},{{\"config\":782,\"discount\":100,\"sold\":false}}],\"refreshSeconds\":15531,\"buyTimes\":0,\"todayTimes\":0,\"refreshTimes\":0}}");
            }
        }

        private void txtNext_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                int newValue = Convert.ToInt16(txtNext.Text);
                itemIdx = newValue;
            } catch
            {
                txtNext.Text = itemIdx.ToString();
            }
        }
    }
}
