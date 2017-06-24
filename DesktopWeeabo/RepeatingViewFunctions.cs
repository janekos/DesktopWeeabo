using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml.Linq;

namespace DesktopWeeabo
{
    public class RepeatingViewFunctions
    {
        private System.Windows.Threading.DispatcherTimer typingTimer;

        public void TextBlockTimer(object sender, ListBox listBox, int view)
        {
            var tb = sender as TextBox;
            if (typingTimer == null)
            {

                typingTimer = new DispatcherTimer();
                typingTimer.Interval = TimeSpan.FromMilliseconds(500);


                typingTimer.Tick += (s, args) => {
                    typingTimer.Stop();
                    if (tb.Text.Length > 0){ BuildListBoxItems(listBox, tb.Text.ToLower(), view); }
                    else { BuildListBoxItems(listBox, "", view); }
                };
            }
            typingTimer.Stop();
            typingTimer.Start();
        }

        public void BuildListBoxItems(ListBox listBox, string query, int view)
        {
            listBox.Items.Clear();
            listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "Loading"));

            string[] viewingStatuses = { "To Watch", "Watched", "Watching", "Dropped" };
            XDocument entries = ItemHandler.MakeLocalSearch(query, viewingStatuses[view]);

            if (entries.Descendants("entry").Any())
            {
                listBox.Items.Clear();
                foreach (var e in entries.Descendants("entry"))
                {
                    listBox.Items.Add(new ListBoxItemForAnime(e, listBox, view, true));
                }
            }
            else if (!entries.Descendants("entry").Any() && query.Length == 0)
            {
                listBox.Items.Clear();
                listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "You have not listed any animes as '"+ viewingStatuses[view] + "'."));
            }
            else if (!entries.Descendants("entry").Any() && query.Length > 0)
            {
                listBox.Items.Clear();
                listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "No animes with viewing status '" + viewingStatuses[view] + "' are matching: '" + query+"'."));
            }
        }
    }
}
