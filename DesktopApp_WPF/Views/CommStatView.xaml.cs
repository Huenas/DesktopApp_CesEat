using DotNetEnv;
using System;
using System.Windows;
using System.Windows.Controls;
namespace DesktopApp_WPF.Views
{
    /// <summary>
    /// Logique d'interaction pour CommStatView.xaml
    /// </summary>
    public partial class CommStatView : UserControl
    {
        public CommStatView()
        {
            InitializeComponent();
        }

        private void btnViewAll_Click(object sender, RoutedEventArgs e)
        {
            Env.TraversePath().Load();
            string UrlCommercialStat = Environment.GetEnvironmentVariable("UrlCommercialStat");
            System.Diagnostics.Process.Start(UrlCommercialStat);
        }
    }
}
