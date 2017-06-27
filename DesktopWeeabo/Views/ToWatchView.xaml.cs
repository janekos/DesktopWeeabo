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
using System.Xml.Linq;

namespace DesktopWeeabo.Views
{
    public partial class ToWatchView : System.Windows.Controls.UserControl
    {
        private bool wasItemChangedBySystem = false;
        RepeatingViewFunctions rvf = new RepeatingViewFunctions();

        public ToWatchView()
        {
            InitializeComponent();
            wasItemChangedBySystem = true;
            for (var i = 0; i < 9; i++)
            {
                if ((orderByComboBox.Items[i] as ComboBoxItem).Content.ToString() == ConfigClass.ToWatch.OrderBy)
                {
                    orderByComboBox.SelectedIndex = i;
                    break;
                }
            }
            descendingOrderByCheckBox.IsChecked = ConfigClass.ToWatch.Descending;
            wasItemChangedBySystem = false;
            Loaded += delegate
            {
                Load_animes();
            };
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            rvf.TextBlockTimer(sender, listBox, 0, (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString(), descendingOrderByCheckBox.IsChecked.ToString());
        }

        private void Load_animes()
        {
            rvf.BuildListBoxItems(listBox, "", 0, ConfigClass.ToWatch.OrderBy, ConfigClass.ToWatch.Descending);
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByComboBoxTimer(sender, listBox, 0, descendingOrderByCheckBox.IsChecked.ToString());
                ConfigClass.ToWatch.OrderBy = (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            }
        }

        private void CheckBoxChanged(object sender, EventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByDescendingTimer(sender, listBox, 0);
                ConfigClass.ToWatch.Descending = descendingOrderByCheckBox.IsChecked ?? false;
            }
        }
    }
}
