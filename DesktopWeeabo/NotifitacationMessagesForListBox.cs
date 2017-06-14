using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DesktopWeeabo
{
    class NotifitacationMessagesForListBox : ListBoxItem
    {
        public NotifitacationMessagesForListBox(double height, string message)
        {
            HorizontalAlignment = HorizontalAlignment.Center;
            HorizontalContentAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            VerticalContentAlignment = VerticalAlignment.Center;
            Content = SetMessage(height, message);
        }

        private TextBlock SetMessage(double height, string message) {
            return new TextBlock()
            {
                Margin = new Thickness(0, (height / 2) - 20, 0, 0),
                FontSize = 30,
                Text = message,
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            };
        }
    }
}
