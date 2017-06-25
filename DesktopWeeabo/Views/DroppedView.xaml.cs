using System;
using System.Windows.Controls;
using System.Xml.Linq;

namespace DesktopWeeabo
{
    public partial class DroppedView : UserControl
    {
        private RepeatingViewFunctions rvf = new RepeatingViewFunctions();
        private XDocument config;
        private bool wasItemChangedBySystem = false;

        public DroppedView()
        {
            InitializeComponent();
            config = ItemHandler.ManageSettings();
            wasItemChangedBySystem = true;
            for (var i = 0; i < 9; i++)
            {
                if ((orderByComboBox.Items[i] as ComboBoxItem).Content.ToString() == config.Root.Element("dropped").Element("orderby").Value)
                {
                    orderByComboBox.SelectedIndex = i;
                    break;
                }
            }
            descendingOrderByCheckBox.IsChecked = Convert.ToBoolean(config.Root.Element("dropped").Element("descendingorderby").Value);
            wasItemChangedBySystem = false;
            Loaded += delegate
            {
                Load_animes();
            };
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            rvf.TextBlockTimer(sender, listBox, 3, (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString(), descendingOrderByCheckBox.IsChecked.ToString());
        }

        private void Load_animes()
        {
            rvf.BuildListBoxItems(listBox, "", 3, config.Root.Element("dropped").Element("orderby").Value, Convert.ToBoolean(config.Root.Element("dropped").Element("descendingorderby").Value));
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByComboBoxTimer(sender, listBox, 3, descendingOrderByCheckBox.IsChecked.ToString());
                ItemHandler.ManageSettings("", "dropped", (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString(), "");
            }
        }

        private void CheckBoxChanged(object sender, EventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByDescendingTimer(sender, listBox, 3);
                ItemHandler.ManageSettings("", "dropped", "", descendingOrderByCheckBox.IsChecked.ToString());
            }
        }
    }
}
