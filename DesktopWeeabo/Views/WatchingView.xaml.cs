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

namespace DesktopWeeabo
{
    /// <summary>
    /// Interaction logic for WatchingView.xaml
    /// </summary>
    public partial class WatchingView : UserControl
    {
        public WatchingView()
        {
            InitializeComponent();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            RepeatingViewFunctions.TextBlockTimer(sender, listBox);
        }

        private void Load_animes(object sender, RoutedEventArgs e)
        {
            RepeatingViewFunctions.BuildListBoxItems(listBox, "", 2);
        }
    }
}
