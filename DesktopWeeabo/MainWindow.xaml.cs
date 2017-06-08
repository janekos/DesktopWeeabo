using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;

namespace DesktopWeeabo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //System.Windows.Threading.DispatcherTimer typingTimer;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new NavigationViewModel();
        }
        /*
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("esimene");
        }

        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("teine");
        }

        private void MenuItem3_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("kolmas");
        }

        private void Search_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (typingTimer == null)
            {
                
                typingTimer = new DispatcherTimer();
                typingTimer.Interval = TimeSpan.FromMilliseconds(500);
                

                typingTimer.Tick += (s, args) => {
                    typingTimer.Stop();
                    if (tb.Text.Length > 0)
                    {
                        BuildItems(tb.Text);
                    }
                    else
                    {
                        listBox.Items.Clear();
                    }
                };
            }
            typingTimer.Stop();
            typingTimer.Tag = (sender as TextBox).Text;
            typingTimer.Start();           
            
        }

        private void BuildItems(string query)
        {
            listBox.Items.Clear();
            string entries = ItemSearchHandler.MakeSearch(query);

            if (entries.Length > 0)
            {
                XDocument response = XDocument.Parse(entries);
                
                foreach (var e in response.Descendants("entry"))
                {
                    Console.WriteLine(e.Element("id"));
                    Console.WriteLine(e.Element("title"));
                    /*foreach (var a in e.Descendants())
                    {
                        Console.WriteLine(a);
                    }
                    listBox.Items.Add(e);
                }
            }
            else
            {
                listBox.Items.Add("No results");
            }

            
        }*/
    }
}
