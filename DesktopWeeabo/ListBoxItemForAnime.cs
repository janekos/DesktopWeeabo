using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml.Linq;

namespace DesktopWeeabo
{
    class ListBoxItemForAnime : ListBoxItem
    {
        private bool listBoxItemRemovalAllowance = true, wasEventFiredBySystem = false, isListBoxItemExpanded = false;
        private XElement animeEntryXML;
        private ListBox listbox;
        private int view;
        private string userInputTextReview, userInputTextDrop, userScoreInputText;
        private Grid grid;
        private string[] viewingStatuses = { "To Watch", "Watched", "Watching", "Dropped" };

        public ListBoxItemForAnime(XElement _e, ListBox _listbox, int _view, bool _listBoxItemRemovalAllowance)
        {
            listbox = _listbox;
            view = _view;
            listBoxItemRemovalAllowance = _listBoxItemRemovalAllowance;
            animeEntryXML = _e;

            userInputTextReview = animeEntryXML.Element("review") != null ? (string)(animeEntryXML.Element("review")) : "";
            userScoreInputText = animeEntryXML.Element("personalscore") != null ? (string)(animeEntryXML.Element("personalscore")) : "";
            userInputTextDrop = animeEntryXML.Element("dropreason") != null ? (string)(animeEntryXML.Element("dropreason")) : "";

            grid = SetGrid();

            Height = 120;
            Focusable = false;
            BorderThickness = new Thickness(0, 0, 0, 0.5);
            BorderBrush = Brushes.Black;
            Content = grid;
        }

        private Image SetCoverImage(string imgUrl)
        {
            return new Image()
            {
                Source = new BitmapImage(new Uri(imgUrl)),
                Height = 100,
                Width = 80,
                Margin = new Thickness(10, 10, 0, 10),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };
        }

        private TextBlock SetAnimeTitle(string title) {
            return new TextBlock()
            {
                FontWeight = FontWeights.Bold,
                Text = title,
                TextTrimming = TextTrimming.WordEllipsis,
                Margin = new Thickness(95, 10, 110, 10)
            };
        }

        private TextBlock SetUserInputTitle(string title) {
            TextBlock tb =  new TextBlock(){ Margin = new Thickness(0, 0, 0, 5) };
            tb.Inlines.Add(new Bold(new Run(title)));
            tb.Inlines.Add(new Run("(you can leave it empty)"));
            return tb;
        }

        private TextBox SetUserInputTextBox(bool showScore) {
            TextBox tb = new TextBox()
            { 
                Margin = new Thickness(0, 0, 0, 5),
                TextWrapping = TextWrapping.Wrap
            };
            tb.Text = showScore ? userInputTextReview : userInputTextDrop;
            return tb;
        }

        private TextBlock SetUserInputPersonalScoreTitle() {
            return new TextBlock()
            {
                Text = "Your score: ",
                FontWeight = FontWeights.Bold
            };
        }

        private TextBox SetUserInputPersonalScoreInput() {
            TextBox tb = new TextBox()
            {
                Text = userScoreInputText,
                Width = 60,
                Margin = new Thickness(0, 0, 10, 0)
            };
            tb.KeyUp += KeyUp_UserScoreInputTextBox;
            return tb;
        }

        private TextBlock SetUserInputPersonalScoreInputError() {
            return new TextBlock()
            {
                Text = "*Only numbers up to 10 are allowed.",
                Foreground = Brushes.Red,
                Visibility = Visibility.Hidden
            };
        }

        private StackPanel SetUserInputPersonalScore() {
            StackPanel sp = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 5, 0, 5)
            };
            sp.Children.Add(SetUserInputPersonalScoreTitle());
            sp.Children.Add(SetUserInputPersonalScoreInput());
            sp.Children.Add(SetUserInputPersonalScoreInputError());

