using System;
using System.Windows.Controls;
using System.Xml.Linq;

namespace DesktopWeeabo
{
    public partial class WatchingView : UserControl
    {
        private RepeatingViewFunctions rvf = new RepeatingViewFunctions();
        private XDocument config;
        private bool wasItemChangedBySystem = false;

        public WatchingView()
        {
            InitializeComponent();
            config = ItemHandler.ManageSettings();
            wasItemChangedBySystem = true;
            for (var i = 0; i < 9; i++)
            {
                if ((orderByComboBox.Items[i] as ComboBoxItem).Content.ToString() == config.Root.Element("watching").Element("orderby").Value)
                {
                    orderByComboBox.SelectedIndex = i;
                    break;
                }
            }
            descendingOrderByCheckBox.IsChecked = Convert.ToBoolean(config.Root.Element("watching").Element("descendingorderby").Value);
            wasItemChangedBySystem = false;
            Loaded += delegate
            {
                Load_animes();
            };
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            rvf.TextBlockTimer(sender, listBox, 2, (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString(), descendingOrderByCheckBox.IsChecked.ToString());
        }

        private void Load_animes()
        {
            rvf.BuildListBoxItems(listBox, "", 2, config.Root.Element("watching").Element("orderby").Value, Convert.ToBoolean(config.Root.Element("watching").Element("descendingorderby").Value));
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByComboBoxTimer(sender, listBox, 2, descendingOrderByCheckBox.IsChecked.ToString());
                ItemHandler.ManageSettings("", "watching", (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString(), "");
            }
        }

        private void CheckBoxChanged(object sender, EventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByDescendingTimer(sender, listBox, 2);
                ItemHandler.ManageSettings("", "watching", "", descendingOrderByCheckBox.IsChecked.ToString());
            }
        }
    }
}
