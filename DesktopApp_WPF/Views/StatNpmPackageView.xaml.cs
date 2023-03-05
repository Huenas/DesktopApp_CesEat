using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DesktopApp_WPF.Views
{
    /// <summary>
    /// Logique d'interaction pour StatNpmPackageView.xaml
    /// </summary>
    public partial class StatNpmPackageView : UserControl
    {
        public StatNpmPackageView()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "base-controller-cesiweb",
                    Values = new ChartValues<double> { 12, 9, 5, 142 ,139 }
                },
                new LineSeries
                {
                    Title = "leaflet_component_cesieat",
                    Values = new ChartValues<double> { 0, 0, 0, 0 , 115 }
                }
            };

            Labels = new[] { "01 Jan", "05 jan", "10 jan", "15 jan", "20 jan" };
            YFormatter = value => value.ToString("C");

            //modifying the series collection will animate and update the chart
            SeriesCollection.Add(new LineSeries
            {
                Values = new ChartValues<double> { 50, 30, 20, 15 },
                LineSmoothness = 0 //straight lines, 1 really smooth lines
            });

            //modifying any series values will also animate and update the chart
            SeriesCollection[2].Values.Add(5d);

            DataContext = this;
        }
        private void btnViewAll_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.npmjs.com/package/base-controller-cesiweb");
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}
