using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace DesktopWeeabo
{
    public partial class SearchView : UserControl
    {

        private System.Windows.Threading.DispatcherTimer typingTimer;

        public SearchView()
        {
            InitializeComponent();
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
                    if (tb.Text.Length > 0)
                    {
                        BuildListBoxItems(tb.Text);
                    }
                    else
                    {
                        listBox.Items.Clear();
                    }
                };
            }
            typingTimer.Stop();
            typingTimer.Tag = tb.Text;
            typingTimer.Start();

        }

        private async void BuildListBoxItems(string query)
        {
            listBox.Items.Clear();
            listBox.Items.Add(new NotifitacationMessagesForListBox(listBox.ActualHeight, "Loading"));
            string entries = await ItemHandler.MakeWebSearch(query);
            XDocument localEntries = ItemHandler.MakeLocalSearch(query);
            string[] viewingStatuses = { "To Watch", "Watched", "Watching", "Dropped" };

            if (!entries.Equals("Exception was thrown"))
            {
                if (entries.Length > 0)
                {
                    listBox.Items.Clear();

                    XDocument response = XDocument.Parse(entries);
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
                BuildListBoxItems(query);
            }
        }
    }
}
