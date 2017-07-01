using DesktopWeeabo.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DesktopWeeabo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            ConfigClass.SetVariables();
            if (!ConfigClass.IsProgramKill)
            {
                ProgramColorChangingClass.ChangeColors();
                InitializeComponent();
                ItemHandler.ManageSettings();
                DataContext = new NavigationViewModel();
                System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
                Search.IsTabStop = true;
            }
            else
            {
                InitializeComponent();
                theProgramIsKill.Visibility = Visibility.Visible;
            }         
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
            if (!ConfigClass.IsProgramKill)
            {
                ItemHandler.CreateBackUp();
                ConfigClass.SaveVariables();
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("DesktopWeeabo\n\n" +
                "Overview: A program I made for managing my anime viewing, because I couldn't be bothered to go on some website to do that.\n\n" +
                "Author: Janek Kossinski\n" +
                "Project github: https://github.com/janekos/DesktopWeeabo (report any bugs there)\n" +
                "Version: 1.0.0\n" +
                "Special thanks: Stackoverflow (yeah..)\n" +
                "2017 Summer","About", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
