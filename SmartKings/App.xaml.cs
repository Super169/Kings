using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SmartKings
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private bool _isExit;

        public string winTitle = "SmartKings v" + Assembly.GetExecutingAssembly().GetName().Version;

        Mutex mutex;

        #region "For call previous instance"

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct MyStruct
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string Message;
        }

        internal const int WM_COPYDATA = 0x004A;
        [StructLayout(LayoutKind.Sequential)]
        internal struct COPYDATASTRUCT
        {
            public IntPtr dwData;       // Specifies data to be passed
            public int cbData;          // Specifies the data size in bytes
            public IntPtr lpData;       // Pointer to data to be passed
        }
        [SuppressUnmanagedCodeSecurity]
        internal class NativeMethod
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr SendMessage(IntPtr hWnd, int Msg,
                IntPtr wParam, ref COPYDATASTRUCT lParam);


            [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        }

        #endregion


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            bool createdNew;
            mutex = new Mutex(true, "{SmartKings Mutex}", out createdNew);
            if (!createdNew)
            {
                mutex = null;

                IntPtr hTargetWnd = NativeMethod.FindWindow(null, winTitle);
                if (hTargetWnd == IntPtr.Zero)
                {

                    MessageBox.Show("Windows not found");
                    Application.Current.Shutdown();
                    return;
                }

                MyStruct myStruct;
                myStruct.Message = Constants.MSG_WAKEUP;
                int myStructSize = Marshal.SizeOf(myStruct);
                IntPtr pMyStruct = Marshal.AllocHGlobal(myStructSize);
                try
                {
                    Marshal.StructureToPtr(myStruct, pMyStruct, true);

                    COPYDATASTRUCT cds = new COPYDATASTRUCT();
                    cds.cbData = myStructSize;
                    cds.lpData = pMyStruct;
                    // NativeMethod.SendMessage(hTargetWnd, WM_COPYDATA, new IntPtr(), ref cds);
                    NativeMethod.SendMessage(hTargetWnd, WM_COPYDATA, IntPtr.Zero, ref cds);
                    // MessageBox.Show("Message sent");


                    int result = Marshal.GetLastWin32Error();
                    if (result != 0)
                    {
                        // MessageBox.Show(string.Format("Result not zero: {0}", result));
                    }
                }
                finally
                {
                    Marshal.FreeHGlobal(pMyStruct);
                }

                Application.Current.Shutdown();
                return;
            }
            
            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;

            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.DoubleClick += (s, args) => ShowMainWindow();
            _notifyIcon.Icon = SmartKings.Properties.Resources.SmartKingsIcon;
            _notifyIcon.Visible = true;

            CreateContextMenu();
            ShowMainWindow();
        }

        private void CreateContextMenu()
        {
            _notifyIcon.ContextMenuStrip =
              new System.Windows.Forms.ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("MainWindow...").Click += (s, e) => ShowMainWindow();
            _notifyIcon.ContextMenuStrip.Items.Add("-");
            _notifyIcon.ContextMenuStrip.Items.Add("Exit").Click += (s, e) => ExitApplication();
        }

        private void ExitApplication()
        {
            _isExit = true;
            ((MainWindow) MainWindow).WindowPreClose();
            MainWindow.Close();
            _notifyIcon.Dispose();
            _notifyIcon = null;
        }

        public void ShowMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
                MainWindow.Activate();
            }
            else
            {
                MainWindow.Show();
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_isExit)
            {
                e.Cancel = true;
                MainWindow.Hide(); // A hidden window can be shown again, a closed one not
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (mutex != null)
            {
                mutex.ReleaseMutex();
                mutex.Dispose();
            }
        }
    }
}