            return sp;
        }

        private Button SetUserInputConfirm() {
            Button btn = new Button()
            {
                Content = "Confirm",
                Margin = new Thickness(0, 0, 10, 0),
                Width = 60
            };
            btn.Click += UserInputConfirm_Click;
            return btn;
        }

        private Button SetUserInputCancel() {
            Button btn = new Button()
            {
                Content = "Cancel",
                Width = 60
            };
            btn.Click += UserInputCancel_Click;
            return btn;
        }

        private StackPanel SetUserInputButtons() {
            StackPanel sp = new StackPanel()
            {
                Orientation = Orientation.Horizontal
            };
            sp.Children.Add(SetUserInputConfirm());
            sp.Children.Add(SetUserInputCancel());
            return sp;
        }

        private StackPanel SetUserInputStackPanel(string title, bool showScore) {
            StackPanel sp = new StackPanel()
            {
                Margin = new Thickness(95, 30, 110, 10)
            };
            sp.Children.Add(SetUserInputTitle(title));
            sp.Children.Add(SetUserInputTextBox(showScore));
            if (showScore) { sp.Children.Add(SetUserInputPersonalScore()); }           
            sp.Children.Add(SetUserInputButtons());
            return sp;
        }
        
        private TextBlock SetSynopsisText(string text) {
            TextBlock tb = new TextBlock()
            {
                Margin = new Thickness(0, 2, 0, 0),
                TextWrapping = TextWrapping.Wrap,
                TextTrimming = TextTrimming.WordEllipsis
            };
            string synposisText = CleanSynopsisText(text);
            tb.Inlines.Add(new Bold(new Run("Synopsis: ")));
            tb.Inlines.Add(new Run(synposisText));
            return tb;
        }

        private TextBlock SetAdditionalAnimeInfo(string first, string second)
        {
            TextBlock tb = new TextBlock() { Margin = new Thickness(0, 2, 0, 0) };
            tb.Inlines.Add(new Bold(new Run(first)));
            tb.Inlines.Add(new Run(second));
            return tb;
        }

        private TextBlock SetUserReviewText(string title, string text, Run textChange)
        {
            TextBlock tb = new TextBlock()
            {
                TextWrapping = TextWrapping.Wrap,
                TextTrimming = TextTrimming.WordEllipsis
            };
            tb.Inlines.Add(new Bold(new Run(title)));
            tb.Inlines.Add(new Run(text));
            tb.Inlines.Add(textChange);
            return tb;
        }

        private TextBlock SetReviewPersonalScore(string text)
        {
            TextBlock tb = new TextBlock();
            tb.Inlines.Add(new Bold(new Run("Personal score: ")));
            tb.Inlines.Add(new Run(text));
            return tb;
        }

        private Run SetAddChange(string first, bool showScore, Visibility visibility)
        {
            Run r = new Run(" Change") { Foreground = Brushes.Blue };
            r.MouseLeftButtonDown += (s, e) => { ShowUserInputPane(first, showScore, this); };
            return r;
        }

        private StackPanel SetMiddleStackPanel()
        {
            StackPanel sp = new StackPanel() { Margin = new Thickness(95, 30, 110, 10) };

            if (view == 1)
            {
                bool reviewCondition = userInputTextReview.Length > 0;
                bool scoreCondition = userScoreInputText.Length > 0;
                string userReviewTextBlock = reviewCondition ? userInputTextReview : "You haven't written a review for this anime.";
                string userScoreTextBlock = scoreCondition ? userScoreInputText : "You haven't scored this anime.";
                
                sp.Children.Insert(0, SetUserReviewText("Personal review: ", userReviewTextBlock, SetAddChange("Your personal review: ", true, Visibility.Visible)));
                sp.Children.Insert(1, SetReviewPersonalScore(userScoreTextBlock));
            }
            if (view == 3)
            {
                bool condition = userInputTextDrop.Length > 0;
                string userDropReasonTextBlock = condition ? userInputTextDrop : "You haven't written the dropping reason for this anime.";
                string userDropReasonTextBox = condition ? userInputTextDrop : "";

                sp.Children.Insert(0, SetUserReviewText("Dropping reason: ", userDropReasonTextBlock, SetAddChange("Your dropping reason: ", false, Visibility.Collapsed)));
            }
            sp.Children.Add(SetSynopsisText((string)(animeEntryXML.Element("synopsis"))));
            sp.Children.Add(SetAdditionalAnimeInfo("English: ", (string)(animeEntryXML.Element("english"))));
            sp.Children.Add(SetAdditionalAnimeInfo("Synonyms: ", (string)(animeEntryXML.Element("synonyms"))));
            sp.Children.Add(SetAdditionalAnimeInfo("Episodes: ", (string)(animeEntryXML.Element("episodes"))));
            sp.Children.Add(SetAdditionalAnimeInfo("Score: ", (string)(animeEntryXML.Element("score"))));
            sp.Children.Add(SetAdditionalAnimeInfo("Type: ", (string)(animeEntryXML.Element("type"))));
            sp.Children.Add(SetAdditionalAnimeInfo("Status: ", (string)(animeEntryXML.Element("status"))));
            sp.Children.Add(SetAdditionalAnimeInfo("Start date: ", (string)(animeEntryXML.Element("start_date"))));
            sp.Children.Add(SetAdditionalAnimeInfo("End date: ", (string)(animeEntryXML.Element("end_date"))));

            return sp;
        }

        private TextBlock SetComboBoxLabel() {
            return new TextBlock()
            {
                Margin = new Thickness(0, 0, 0, 4),
                FontWeight = FontWeights.Bold,
                Text = "Viewing status"
            };
        }

        private ComboBoxItem SetComboBoxItem(string name)
        {
            return new ComboBoxItem() { Content = name };
        }

        private ComboBox SetStatusComboBox() {
            ComboBox cb = new ComboBox()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 25,
                Width = 90
            };
            cb.Items.Add(SetComboBoxItem(viewingStatuses[0]));
            cb.Items.Add(SetComboBoxItem(viewingStatuses[1]));
            cb.Items.Add(SetComboBoxItem(viewingStatuses[2]));
            cb.Items.Add(SetComboBoxItem(viewingStatuses[3]));
            if (view != -1) {
                cb.Items.Add(SetComboBoxItem("Remove"));
                wasEventFiredBySystem = true;
                cb.SelectedIndex = view;
                wasEventFiredBySystem = false;
            }
            cb.MouseWheel += ComboBox_MouseWheel;
            cb.SelectionChanged += StatusComboBox_SelectionChanged;
            return cb;
        }

        private TextBlock SetShowMoreText() {
            TextBlock tb = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = Brushes.Blue,
                Text = "Show more",
                Margin = new Thickness(0, 37, 0, 0)
            };
            tb.MouseLeftButtonDown += (s, e) => { ExpandListItem(this); };
            return tb;
        }

        private StackPanel SetRightStackPanel() {
            StackPanel sp = new StackPanel()
            {
                Margin = new Thickness(0, 10, 10, 10),
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Right,
                Width = 90
            };
            sp.Children.Add(SetComboBoxLabel());
            sp.Children.Add(SetStatusComboBox());
            sp.Children.Add(SetShowMoreText());
            return sp;
        }

        private Grid SetGrid()
        {
            Grid g = new Grid() { VerticalAlignment = VerticalAlignment.Top };
            g.Children.Add(SetCoverImage((string)(animeEntryXML.Element("image"))));
            g.Children.Add(SetAnimeTitle((string)(animeEntryXML.Element("title"))));
            g.Children.Add(SetMiddleStackPanel());
            g.Children.Add(SetRightStackPanel());

            return g;
        }

        private void UserInputCancel_Click(object sender, RoutedEventArgs e) {
            TextBlock showMore = (grid.Children[3] as StackPanel).Children[2] as TextBlock;
            ComboBox statusComboBox = (grid.Children[3] as StackPanel).Children[1] as ComboBox;
            showMore.Visibility = Visibility.Visible;
            grid.Children.RemoveAt(2);
            grid.Children.Insert(2, SetMiddleStackPanel());
            statusComboBox.IsEnabled = true;
            wasEventFiredBySystem = true;
            statusComboBox.SelectedIndex = view;
            wasEventFiredBySystem = false;
            if (isListBoxItemExpanded) { ExpandListItem(this); }
        }

        private void UserInputConfirm_Click(object sender, RoutedEventArgs e)
        {
            ComboBox statusComboBox = (grid.Children[3] as StackPanel).Children[1] as ComboBox;
            TextBox userInputTextBox = ((grid.Children[2] as StackPanel).Children[1] as TextBox);
            TextBox userScoreInputTextBox = ((grid.Children[2] as StackPanel).Children[2] as StackPanel).Children[1] as TextBox;
            int selectedComboBoxItemIndex = statusComboBox.Items.IndexOf(statusComboBox.SelectedItem);
            string viewingStatusString = (string)(statusComboBox.SelectedItem as ComboBoxItem).Content;
            userScoreInputText = "";
            if (selectedComboBoxItemIndex == 1) {
                userInputTextReview = userInputTextBox.Text.Length > 0 ? userInputTextBox.Text : "";
                userScoreInputText = userScoreInputTextBox.Text.Length > 0 ? userScoreInputTextBox.Text : "";
                ItemHandler.UpdateSaveFile(animeEntryXML, viewingStatusString, userInputTextReview, userScoreInputText);
            }
            else if (selectedComboBoxItemIndex == 3)
            {
                userInputTextDrop = userInputTextBox.Text.Length > 0 ? userInputTextBox.Text : "";
                ItemHandler.UpdateSaveFile(animeEntryXML, viewingStatusString, "", "", userInputTextDrop);
            }
            if (listBoxItemRemovalAllowance && ((view != 1 && selectedComboBoxItemIndex == 1) || (view != 3 && selectedComboBoxItemIndex == 3))) {
                listbox.Items.Remove(this);
            } else {
                ReloadViewAndGrid(selectedComboBoxItemIndex, 3);
                view = selectedComboBoxItemIndex;
                UserInputCancel_Click(new object(), new RoutedEventArgs());
            }
        }

        private void StatusComboBox_SelectionChanged(object sender, EventArgs e)
        {
            if (!wasEventFiredBySystem)
            {
                ComboBox statusComboBox = sender as ComboBox;
                ComboBoxItem cbi = statusComboBox.SelectedItem as ComboBoxItem;
                string viewingStatus = (string)cbi.Content;
                int selectedViewingStatusInt = statusComboBox.Items.IndexOf(statusComboBox.SelectedItem);
                switch (selectedViewingStatusInt)
                {
                    case 1:
                        
                        if (userInputTextReview.Length > 0 || userScoreInputText.Length > 0)
                        {
                            if (listBoxItemRemovalAllowance)
                            {
                                listbox.Items.Remove(this);
                            }
                            else
                            {
                                ReloadViewAndGrid(selectedViewingStatusInt, 2);
                                ReloadViewAndGrid(selectedViewingStatusInt, 3);
                            }
                            ItemHandler.UpdateSaveFile(animeEntryXML, viewingStatus);
                        }
                        else
                        {
                            ShowUserInputPane("Your personal review: ", true, this);
                        }
                        break;
                    case 3:
                        if (userInputTextDrop.Length > 0)
                        {
                            if (listBoxItemRemovalAllowance)
                            {
                                listbox.Items.Remove(this);
                            }
                            else
                            {
                                ReloadViewAndGrid(selectedViewingStatusInt, 2);
                                ReloadViewAndGrid(selectedViewingStatusInt, 3);
                            }
                            ItemHandler.UpdateSaveFile(animeEntryXML, viewingStatus);
                        }
                        else
                        {
                            ShowUserInputPane("Your dropping reason: ", false, this);
                        }

                        break;
                    case 4:
                        if (!listBoxItemRemovalAllowance)
                        {
                            ReloadViewAndGrid(-1, 2);
                            ReloadViewAndGrid(-1, 3);
                        }
                        else
                        {
                            listbox.Items.Remove(this);
                            if (listbox.Items.Count == 0)
                            {
                                listbox.Items.Add(new NotifitacationMessagesForListBox(listbox.ActualHeight, "You have not listed any animes as '" + viewingStatuses[view] + "'."));
                            }
                        }
                        ItemHandler.UpdateSaveFile(animeEntryXML, viewingStatus, "", "", "", true);

                        break;
                    default:
                        if (listBoxItemRemovalAllowance)
                        {
                            listbox.Items.Remove(this);
                        }
                        else
                        {
                            ReloadViewAndGrid(selectedViewingStatusInt, 2);
                            ReloadViewAndGrid(selectedViewingStatusInt, 3);
                        }
                        ItemHandler.UpdateSaveFile(animeEntryXML, viewingStatus);
                        break;
                }
            }
        }

        private void KeyUp_UserScoreInputTextBox(object sender, EventArgs e)
        {
            TextBox scoreInputTextBox = sender as TextBox;
            Button confirmButton = ((grid.Children[2] as StackPanel).Children[3] as StackPanel).Children[0] as Button;
            TextBlock userInputPersonalScoreInputError = ((grid.Children[2] as StackPanel).Children[2] as StackPanel).Children[2] as TextBlock;
            bool isDouble = double.TryParse(scoreInputTextBox.Text, out double score);

            if ((!isDouble && scoreInputTextBox.Text.Length > 0) || !(score >= 0 && score <= 10))
            {
                confirmButton.IsEnabled = false;
                userInputPersonalScoreInputError.Visibility = Visibility.Visible;
            }
            else if(isDouble || scoreInputTextBox.Text.Length == 0)
            {
                confirmButton.IsEnabled = true;
                userInputPersonalScoreInputError.Visibility = Visibility.Hidden;
            }
        }

        private static string CleanSynopsisText(string text)
        {
            text = text.Replace("<br />", " ");
            text = text.Replace("&#039;", "'");
            text = text.Replace("[i]", "");
            text = text.Replace("[/i]", "");
            text = text.Replace("[spoiler]", "");
            text = text.Replace("[/spoiler]", "");
            text = text.Replace("\n", " ");
            text = text.Replace("&mdash;", "-");
            text = text.Replace("&rsquo;", "'");

            return text;
        }

        private static void ExpandListItem(ListBoxItemForAnime instance)
        {
            TextBlock showMore = (instance.grid.Children[3] as StackPanel).Children[2] as TextBlock;
            if (instance.isListBoxItemExpanded == false)
            {
                instance.Height = double.NaN;
                showMore.Text = "Show less";
                instance.isListBoxItemExpanded = true;
            }
            else
            {
                instance.Height = 120;
                showMore.Text = "Show more";
                instance.isListBoxItemExpanded = false;
            }
        }

        private static void ShowUserInputPane(string headline, bool showScore, ListBoxItemForAnime instance)
        {
            ComboBox statusComboBox = (instance.grid.Children[3] as StackPanel).Children[1] as ComboBox;
            TextBlock showMore = ((instance.grid.Children[3] as StackPanel).Children[2] as TextBlock);
            statusComboBox.IsEnabled = false;
            showMore.Visibility = Visibility.Hidden;
            instance.grid.Children.RemoveAt(2);
            instance.grid.Children.Insert(2, instance.SetUserInputStackPanel(headline, showScore));

            if (!instance.isListBoxItemExpanded)
            {
                ExpandListItem(instance);
            }
        }

        private void ReloadViewAndGrid(int newView, int whichPanel)
        {
            view = newView;
            grid.Children.RemoveAt(whichPanel);

            if (whichPanel == 2)
            {
                grid.Children.Insert(2, SetMiddleStackPanel());
            }
            else if(whichPanel == 3)
            {
                grid.Children.Insert(3, SetRightStackPanel());
            }            
        }

        void ComboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }
    }
}
