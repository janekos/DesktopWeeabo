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

        public static XDocument MakeLocalSearch(string query, string viewingStatus = "")
        {
            XDocument doc = XDocument.Load(path + "/entries.xml");
            XDocument result = new XDocument(new XElement("anime"));
            int queryLen = query.Length;
            int viewingStatusLen = viewingStatus.Length;
            
            foreach (var e in doc.Descendants("entry"))
            {
                if (viewingStatusLen != 0)
                {                
                    if (e.Element("ViewingStatus").Value.Equals(viewingStatus))
                    {
                        if (queryLen != 0)
                        {
                            if (e.Element("title").Value.ToLower().Contains(query) || e.Element("english").Value.ToLower().Contains(query) || e.Element("synonyms").Value.ToLower().Contains(query))
                            {                        
                                result.Root.Add(e);
                            }
                        }
                        else
                        {
                            result.Root.Add(e);
                        }
                    }
                }
                else
                {
                    if (e.Element("title").Value.ToLower().Contains(query) || e.Element("english").Value.ToLower().Contains(query) || e.Element("synonyms").Value.ToLower().Contains(query))
                    {
                        result.Root.Add(e);
                    }
                }
            }

            return result;
        }

        public static void UpdateSaveFile(XElement itemToModify, string viewingStatus)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                CreateSaveFile();
            }

            XDocument doc = XDocument.Load(path + "/entries.xml");
            Boolean entryExists = false;
            
            foreach (var e in doc.Descendants("entry"))
            {
                if (int.Parse(e.Element("id").Value) == int.Parse(itemToModify.Element("id").Value))
                {
                    e.Element("ViewingStatus").Value = viewingStatus;
                    entryExists = true;
                    break;
                }
            }

            if (!entryExists)
            {
                itemToModify.Add(new XElement("ViewingStatus", viewingStatus));
                doc.Root.Add(itemToModify);
            }

            doc.Save(path + "/entries.xml");
        }

        public static void CreateSaveFile()
        {
            XDocument doc = new XDocument( new XElement("anime"));
            doc.Save(path + "/entries.xml");
        }
    }
}
