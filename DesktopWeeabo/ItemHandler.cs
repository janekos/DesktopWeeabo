using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
            CheckAndCreateDirAndFile();

            XDocument doc = XDocument.Load(path + "/MainEntries.xml");
            XDocument result = new XDocument(new XElement("anime"));
            int queryLen = query.Length;
            int viewingStatusLen = viewingStatus.Length;
            
            foreach (var e in doc.Descendants("entry"))
            {
                if (viewingStatusLen != 0)
                {                
                    if (e.Element("viewingstatus").Value.Equals(viewingStatus))
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

        public static void UpdateSaveFile(XElement itemToModify, string viewingStatus, string userReview = "", string userScore="", string userDropReason="", string currEpisode="", bool remove = false)
        {
            CheckAndCreateDirAndFile();

            XDocument doc = XDocument.Load(path + "/MainEntries.xml");
            Boolean entryExists = false;
            
            foreach (var e in doc.Descendants("entry"))
            {
                if (int.Parse(e.Element("id").Value) == int.Parse(itemToModify.Element("id").Value))
                {
                    if (remove)
                    {
                        e.Remove();
                    }
                    else
                    {
                        e.Element("viewingstatus").Value = viewingStatus;
                        e.Element("review").Value = userReview.Length > 0 ? userReview: "";
                        e.Element("personal_score").Value = userScore.Length > 0 ? userScore : "";
                        e.Element("dropreason").Value = userDropReason.Length > 0 ? userDropReason : "";
                        e.Element("currepisode").Value = currEpisode.Length > 0 ? currEpisode : "";
                    }                    
                    entryExists = true;

                    break;
                }
            }

            if (!entryExists)
            {
                itemToModify.Add(
                    new XElement("viewingstatus", viewingStatus),
                    new XElement("review", userReview),
                    new XElement("personal_score", userScore),
                    new XElement("dropreason", userDropReason),
                    new XElement("currepisode", currEpisode)
                );
                doc.Root.Add(itemToModify);
            }

            doc.Save(path + "/MainEntries.xml");
        }        

        public static void CreateBackUp()
        {
            if (File.Exists(path + "/MainEntries.xml"))
            {
                if (!Directory.Exists(path+"/BackUp")) { Directory.CreateDirectory(path+ "/BackUp"); }
                File.Copy(path + "/MainEntries.xml", path + "/BackUp/MainEntries" + DateTime.Now.ToString("_dd_MM_yyyy") + ".xml", true);
            }
        }

        public static XDocument ManageSettings(string backUp = "", string view = "", string orderBy = "", string descendingOrderBy = "")
        {
            CheckAndCreateDirAndFile();

            XDocument settings = XDocument.Load(path + "/Config.xml");
            if (!settings.Root.Elements().Any())
            {
                settings.Root.Add(
                        new XElement("backup", "true"),
                        new XElement("towatch",
                            new XElement("orderby", "Title"),
                            new XElement("descendingorderby", "false")
                        ),
                        new XElement("watched",
                            new XElement("orderby", "Title"),
                            new XElement("descendingorderby", "false")
                        ),
                        new XElement("watching",
                            new XElement("orderby", "Title"),
                            new XElement("descendingorderby", "false")
                        ),
                        new XElement("dropped",
                            new XElement("orderby", "Title"),
                            new XElement("descendingorderby", "false")
                        ),
                        new XElement("search",
                            new XElement("orderby", "Title"),
                            new XElement("descendingorderby", "false")
                        )
                    );
                settings.Save(path + "/Config.xml");
            }
            if (backUp.Length > 0) { settings.Root.Element("backup").Value = backUp; }
            if (orderBy.Length > 0) { settings.Root.Element(view).Element("orderby").Value = orderBy; }
            if (descendingOrderBy.Length > 0) { settings.Root.Element(view).Element("descendingorderby").Value = descendingOrderBy; }
            if (backUp.Length > 0  || orderBy.Length > 0 || descendingOrderBy.Length > 0) { settings.Save(path + "/Config.xml"); }

            return settings;
        }

        private static void CheckAndCreateDirAndFile()
        {
            if (!Directory.Exists(path)){ Directory.CreateDirectory(path); }
            if (!File.Exists(path + "/MainEntries.xml")){ CreateSaveFile("/MainEntries.xml", "anime"); }
            if (!File.Exists(path + "/Config.xml")){ CreateSaveFile("/Config.xml", "config"); }
        }

        private static void CreateSaveFile(string fname, string rootElementName)
        {
            XDocument doc = new XDocument(new XElement(rootElementName));
            doc.Save(path + fname);
        }
    }
}
