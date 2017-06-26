using DesktopWeeabo.ViewModels;
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
            Search.IsTabStop = true;
        }

        private void SelectedButton(object sender, RoutedEventArgs e)
        {
            ToWatch.IsTabStop = false;
            Watching.IsTabStop = false;
            Dropped.IsTabStop = false;
            Search.IsTabStop = false;
            Watched.IsTabStop = false;
            Settings.IsTabStop = false;
            var selected = sender as Button;
            selected.IsTabStop = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ItemHandler.CreateBackUp();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("DesktopWeeabo\n\n" +
                "Overview: A program I made for managing my anime viewing, because I couldn't be bothered to go on some website to do that.\n\n" +
                "Author: Janek Kossinski\n" +
                "Email: janek.kossinski@gmail.com\n" +
                "Project github: https://github.com/janekos/DesktopWeeabo \n" +
                "Version: 1.0.0\n" +
                "Special thanks: Stackoverflow (yeah..)\n" +
                "2017 Summer","About", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
