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
    class ItemHandler
    {

        private static string username = "maltest123";
        private static string password = "gfdg987g7df8dfgSAHD";
        private static string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
        private static string path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)),"DesktopWeeabo");
        
        public async static Task<string> MakeWebSearch(string query) 
        {
            var url = "https://myanimelist.net/api/anime/search.xml?q=" + query;
            try
            {
                WebClient client = new WebClient();
                client.Headers[HttpRequestHeader.Authorization] = "Basic " + credentials;            
                var data = await client.OpenReadTaskAsync(new Uri(url));
                StreamReader reader = new StreamReader(data);
                string response = await reader.ReadToEndAsync();
                reader.Close();
                return response;
            }
            catch
            {
                return "Exception was thrown";
            }
        }

        public async static Task<XDocument> MakeLocalSearch(string query, string viewingStatus)
        {
            return new XDocument();
            //todo
        }

        public async static void UpdateSaveFile(XElement itemToModify)
        {
            //todo
        }
    }
}
