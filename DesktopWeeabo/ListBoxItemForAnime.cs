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
using System.Xml.Linq;

namespace DesktopWeeabo
{
    class ListBoxItemForAnime : ListBoxItem
    {
        private bool listBoxItemRemovalAllowance = true, wasEventFiredBySystem = false, isListBoxItemExpanded = false;
        private XElement animeEntryXML;
        private ListBox listbox;
        private int view, listBoxItemIndex;
        private TextBlock userInputTitle, showMoreText, userInputPersonalScoreTitle, userInputPersonalScoreInputError;
        private TextBox userInputTextBox, userInputPersonalScoreInput;
        private StackPanel userInputButtons, userInputPersonalScore, userInputStackPanel, middleStackPanel;
        private Button userInputConfirm, userInputCancel;
        private ComboBox statusComboBox;
        private Grid grid;

        public ListBoxItemForAnime(XElement _e, ListBox _listbox, int _view, bool _listBoxItemRemovalAllowance, int _listBoxItemIndex)
        {
            listbox = _listbox;
            view = _view;
            listBoxItemRemovalAllowance = _listBoxItemRemovalAllowance;
            animeEntryXML = _e;
            listBoxItemIndex = _listBoxItemIndex;

            userInputTitle = SetUserInputTitle();
            userInputTextBox = SetUserInputTextBox();
            userInputPersonalScoreTitle = SetUserInputPersonalScoreTitle();
            userInputPersonalScoreInput = SetUserInputPersonalScoreInput();
            userInputPersonalScoreInputError = SetUserInputPersonalScoreInputError();
            userInputPersonalScore = SetUserInputPersonalScore();
            userInputConfirm = SetUserInputConfirm();
            userInputCancel = SetUserInputCancel();
            userInputButtons = SetUserInputButtons();
            userInputStackPanel = SetUserInputStackPanel();
            middleStackPanel = SetMiddleStackPanel((string)(animeEntryXML.Element("review")), (string)(animeEntryXML.Element("personalscore")), (string)(animeEntryXML.Element("dropreason")));            
            statusComboBox = SetStatusComboBox();
            showMoreText = SetShowMoreText();

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

        private TextBlock SetUserInputTitle() {
            return new TextBlock(){ Margin = new Thickness(0, 0, 0, 5) };
        }

        private TextBox SetUserInputTextBox() {
            return new TextBox()
            {
                Margin = new Thickness(0, 0, 0, 5),
                TextWrapping = TextWrapping.Wrap
            };
        }

        private TextBlock SetUserInputPersonalScoreTitle() {
            return new TextBlock()
            {
                Text = "Your score: ",
                FontWeight = FontWeights.Bold
            };
        }

        private TextBox SetUserInputPersonalScoreInput() {
            return new TextBox()
            {
                Width = 60,
                Margin = new Thickness(0, 0, 10, 0)
            };
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
            sp.Children.Add(userInputPersonalScoreTitle);
            sp.Children.Add(userInputPersonalScoreInput);
            sp.Children.Add(userInputPersonalScoreInputError);

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
            sp.Children.Add(userInputConfirm);
            sp.Children.Add(userInputCancel);
            return sp;
        }

        private StackPanel SetUserInputStackPanel() {
            StackPanel sp = new StackPanel()
            {
                Margin = new Thickness(95, 30, 110, 10)
            };
            sp.Children.Add(userInputTitle);
            sp.Children.Add(userInputTextBox);
            sp.Children.Add(userInputPersonalScore);
            sp.Children.Add(userInputButtons);
            sp.Visibility = Visibility.Collapsed;
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

        private StackPanel SetMiddleStackPanel(string userReview, string userScore, string userDropReason) {

            StackPanel sp = new StackPanel() { Margin = new Thickness(95, 30, 110, 10) };
            if (view == 1)
            {
                bool reviewCondition = (userReview != null) && (userReview.Length > 0);
                bool scoreCondition = (userScore != null) && (userScore.Length > 0);

                string userReviewTextBlock = reviewCondition ? userReview : "You haven't written a review for this anime.";
                string userReviewTextBox = reviewCondition ? userReview : "";
                string userScoreTextBlock = scoreCondition ? userScore : "You haven't scored this anime.";
                string userScoreTextBox = scoreCondition ? userScore : "";

                Run addReview = new Run(" Change") { Foreground = Brushes.Blue };
                addReview.MouseLeftButtonDown += (s, e) => { ShowUserInputPane("Your personal review: ", Visibility.Visible, this); };

                TextBlock review = new TextBlock()
                {
                    TextWrapping = TextWrapping.Wrap,
                    TextTrimming = TextTrimming.WordEllipsis
                };
                review.Inlines.Add(new Bold(new Run("Personal review: ")));
                review.Inlines.Add(new Run(userReview));
                review.Inlines.Add(addReview);
                userInputTextBox.Text = userReviewTextBox;

                TextBlock reviewPersonalScore = new TextBlock();
                reviewPersonalScore.Inlines.Add(new Bold(new Run("Personal score: ")));
                reviewPersonalScore.Inlines.Add(new Run(userScore));
                userInputPersonalScoreInput.Text = userScoreTextBox;

                sp.Children.Add(review);
                sp.Children.Add(reviewPersonalScore);
            }
            if (view == 3)
            {
                bool condition = (userDropReason != null) && (userDropReason.Length > 0);

                string userDropReasonTextBlock = condition ? userDropReason : "You haven't written the dropping reason for this anime.";
                string userDropReasonTextBox = condition ? userDropReason : "";

                Run addDropReason = new Run(" Change") { Foreground = Brushes.Blue };
                addDropReason.MouseLeftButtonDown += (s, e) => { ShowUserInputPane("Your dropping reason: ", Visibility.Collapsed, this); };

                TextBlock dropReason = new TextBlock()
                {
                    TextWrapping = TextWrapping.Wrap,
                    TextTrimming = TextTrimming.WordEllipsis
                };
                dropReason.Inlines.Add(new Bold(new Run("Dropping reason: ")));
                dropReason.Inlines.Add(new Run(userDropReason));
                dropReason.Inlines.Add(addDropReason);
                userInputTextBox.Text = userDropReasonTextBox;

                sp.Children.Add(dropReason);
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
            cb.Items.Add(SetComboBoxItem("To Watch"));
            cb.Items.Add(SetComboBoxItem("Watched"));
            cb.Items.Add(SetComboBoxItem("Watching"));
            cb.Items.Add(SetComboBoxItem("Dropped"));
            if (view != -1) { cb.SelectedIndex = view; }
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
            sp.Children.Add(statusComboBox);
            sp.Children.Add(showMoreText);
            return sp;
        }

        private Grid SetGrid()
        {
            Grid g = new Grid() { VerticalAlignment = VerticalAlignment.Top };
            g.Children.Add(SetCoverImage((string)(animeEntryXML.Element("image"))));
            g.Children.Add(SetAnimeTitle((string)(animeEntryXML.Element("title"))));
            g.Children.Add(userInputStackPanel);
            g.Children.Add(middleStackPanel);
            g.Children.Add(SetRightStackPanel());

            return g;
        }

        private void UserInputCancel_Click(object sender, RoutedEventArgs e) {
            userInputTitle.Inlines.Clear();
            userInputStackPanel.Visibility = Visibility.Collapsed;
            middleStackPanel.Visibility = Visibility.Visible;
            showMoreText.Visibility = Visibility.Visible;
            statusComboBox.IsEnabled = true;
            wasEventFiredBySystem = true;
            statusComboBox.SelectedIndex = view;
            wasEventFiredBySystem = false;
            ExpandListItem(this);
        }

        private void UserInputConfirm_Click(object sender, RoutedEventArgs e)
        {
            int selectedComboBoxItemIndex = statusComboBox.Items.IndexOf(statusComboBox.SelectedItem);
            string viewingStatus = (string)(statusComboBox.SelectedItem as ComboBoxItem).Content;
            string userInput = userInputTextBox.Text;

            if (selectedComboBoxItemIndex == 1)
            {
                double userScore = double.NaN;
                int scoreInputLen = userInputPersonalScoreInput.Text.Length;
                bool isDouble = scoreInputLen > 0 ? Double.TryParse(userInputPersonalScoreInput.Text, out userScore) : true;
                if (!isDouble)
                {
                    userInputPersonalScoreInputError.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    if ((userScore != userScore) || (userScore >= 0 && userScore <= 10))
                    {
                        ItemHandler.UpdateSaveFile(animeEntryXML, viewingStatus, userInput, userScore != userScore ? "" : userScore.ToString(), "");
                    }
                    else
                    {
                        userInputPersonalScoreInputError.Visibility = Visibility.Visible;
                        return;
                    }
                }
            }
            else if (selectedComboBoxItemIndex == 3)
            {
                ItemHandler.UpdateSaveFile(animeEntryXML, viewingStatus, "", "", userInput);
            }

            if (listBoxItemRemovalAllowance && view != 1 && view != 3)
            {
                listbox.Items.Remove(this);
            }
            else if (!listBoxItemRemovalAllowance || view == 1 || view == 3)
            {
                UserInputCancel_Click(new object(), new RoutedEventArgs());
            }
        }

        private void StatusComboBox_SelectionChanged(object sender, EventArgs e)
        {
            if (!wasEventFiredBySystem)
            {
                ComboBoxItem cbi = (sender as ComboBox).SelectedItem as ComboBoxItem;
                string viewingStatus = (string)cbi.Content;
                int viewingStatusInt = statusComboBox.Items.IndexOf(statusComboBox.SelectedItem);
                if (viewingStatusInt == 1 || viewingStatusInt == 3)
                {
                    if (viewingStatusInt == 1)
                    {
                        ShowUserInputPane("Your personal review: ", Visibility.Visible, this);
                    }
                    else if (viewingStatusInt == 3)
                    {
                        ShowUserInputPane("Your dropping reason: ", Visibility.Collapsed, this);
                    }
                }
                else
                {
                    ItemHandler.UpdateSaveFile(animeEntryXML, viewingStatus, "", "", "");
                    if (listBoxItemRemovalAllowance) { listbox.Items.Remove(this); }
                }
                //var listBoxItem = VisualTreeHelper.GetParent(VisualTreeHelper.GetParent(cbox) as Grid) as ListBoxItem;
                //System.Diagnostics.Debug.WriteLine();
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
            if (instance.isListBoxItemExpanded == false)
            {
                instance.Height = double.NaN;
                instance.showMoreText.Text = "Show less";
                instance.isListBoxItemExpanded = true;
            }
            else
            {
                instance.Height = 120;
                instance.showMoreText.Text = "Show more";
                instance.isListBoxItemExpanded = false;
            }
        }

        private static void ShowUserInputPane(string title, Visibility visibility, ListBoxItemForAnime instance)
        {
            instance.userInputTitle.Inlines.Add(new Bold(new Run(title)));
            instance.userInputTitle.Inlines.Add(new Run("(you can leave it empty)"));
            instance.userInputPersonalScore.Visibility = visibility;
            instance.userInputStackPanel.Visibility = Visibility.Visible;
            instance.middleStackPanel.Visibility = Visibility.Collapsed;
            instance.showMoreText.Visibility = Visibility.Hidden;
            instance.statusComboBox.IsEnabled = false;

            if (instance.isListBoxItemExpanded)
            {
                ExpandListItem(instance);
            }
            else
            {
                ExpandListItem(instance);
                instance.isListBoxItemExpanded = false;
            }
        }
    }
}
