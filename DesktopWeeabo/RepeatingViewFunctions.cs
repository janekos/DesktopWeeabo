using DesktopWeeabo.CustomControls;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;

namespace DesktopWeeabo
{
    public class RepeatingViewFunctions
    {
        private System.Windows.Threading.DispatcherTimer typingTimer, ComboBoxTimer, CheckBoxTimer;
        private string orderByMem;

        public void TextBlockTimer(object sender, ListBox listBox, TextBlock entryCount, int view, string sortBy, string descending)
        {
            var tb = sender as TextBox;
            if (typingTimer == null)
            {
                typingTimer = new DispatcherTimer();
                typingTimer.Interval = TimeSpan.FromMilliseconds(500);
                typingTimer.Tick += (s, args) => {
                    typingTimer.Stop();
                    if (tb.Text.Length > 0) { BuildListBoxItems(listBox, entryCount, tb.Text.ToLower(), view, sortBy, Convert.ToBoolean(descending)); }
                    else { BuildListBoxItems(listBox, entryCount, "", view, sortBy, Convert.ToBoolean(descending)); }                    
                };
            }
            typingTimer.Stop();
            typingTimer.Start();
        }

        public void SortByComboBoxTimer(object sender, ListBox listBox, TextBlock entryCount, int view, string descending)
        {
            if (ComboBoxTimer == null)
            {
                ComboBoxTimer = new DispatcherTimer();
                ComboBoxTimer.Interval = TimeSpan.FromMilliseconds(500);
                ComboBoxTimer.Tick += (s, args) => {
                    ComboBoxTimer.Stop();
                    string sortby = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();
                    BuildListBoxItems(listBox, entryCount, "", view, sortby, Convert.ToBoolean(descending));
                };
            }
            ComboBoxTimer.Stop();
            ComboBoxTimer.Start();
        }

        public void SortByDescendingTimer(object sender, ListBox listBox, TextBlock entryCount, int view)
        {
            if (CheckBoxTimer == null)
            {
                CheckBoxTimer = new DispatcherTimer();
                CheckBoxTimer.Interval = TimeSpan.FromMilliseconds(500);
                CheckBoxTimer.Tick += (s, args) => {
                    CheckBoxTimer.Stop();
                    bool descending = (sender as CheckBox).IsChecked ?? false;
                    BuildListBoxItems(listBox, entryCount, "", view, "", descending);
                };
            }
            CheckBoxTimer.Stop();
            CheckBoxTimer.Start();
        }

        public async void BuildListBoxItems(ListBox listBox, TextBlock entryCount, string query, int view, string orderBy, bool descendingOrder)
        {
            listBox.Items.Clear();
            listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "Loading"));
            entryCount.Text = "0";
            string[] viewingStatuses = { "To Watch", "Watched", "Watching", "Dropped" };
            orderByMem = orderBy.Length > 0 ? orderBy : orderByMem;
            XDocument localEntries = await ItemHandler.MakeLocalSearch(query, viewingStatuses[view]);
            if (localEntries != null)
            {
                XDocument entries = SortEntries(listBox, localEntries, orderByMem, descendingOrder);
                if (entries == null) { return; }
                if (entries.Descendants("entry").Any())
                {
                    listBox.Items.Clear();
                    int count = 0;
                    foreach (var e in entries.Descendants("entry"))
                    {
                        await listBox.Dispatcher.BeginInvoke(new Action(delegate ()
                         {
                             listBox.Items.Add(new ListBoxItemForAnime(e, listBox, view, true));
                         }));
                        count++;
                    }
                    entryCount.Text = count.ToString();
                }
                else if (!entries.Descendants("entry").Any() && query.Length == 0)
                {
                    listBox.Items.Clear();
                    listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "You have not listed any animes as '" + viewingStatuses[view] + "'."));
                    entryCount.Text = "0";
                }
                else if (!entries.Descendants("entry").Any() && query.Length > 0)
                {
                    listBox.Items.Clear();
                    listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "No animes with viewing status '" + viewingStatuses[view] + "' are matching: '" + query + "'."));
                    entryCount.Text = "0";
                }
            }
            else
            {
                listBox.Items.Clear();
                listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "Something is wrong with the 'MainEntries.xml' file. Consider switching in a back up."));
                entryCount.Text = "0";
            }
        }

        private XDocument SortEntries(ListBox lb, XDocument entries, string orderBy, bool descendingOrder)
        {
            IOrderedEnumerable<XElement> sorted = null;
            bool orderByNumber = false;
            if (orderBy.Equals("Episodes") || orderBy.Equals("Score") || orderBy.Equals("Personal score") || orderBy.Equals("Watch priority")) { orderByNumber = true; }

            if (descendingOrder)
            {
                if (orderByNumber){ sorted = entries.Descendants("entry").OrderByDescending(p => TryParsing(p.Element(orderBy.ToLower().Replace(" ", "_")).Value)); }
                else{ sorted = entries.Descendants("entry").OrderByDescending(p => p.Element(orderBy.ToLower().Replace(" ", "_")).Value); }
            }
            else
            {
                if (orderByNumber){ sorted = entries.Descendants("entry").OrderBy( p => TryParsing(p.Element(orderBy.ToLower().Replace(" ", "_")).Value)); }
                else{ sorted = entries.Descendants("entry").OrderBy(p => p.Element(orderBy.ToLower().Replace(" ", "_")).Value); }
            }
            XDocument doc = new XDocument(new XElement("anime", sorted));
            return doc;
        }

        private double TryParsing(string item)
        {
            bool isDouble = double.TryParse(item, out double tmp);
            if (isDouble) { return tmp; }
            else { return 0; }
        }
    }
}
