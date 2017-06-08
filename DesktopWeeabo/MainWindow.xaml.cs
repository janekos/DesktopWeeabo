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

namespace DesktopWeeabo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

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

        private async void Search_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var username = "null";
                var password = "null";
                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                var textBox = sender as TextBox;
                var query = textBox.Text;
                Console.WriteLine(query);

                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
                client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                Stream data = client.OpenRead("https://myanimelist.net/api/anime/search.xml?q="+query);
                StreamReader reader = new StreamReader(data);
                string responseFromServer = reader.ReadToEnd();
                //Console.WriteLine(responseFromServer);
                data.Close();
                reader.Close();
            }
            catch{
            }
        }
    }
}
