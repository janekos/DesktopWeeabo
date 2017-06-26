using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Xml.Linq;

namespace DesktopWeeabo.Views
{
    public partial class SearchView : UserControl
    {

        private System.Windows.Threading.DispatcherTimer typingTimer, ComboBoxTimer, CheckBoxTimer;
        private RepeatingViewFunctions rvf = new RepeatingViewFunctions();
        private XDocument config;
        private string queryMem = "";
        private string orderByMem;
        private bool wasItemChangedBySystem = false;

        public SearchView()
        {
            InitializeComponent();
            config = ItemHandler.ManageSettings();
            wasItemChangedBySystem = true;
            for (var i = 0; i < 10; i++)
            {
                if ((orderByComboBox.Items[i] as ComboBoxItem).Content.ToString() == config.Root.Element("search").Element("orderby").Value)
                {
                    orderByComboBox.SelectedIndex = i;
                    break;
                }
            }
            if (orderByComboBox.SelectedIndex == 0) { descendingOrderByCheckBox.IsEnabled = false; }
            descendingOrderByCheckBox.IsChecked = Convert.ToBoolean(config.Root.Element("search").Element("descendingorderby").Value);
            wasItemChangedBySystem = false;

            Loaded += delegate
            {
                listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "Try searching for something above."));
            };
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
                        listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "Try searching for something above."));
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
                        ItemHandler.ManageSettings("", "search", (orderByComboBox.SelectedItem as ComboBoxItem).Content.ToString(), "");
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
                        ItemHandler.ManageSettings("", "search", "", descendingOrderByCheckBox.IsChecked.ToString());
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
                        listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "No results"));
                    }
                }
                else if (entries.Equals("Exception was thrown"))
                {
                    BuildListBoxItems(query, orderBy, descendingOrder);
                }
            }
        }

        private XDocument SortEntries(XDocument entries, string orderBy, bool descendingOrder)
        {
            IOrderedEnumerable<XElement> sorted;
            if (descendingOrder)
            {
                sorted = entries.Descendants("entry").OrderByDescending(p => p.Element(orderBy.ToLower().Replace(" ", "_")).Value);
            }
            else
            {
                sorted = entries.Descendants("entry").OrderBy(p => p.Element(orderBy.ToLower().Replace(" ", "_")).Value);
            }
            XDocument doc = new XDocument(new XElement("anime", sorted));
            return doc;
        }
    }
}
