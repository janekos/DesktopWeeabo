using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace DesktopWeeabo
{
    //[ContentProperty("Items")]
    public partial class CustomComboBox : UserControl
    {
        //maybe in future
        /*private bool isDropDown = true;
        public static readonly DependencyProperty ThisSelectedIndex = DependencyProperty.Register("SelectedIndex", typeof(int), typeof(CustomComboBox));
        public static readonly DependencyProperty CustomComboBoxProperty = DependencyProperty.Register("Items", typeof(UIElementCollection), typeof(CustomComboBox));

        public UIElementCollection Items
        {
            get { return (UIElementCollection)GetValue(CustomComboBoxProperty); }
            set { SetValue(CustomComboBoxProperty, value); }
        }

        public int ThisSelectedIndexProperty
        {
            get { return (int)GetValue(ThisSelectedIndex); }
            set { SetValue(ThisSelectedIndex, value); }
        }

        public string ThisSelectedItem
        {
            get { return SelectedItem.Content.ToString(); }
            set { SelectedItem.Content = value; }
        }

        public CustomComboBox()
        {
            InitializeComponent();
            Items = new UIElementCollection(this, this);
            Loaded += BaseControl_Loaded;
        }

        private void ShowDropDown(object sender, EventArgs ea)
        {
            if (isDropDown) { ItemStackPanel.Visibility = Visibility.Visible; isDropDown = false; }
            else { ItemStackPanel.Visibility = Visibility.Collapsed; isDropDown = true; }
        }

        void BaseControl_Loaded(object sender, RoutedEventArgs e)
        {
            int count = 0;
            foreach (UIElement element in Items.Cast<UIElement>().ToList())
            {
                Items.Remove(element);
                PanelItSelf.Children.Add(element);
                (PanelItSelf.Children[count] as Label).MouseLeftButtonDown += ItemClick;
                try
                {
                    if(count == ThisSelectedIndexProperty)
                    {
                        ThisSelectedItem = (element as Label).Content.ToString();
                    }
                } catch { }
                count++;
            }
        }

        private void ItemClick(object sender, EventArgs ea)
        {
            SelectedItem.Content = (sender as Label).Content;
            ShowDropDown(new object(), new EventArgs());
        }*/
    }
}
