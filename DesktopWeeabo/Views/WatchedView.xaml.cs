﻿using System;
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

namespace DesktopWeeabo
{
    /// <summary>
    /// Interaction logic for WatchedView.xaml
    /// </summary>
    public partial class WatchedView : UserControl
    {
        private RepeatingViewFunctions rvf = new RepeatingViewFunctions();

        public WatchedView()
        {
            InitializeComponent();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            rvf.TextBlockTimer(sender, listBox, 1);
        }

        private void Load_animes(object sender, RoutedEventArgs e)
        {
            rvf.BuildListBoxItems(listBox, "", 1);
        }
    }
}
