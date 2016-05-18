﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace KingsBackground
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int count = 0;
        System.Timers.Timer myTimer = new System.Timers.Timer(1000);

        public MainWindow()
        {
            InitializeComponent();
            myTimer.Interval = 1000;
            myTimer.Elapsed += new System.Timers.ElapsedEventHandler(UpdateCount);
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            btnGo.IsEnabled = false;
            ((App)Application.Current).SetNotifyText("Timer running");
            myTimer.Enabled = true;
        }

        private void UpdateCount(object sender, ElapsedEventArgs e)
        {
            UpdateUI((++count).ToString());
        }

        private void UpdateUI(string info)
        {
            Dispatcher currDispatcher = Dispatcher.FromThread(Thread.CurrentThread);
            if (currDispatcher == null)
            {
                Application.Current.Dispatcher.BeginInvoke(
                  System.Windows.Threading.DispatcherPriority.Normal,
                  (Action)(() => UpdateUI(info)));
                return;
            }

            lblCount.Content = info;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
            source.AddHook(new HwndSourceHook(WndProc));
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            System.Windows.Forms.Message m = System.Windows.Forms.Message.Create(hwnd, msg, wParam, lParam);
            if (m.Msg == WM_COPYDATA)
            {
                // MessageBox.Show("Message received");
                // Get the COPYDATASTRUCT struct from lParam.
                COPYDATASTRUCT cds = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));

                // If the size matches
                if (cds.cbData == Marshal.SizeOf(typeof(MyStruct)))
                {
                    // Marshal the data from the unmanaged memory block to a 
                    // MyStruct managed struct.
                    MyStruct myStruct = (MyStruct)Marshal.PtrToStructure(cds.lpData,
                        typeof(MyStruct));

                    // Display the MyStruct data members.
                    if (myStruct.Message == "Show Up")
                    {
                        // MessageBox.Show("Windows show");
                        if (this.IsVisible)
                        {
                            if (this.WindowState == WindowState.Minimized)
                            {
                                this.WindowState = WindowState.Normal;
                            }
                            this.Activate();
                        }
                        else
                        {
                            this.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Message not matched");
                    }
                }
            }
            return IntPtr.Zero;
        }

        internal const int WM_COPYDATA = 0x004A;

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct MyStruct
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Message;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct COPYDATASTRUCT
        {
            public IntPtr dwData;       // Specifies data to be passed
            public int cbData;          // Specifies the data size in bytes
            public IntPtr lpData;       // Pointer to data to be passed
        }


    }
}
