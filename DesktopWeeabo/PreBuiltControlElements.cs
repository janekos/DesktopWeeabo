using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace DesktopWeeabo
{
    class PreBuiltControlElements
    {
        private static Boolean OpenExtraInfo { get; set; } = false;

        public static ListBoxItem ListBoxItemForAnime(XElement e, ListBox listBox, int view)
        {
            Image image = new Image()
            {
                Source = new BitmapImage(new Uri((string)(e.Element("image")))),
                Height = 100,
                Width = 80,
                Margin = new Thickness(10, 10, 0, 10),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            TextBlock title = new TextBlock()
            {
                FontWeight = FontWeights.Bold,
                Text = (string)(e.Element("title")),
                TextTrimming = TextTrimming.WordEllipsis
            };

            TextBlock synopsis = new TextBlock()
            {
                Margin = new Thickness(0, 2, 0, 0),
                TextWrapping = TextWrapping.Wrap,
                TextTrimming = TextTrimming.WordEllipsis
            };
            string synposisText = CleanSynopsisText((string)(e.Element("synopsis")));
            synopsis.Text = synposisText;

            TextBlock english = new TextBlock();
            english.Margin = new Thickness(0, 10, 0, 0);
            english.Inlines.Add(new Bold(new Run("English: ")));
            english.Inlines.Add(new Run((string)(e.Element("english"))));

            TextBlock synonyms = new TextBlock();
            synonyms.Margin = new Thickness(0, 2, 0, 0);
            synonyms.Inlines.Add(new Bold(new Run("Synonyms: ")));
            synonyms.Inlines.Add(new Run((string)(e.Element("synonyms"))));

            TextBlock episodes = new TextBlock();
            episodes.Margin = new Thickness(0, 2, 0, 0);
            episodes.Inlines.Add(new Bold(new Run("Episodes: ")));
            episodes.Inlines.Add(new Run((string)(e.Element("episodes"))));

            TextBlock score = new TextBlock();
            score.Margin = new Thickness(0, 2, 0, 0);
            score.Inlines.Add(new Bold(new Run("Score: ")));
            score.Inlines.Add(new Run((string)(e.Element("score"))));

            TextBlock type = new TextBlock();
            type.Margin = new Thickness(0, 2, 0, 0);
            type.Inlines.Add(new Bold(new Run("Type: ")));
            type.Inlines.Add(new Run((string)(e.Element("type"))));

            TextBlock status = new TextBlock();
            status.Margin = new Thickness(0, 2, 0, 0);
            status.Inlines.Add(new Bold(new Run("Status: ")));
            status.Inlines.Add(new Run((string)(e.Element("status"))));

            TextBlock startDate = new TextBlock();
            startDate.Margin = new Thickness(0, 2, 0, 0);
            startDate.Inlines.Add(new Bold(new Run("Start date: ")));
            startDate.Inlines.Add(new Run((string)(e.Element("start_date"))));

            TextBlock endDate = new TextBlock();
            endDate.Margin = new Thickness(0, 2, 0, 0);
            endDate.Inlines.Add(new Bold(new Run("End date: ")));
            endDate.Inlines.Add(new Run((string)(e.Element("end_date"))));

            StackPanel middleTextPanel = new StackPanel()
            {
                Margin = new Thickness(95, 10, 110, 10)
            };
            middleTextPanel.Children.Add(title);
            middleTextPanel.Children.Add(synopsis);
            middleTextPanel.Children.Add(english);
            middleTextPanel.Children.Add(synonyms);
            middleTextPanel.Children.Add(episodes);
            middleTextPanel.Children.Add(score);
            middleTextPanel.Children.Add(type);
            middleTextPanel.Children.Add(status);
            middleTextPanel.Children.Add(startDate);
            middleTextPanel.Children.Add(endDate);

            TextBlock comboBoxLabel = new TextBlock()
            {
                Margin = new Thickness(0, 0, 0, 4),
                FontWeight = FontWeights.Bold,
                Text = "Viewing status"
            };

            ComboBoxItem cbi1 = new ComboBoxItem();
            cbi1.Content = "To Watch";
            ComboBoxItem cbi2 = new ComboBoxItem();
            cbi2.Content = "Watched";
            ComboBoxItem cbi3 = new ComboBoxItem();
            cbi3.Content = "Watching";
            ComboBoxItem cbi4 = new ComboBoxItem();
            cbi4.Content = "Dropped";

            ComboBox comboBox = new ComboBox()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 25,
                Width = 90
            };
            comboBox.Items.Add(cbi1);
            comboBox.Items.Add(cbi2);
            comboBox.Items.Add(cbi3);
            comboBox.Items.Add(cbi4);
            comboBox.SelectionChanged += (sender, sentEvent) => {
                var cbox = sender as ComboBox;
                var val = cbox.SelectedValue;
                var id = cbox.Name;
                //var parentName = VisualTreeHelper.GetParent(cbox) as Grid;
                System.Diagnostics.Debug.WriteLine(e);
            };

            TextBlock showMore = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.Blue,
                Text = "Show more",
                Margin = new Thickness(0, 37, 0, 0)
            };

            StackPanel rightTextPanel = new StackPanel()
            {
                Margin = new Thickness(0, 10, 10, 10),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 90
            };
            rightTextPanel.Children.Add(comboBoxLabel);
            rightTextPanel.Children.Add(comboBox);
            rightTextPanel.Children.Add(showMore);

            Grid grid = new Grid();
            grid.Name = "asd";
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Children.Add(image);
            grid.Children.Add(middleTextPanel);
            grid.Children.Add(rightTextPanel);

            ListBoxItem itm = new ListBoxItem()
            {
                Content = grid,
                Height = 120,
                Focusable = false,
                BorderThickness = new Thickness(0, 0, 0, 0.5),
                BorderBrush = Brushes.Black
            };
            showMore.MouseLeftButtonDown += (sender, eventarg) => {
                ExpandListItemForExtraInfo(sender, listBox, listBox.Items.IndexOf(itm), middleTextPanel.ActualHeight);
            };

            return itm;
        }

        public static ListBoxItem NotifitacationMessagesForListBox(ListBox lb, string message)
        {
            TextBlock tb = new TextBlock()
            {
                Margin = new Thickness(0, (lb.ActualHeight / 2) - 20, 0, 0),
                FontSize = 30,
                Text = message,
                FontWeight = FontWeights.Bold
            };

            ListBoxItem itm = new ListBoxItem()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            itm.Content = tb;

            return itm;
        }

        private static string CleanSynopsisText(string text)
        {
            text = text.Replace("<br />", "");
            text = text.Replace("&#039;", "'");
            text = text.Replace("[i]", "");
            text = text.Replace("[/i]", "");
            text = text.Replace("[spoiler]", "");
            text = text.Replace("[/spoiler]", "");
            text = text.Replace("/n", "");
            text = text.Replace("&mdash;", "-");
            text = text.Replace("&rsquo;", "'");

            return text;
        }

        private static void ExpandListItemForExtraInfo(object sender, ListBox listbox, int itmIndex, double height)
        {
            ListBoxItem senderItm = (ListBoxItem)listbox.Items.GetItemAt(itmIndex);
            TextBlock senderBlock = sender as TextBlock;
            if (OpenExtraInfo == false)
            {
                senderItm.Height = height + 20;
                OpenExtraInfo = true;
                senderBlock.Text = "Show less";
            }
            else
            {
                senderItm.Height = 120;
                OpenExtraInfo = false;
                senderBlock.Text = "Show more";
            }
        }
    }
}
