using KingsInterface;
using KingsInterface.data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace KingsTester
{
    public partial class MainWindow : Window
    {
        private void goHarvestAll()
        {
            foreach (GameAccount oGA in gameAccounts)
            {
                action.goManorHarvestAll(oGA, UpdateInfoHandler);
            }
        }
    }
}
