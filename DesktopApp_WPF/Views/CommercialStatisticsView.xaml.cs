using System;
using System.Windows.Controls;

namespace DesktopApp_WPF.Views
{
    /// <summary>
    /// Logique d'interaction pour CommercialStatisticsView.xaml
    /// </summary>
    public partial class CommercialStatisticsView : UserControl
    {
        public CommercialStatisticsView()
        {
            InitializeComponent();
            webBrowser.Navigate(new Uri("https://charts.mongodb.com/charts-project-0-wwtaj/public/dashboards/8cb49e86-4f86-4c76-a9e2-c0dbcc204365"));

        }


    }
}
