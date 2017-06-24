using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DesktopWeeabo
{
    class NotifitacationMessagesForListBox : ListBoxItem
    {
        public NotifitacationMessagesForListBox(double height, string message)
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            VerticalContentAlignment = VerticalAlignment.Center;
            BorderBrush = Brushes.Pink;
            BorderThickness = new Thickness(1);
            Content = SetMessage(height, message);
        }

        private TextBlock SetMessage(double height, string message) {
            return new TextBlock()
            {
                //Margin = new Thickness(0, (height / 2) - 20, 0, 0),
                FontSize = 30,
                Text = message,
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            };
        }
    }
}
