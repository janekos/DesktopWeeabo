using System;
using System.Windows.Controls;

namespace DesktopWeeabo.Views
{
    public partial class DroppedView : UserControl
    {
        private bool wasItemChangedBySystem = false;
        private RepeatingViewFunctions rvf = new RepeatingViewFunctions();

        public DroppedView()
        {
            InitializeComponent();
            wasItemChangedBySystem = true;
            for (var i = 0; i < 9; i++)
            {
                if ((orderByComboBox.Items[i] as ComboBoxItem).Content.ToString() == ConfigClass.Dropped.OrderBy)
                {
                    orderByComboBox.SelectedIndex = i;
                    break;
                }
            }
            descendingOrderByCheckBox.IsChecked = ConfigClass.Dropped.Descending;
            wasItemChangedBySystem = false;
            Loaded += delegate
            {
                Load_animes();
            };
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            rvf.TextBlockTimer(sender, listBox, entryCount, 3, (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString(), descendingOrderByCheckBox.IsChecked.ToString());
        }

        private void Load_animes()
        {
            rvf.BuildListBoxItems(listBox, entryCount, "", 3, ConfigClass.Dropped.OrderBy, ConfigClass.Dropped.Descending);
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByComboBoxTimer(sender, listBox, entryCount, 3, descendingOrderByCheckBox.IsChecked.ToString());
                ConfigClass.Dropped.OrderBy = (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            }
        }

        private void CheckBoxChanged(object sender, EventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByDescendingTimer(sender, listBox, entryCount, 3);
                ConfigClass.Dropped.Descending = descendingOrderByCheckBox.IsChecked ?? false;
            }
        }
    }
}
