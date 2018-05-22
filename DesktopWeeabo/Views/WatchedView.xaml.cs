﻿using System;
using System.Windows.Controls;

namespace DesktopWeeabo.Views
{
    public partial class WatchedView : UserControl
    {
        private bool wasItemChangedBySystem = false;
        private RepeatingViewFunctions rvf = new RepeatingViewFunctions();

        public WatchedView()
        {
            InitializeComponent();
            wasItemChangedBySystem = true;
            for (var i = 0; i < 10; i++)
            {
                if ((orderByComboBox.Items[i] as ComboBoxItem).Content.ToString() == ConfigClass.Watched.OrderBy)
                {
                    orderByComboBox.SelectedIndex = i;
                    break;
                }
            }
            descendingOrderByCheckBox.IsChecked = ConfigClass.Watched.Descending;
            wasItemChangedBySystem = false;
            Loaded += delegate
            {
                Load_animes();
            };
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            rvf.TextBlockTimer(sender, listBox, entryCount, 1, (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString(), descendingOrderByCheckBox.IsChecked.ToString());
        }

        private void Load_animes()
        {
            rvf.BuildListBoxItems(listBox, entryCount, "", 1, ConfigClass.Watched.OrderBy, ConfigClass.Watched.Descending);
        }

        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByComboBoxTimer(sender, listBox, entryCount, 1, descendingOrderByCheckBox.IsChecked.ToString());
                ConfigClass.Watched.OrderBy = (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString();
            }
        }

        private void CheckBoxChanged(object sender, EventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                rvf.SortByDescendingTimer(sender, listBox, entryCount, 1);
                ConfigClass.Watched.Descending = descendingOrderByCheckBox.IsChecked ?? false;
            }
        }
    }
}
