﻿using System;
using System.Windows.Controls;

namespace DesktopWeeabo.Views
{
    public partial class WatchingView : UserControl
    {
        private bool wasItemChangedBySystem = false;
        private RepeatingViewFunctions rvf = new RepeatingViewFunctions();

        public WatchingView()
        {
            InitializeComponent();
            wasItemChangedBySystem = true;
            for (var i = 0; i < 9; i++)
            {
                if ((orderByComboBox.Items[i] as ComboBoxItem).Content.ToString() == ConfigClass.Watching.OrderBy)
                {
                    orderByComboBox.SelectedIndex = i;
                    break;
                }
            }
            descendingOrderByCheckBox.IsChecked = ConfigClass.Watching.Descending;
            wasItemChangedBySystem = false;
            Loaded += delegate
            {
                Load_animes();
            };
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            rvf.TextBlockTimer(sender, listBox, entryCount, 2, (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString(), descendingOrderByCheckBox.IsChecked.ToString());
        }

        private void Load_animes()
        {
            rvf.BuildListBoxItems(listBox, entryCount, "", 2, ConfigClass.Watching.OrderBy, ConfigClass.Watching.Descending);
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByComboBoxTimer(sender, listBox, entryCount, 2, descendingOrderByCheckBox.IsChecked.ToString());
                ConfigClass.Watching.OrderBy = (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            }
        }

        private void CheckBoxChanged(object sender, EventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByDescendingTimer(sender, listBox, entryCount, 2);
                ConfigClass.Watching.Descending = descendingOrderByCheckBox.IsChecked ?? false;
            }
        }
    }
}
