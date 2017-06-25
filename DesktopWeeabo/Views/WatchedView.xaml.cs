using System;
using System.Windows.Controls;
using System.Xml.Linq;

namespace DesktopWeeabo
{
    public partial class WatchedView : UserControl
    {
        private RepeatingViewFunctions rvf = new RepeatingViewFunctions();
        private XDocument config;
        private bool wasItemChangedBySystem = false;

        public WatchedView()
        {
            InitializeComponent();
            config = ItemHandler.ManageSettings();
            wasItemChangedBySystem = true;
            for (var i = 0; i < 10; i++)
            {
                if ((orderByComboBox.Items[i] as ComboBoxItem).Content.ToString() == config.Root.Element("watched").Element("orderby").Value)
                {
                    orderByComboBox.SelectedIndex = i;
                    break;
                }
            }
            descendingOrderByCheckBox.IsChecked = Convert.ToBoolean(config.Root.Element("watched").Element("descendingorderby").Value);
            wasItemChangedBySystem = false;
            Loaded += delegate
            {
                Load_animes();
            };
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            rvf.TextBlockTimer(sender, listBox, 1, (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString(), descendingOrderByCheckBox.IsChecked.ToString());
        }

        private void Load_animes()
        {
            rvf.BuildListBoxItems(listBox, "", 1, config.Root.Element("watched").Element("orderby").Value, Convert.ToBoolean(config.Root.Element("watched").Element("descendingorderby").Value));
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByComboBoxTimer(sender, listBox, 1, descendingOrderByCheckBox.IsChecked.ToString());
                ItemHandler.ManageSettings("", "watched", (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString(), "");
            }
        }

        private void CheckBoxChanged(object sender, EventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByDescendingTimer(sender, listBox, 1);
                ItemHandler.ManageSettings("", "watched", "", descendingOrderByCheckBox.IsChecked.ToString());
            }
        }
    }
}
