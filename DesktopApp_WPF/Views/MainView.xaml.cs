using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace DesktopApp_WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {

            InitializeComponent();
        }
        private bool _isButtonLogPressed = false;


        public bool IsButtonLogPressed
        {
            get
            {
                return _isButtonLogPressed;
            }

            set
            {
                _isButtonLogPressed = value;

            }
        }


        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }
        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            IsButtonLogPressed = true;

            Window mainWindow1 = Application.Current.MainWindow;
            mainWindow1.Close();
            Window loginWindow = new LoginView();
            this.Close();
            Window mainWindow2 = Application.Current.MainWindow;
            Window mainWindow = mainWindow2;
            mainWindow.ShowDialog();


        }

    }
}
