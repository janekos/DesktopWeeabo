using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Xml.Linq;

namespace DesktopWeeabo.Views
{
    public partial class SettingsView : UserControl
    {
        private DispatcherTimer savingTimer;
        private XDocument config = ItemHandler.ManageSettings();

        public SettingsView()
        {
            InitializeComponent();
            backUpCheckBox.IsChecked = Convert.ToBoolean(config.Root.Element("backup").Value);
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            SortByComboBoxTimer(ItemHandler.ManageSettings(backUpCheckBox.IsChecked.ToString()));
            ItemHandler.ManageSettings(backUpCheckBox.IsChecked.ToString());
        }

        private void Element_MouseEnter(object sender, MouseEventArgs e)
        {
            //futureproofing
            if (sender.GetType() == typeof(CheckBox))
            {
                CheckBox obj = sender as CheckBox;
                string name = obj.Name;
                switch (name)
                {
                    case "backUpCheckBox":
                        explanation.Text = backUpCheckBox.ToolTip.ToString();
                        break;
                }
            }
            explanationHeader.Visibility = Visibility.Visible;            
        }

        private void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            explanationHeader.Visibility = Visibility.Hidden;
            explanation.Text = "";
        }

        private void SortByComboBoxTimer(Action todo)
        {
            if (savingTimer == null)
            {
                savingTimer = new DispatcherTimer();
                savingTimer.Interval = TimeSpan.FromMilliseconds(500);
                savingTimer.Tick += (s, args) => {
                    savingTimer.Stop();
                    todo();
                };
            }
            savingTimer.Stop();
            savingTimer.Start();
        }
    }
}
