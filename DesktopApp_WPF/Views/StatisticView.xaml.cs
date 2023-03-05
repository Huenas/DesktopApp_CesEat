using System.Windows;
using System.Windows.Controls;


namespace DesktopApp_WPF.Views
{
    /// <summary>
    /// Logique d'interaction pour StatisticView.xaml
    /// </summary>
    public partial class StatisticView : UserControl
    {
        public StatisticView()
        {
            InitializeComponent();
        }

        private void btnViewAll_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://portainer.localhost/#!/2/docker/containers");
        }

    }
}
