using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace DesktopWeeabo
{
    /// <summary>
    /// Interaction logic for ToWatchView.xaml
    /// </summary>
    public partial class ToWatchView : System.Windows.Controls.UserControl
    {
        public ToWatchView()
        {
            InitializeComponent();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            RepeatingViewFunctions.TextBlockTimer(sender, listBox);
        }

        private void Load_animes(object sender, RoutedEventArgs e)
        {
            RepeatingViewFunctions.BuildListBoxItems(listBox, "", 0);
        }

        //todo item sorting by different criterias
    }
}
