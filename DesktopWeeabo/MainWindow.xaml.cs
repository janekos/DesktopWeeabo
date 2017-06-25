using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;

namespace DesktopWeeabo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ItemHandler.ManageSettings();
            DataContext = new NavigationViewModel();
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            Search.Background = Brushes.LightBlue;
        }

        private void SelectedButton(object sender, RoutedEventArgs e)
        {
            ToWatch.Background = Brushes.White;
            Watching.Background = Brushes.White;
            Dropped.Background = Brushes.White;
            Search.Background = Brushes.White;
            Watched.Background = Brushes.White;
            Settings.Background = Brushes.White;
            About.Background = Brushes.White;
            var selected = sender as Button;
            selected.Background = Brushes.LightBlue;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ItemHandler.CreateBackUp();
        }
    }
}
