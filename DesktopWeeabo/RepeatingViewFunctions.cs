using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;

namespace DesktopWeeabo
{
    class RepeatingViewFunctions
    {
        private static System.Windows.Threading.DispatcherTimer typingTimer;

        public static void TextBlockTimer(object sender, ListBox listBox)
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
                        BuildListBoxItems(listBox, tb.Text.ToLower(), 0);
                    }
                    else
                    {
                        BuildListBoxItems(listBox, "", 0);
                    }
                };
            }
            typingTimer.Stop();
            typingTimer.Tag = tb.Text;
            typingTimer.Start();
        }

        public static void BuildListBoxItems(ListBox listBox, string query, int view)
        {
            listBox.Items.Clear();
            listBox.Items.Add(PreBuiltControlElements.NotifitacationMessagesForListBox(listBox, "Loading"));

            string[] viewingStatuses = { "To Watch", "Watched", "Watching", "Dropped" };
            XDocument entries = ItemHandler.MakeLocalSearch(query, viewingStatuses[view]);
            
            //System.Diagnostics.Debug.WriteLine(e);

            if (entries.Descendants("entry").Any())
            {
                listBox.Items.Clear();
                foreach (var e in entries.Descendants("entry"))
                {
                    listBox.Items.Add(PreBuiltControlElements.ListBoxItemForAnime(e, listBox, view, true));
                }
            }
            else if (!entries.Descendants("entry").Any())
            {
                listBox.Items.Clear();
                listBox.Items.Add(PreBuiltControlElements.NotifitacationMessagesForListBox(listBox, "You have not listed any animes as '"+ viewingStatuses[view] + "'."));
            }
        }
    }
}
