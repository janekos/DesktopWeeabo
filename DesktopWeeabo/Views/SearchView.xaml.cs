using DesktopWeeabo.CustomControls;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;

namespace DesktopWeeabo.Views
{
    public partial class SearchView : UserControl
    {

        private DispatcherTimer typingTimer, ComboBoxTimer, CheckBoxTimer;
        private string queryMem = "";
        private string orderByMem;
        private bool wasItemChangedBySystem = false;

        public SearchView()
        {
            if (!ConfigClass.IsProgramKill)
            {
                InitializeComponent();
                wasItemChangedBySystem = true;
                for (var i = 0; i < 10; i++)
                {
                    if ((orderByComboBox.Items[i] as ComboBoxItem).Content.ToString() == ConfigClass.Search.OrderBy)
                    {
                        orderByComboBox.SelectedIndex = i;
                        break;
                    }
                }
                if (orderByComboBox.SelectedIndex == 0) { descendingOrderByCheckBox.IsEnabled = false; }
                descendingOrderByCheckBox.IsChecked = ConfigClass.Search.Descending;
                wasItemChangedBySystem = false;

                Loaded += delegate
                {
                    listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "Try searching for something."));
                };
            }            
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
                    if (tb.Text.Length > 0) { BuildListBoxItems(tb.Text, (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString(), descendingOrderByCheckBox.IsChecked ?? false); }
                    else
                    {
                        listBox.Items.Clear();
                        listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "Try searching for something."));
                        queryMem = "";
                    }
                };
            }
            typingTimer.Stop();
            typingTimer.Start();
        }

        public void SortByComboBoxTimer(object sender, EventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                if (ComboBoxTimer == null)
                {
                    ComboBoxTimer = new DispatcherTimer();
                    ComboBoxTimer.Interval = TimeSpan.FromMilliseconds(500);
                    ComboBoxTimer.Tick += (s, args) => {
                        ComboBoxTimer.Stop();
                        string sortby = ((sender as ComboBox).SelectedItem as ComboBoxItem).Content.ToString();
                        if (sortby.Equals("No sort")) { descendingOrderByCheckBox.IsEnabled = false; }
                        else { descendingOrderByCheckBox.IsEnabled = true; }
                        BuildListBoxItems("", sortby, descendingOrderByCheckBox.IsChecked ?? false);
                        ConfigClass.Search.OrderBy = (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString();
                    };
                }
                ComboBoxTimer.Stop();
                ComboBoxTimer.Start();
            }
        }

        public void SortByDescendingTimer(object sender, EventArgs e)
        {
            if (!wasItemChangedBySystem)
            {
                if (CheckBoxTimer == null)
                {
                    CheckBoxTimer = new DispatcherTimer();
                    CheckBoxTimer.Interval = TimeSpan.FromMilliseconds(500);
                    CheckBoxTimer.Tick += (s, args) => {
                        CheckBoxTimer.Stop();
                        bool descending = (sender as CheckBox).IsChecked ?? false;
                        BuildListBoxItems("", "", descending);
                        ConfigClass.Search.Descending = descendingOrderByCheckBox.IsChecked ?? false;
                    };
                }
                CheckBoxTimer.Stop();
                CheckBoxTimer.Start();
            }
        }

        private async void BuildListBoxItems(string query, string orderBy, bool descendingOrder)
        {
            queryMem = query.Length > 0 ? query : queryMem;
            orderByMem = orderBy.Length > 0 ? orderBy : orderByMem;
            if (queryMem.Length > 0)
            {
                listBox.Items.Clear();
                listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "Loading"));
                string entries = await ItemHandler.MakeWebSearch(queryMem);
                XDocument localEntries = ItemHandler.MakeLocalSearch(queryMem);
                string[] viewingStatuses = { "To Watch", "Watched", "Watching", "Dropped" };

                if (localEntries != null)
                {
                    if (!entries.Equals("Exception was thrown"))
                    {
                        if (entries.Length > 0)
                        {
                            listBox.Items.Clear();
                            XDocument response;
                            if (orderBy.Equals("No sort")) { response = XDocument.Parse(entries); }
                            else { response = SortEntries(XDocument.Parse(entries), orderByMem, descendingOrder); }
                            foreach (var e in response.Descendants("entry"))
                            {
                                int view = -1;
                                foreach (var el in localEntries.Descendants("entry"))
                                {
                                    if (int.Parse(e.Element("id").Value) == int.Parse(el.Element("id").Value))
                                    {
                                        view = Array.IndexOf(viewingStatuses, el.Element("viewingstatus").Value);
                                        listBox.Items.Add(new ListBoxItemForAnime(el, listBox, view, false));
                                        break;
                                    }
                                }
                                if (view == -1) { listBox.Items.Add(new ListBoxItemForAnime(e, listBox, view, false)); }
                            }
                        }
                        else
                        {
                            listBox.Items.Clear();
                            listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "No results for '" + query + "'."));
                        }
                    }
                    else if (entries.Equals("Exception was thrown"))
                    {
                        BuildListBoxItems(query, orderBy, descendingOrder);
                    }
                }
                else
                {
                    listBox.Items.Clear();
                    listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "Something is wrong with the 'MainEntries.xml' file. Consider switching in a back up."));
                }
                
            }
        }

        private XDocument SortEntries(XDocument entries, string orderBy, bool descendingOrder)
        {
            IOrderedEnumerable<XElement> sorted;
            bool orderByNumber = false;
            if (orderBy.Equals("Episodes") || orderBy.Equals("Score")) { orderByNumber = true; }

            if (descendingOrder)
            {
                if (orderByNumber) { sorted = entries.Descendants("entry").OrderByDescending(p => TryParsing(p.Element(orderBy.ToLower().Replace(" ", "_")).Value)); }
                else { sorted = entries.Descendants("entry").OrderByDescending(p => p.Element(orderBy.ToLower().Replace(" ", "_")).Value); }
            }
            else
            {
                if (orderByNumber) { sorted = entries.Descendants("entry").OrderBy(p => TryParsing(p.Element(orderBy.ToLower().Replace(" ", "_")).Value)); }
                else { sorted = entries.Descendants("entry").OrderBy(p => p.Element(orderBy.ToLower().Replace(" ", "_")).Value); }
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
