using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace DesktopWeeabo
{
    class ItemSearchHandler
    {

        private static string username = "maltest123";
        private static string password = "gfdg987g7df8dfgSAHD";
        private static string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
        
        public static string MakeSearch(string query) 
        {
            var url = "https://myanimelist.net/api/anime/search.xml?q=" + query;

            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;
            var data = client.OpenRead(new Uri(url));
            StreamReader reader = new StreamReader(data);
            string response = reader.ReadToEnd();
            reader.Close();
            return response;
        }        
    }
}
