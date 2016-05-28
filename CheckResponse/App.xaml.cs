using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CheckResponse
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (Fiddler.FiddlerApplication.IsStarted())
            {
                Fiddler.FiddlerApplication.Shutdown();
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
