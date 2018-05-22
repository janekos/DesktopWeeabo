using System;
using System.Threading;
using System.Windows.Controls;

namespace DesktopWeeabo.Views
{
    public partial class ToWatchView : UserControl
    {
        private bool wasItemChangedBySystem = false;
        RepeatingViewFunctions rvf = new RepeatingViewFunctions();

        public ToWatchView()
        {
            InitializeComponent();
            wasItemChangedBySystem = true;
            for (var i = 0; i < 10; i++)
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
            rvf.TextBlockTimer(sender, listBox, entryCount, 0, (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString(), descendingOrderByCheckBox.IsChecked.ToString());
        }

        private void Load_animes()
        {
            rvf.BuildListBoxItems(listBox, entryCount, "", 0, ConfigClass.ToWatch.OrderBy, ConfigClass.ToWatch.Descending);
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByComboBoxTimer(sender, listBox, entryCount, 0, descendingOrderByCheckBox.IsChecked.ToString());
                ConfigClass.ToWatch.OrderBy = (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            }
        }

        private void CheckBoxChanged(object sender, EventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByDescendingTimer(sender, listBox, entryCount, 0);
                ConfigClass.ToWatch.Descending = descendingOrderByCheckBox.IsChecked ?? false;
            }
        }
    }
}
