using DesktopWeeabo;
using DesktopWeeabo.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopWeeabo.ViewModels
{
    class NavigationViewModel : INotifyPropertyChanged
    {
        public ICommand SearchCommand { get; set; }
        public ICommand ToWatchCommand { get; set; }
        public ICommand WatchedCommand { get; set; }
        public ICommand WatchingCommand { get; set; }
        public ICommand DroppedCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

        private object selectedViewModel;

        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set { selectedViewModel = value; OnPropertyChanged("SelectedViewModel"); }
        }


        public NavigationViewModel()
        {
            SearchCommand = new BaseCommand(OpenSearch);
            ToWatchCommand = new BaseCommand(OpenToWatch);
            WatchedCommand = new BaseCommand(OpenWatched);
            WatchingCommand = new BaseCommand(OpenWatching);
            DroppedCommand = new BaseCommand(OpenDropped);
            SettingsCommand = new BaseCommand(OpenSettings);
        }

        private void OpenSearch(object obj)
        {
            SelectedViewModel = new SearchViewModel();
        }
        private void OpenToWatch(object obj)
        {
            SelectedViewModel = new ToWatchViewModel();
        }
        private void OpenWatched(object obj)
        {
            SelectedViewModel = new WatchedViewModel();
        }
        private void OpenWatching(object obj)
        {
            SelectedViewModel = new WatchingViewModel();
        }
        private void OpenDropped(object obj)
        {
            SelectedViewModel = new DroppedViewModel();
        }
        private void OpenSettings(object obj)
        {
            SelectedViewModel = new SettingsViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

    public class BaseCommand : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<object> _method;
        public event EventHandler CanExecuteChanged;

        public BaseCommand(Action<object> method)
            : this(method, null)
        {
        }

        public BaseCommand(Action<object> method, Predicate<object> canExecute)
        {
            _method = method;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _method.Invoke(parameter);
        }
    }
}
