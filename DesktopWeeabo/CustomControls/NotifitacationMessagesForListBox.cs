using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DesktopWeeabo.CustomControls
{
    class NotifitacationMessagesForListBox : ListBoxItem
    {
        public NotifitacationMessagesForListBox(double height, string message)
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            Height = height-10;
            Foreground = Application.Current.Resources["AppColorForText"] as SolidColorBrush;
            BorderThickness = new Thickness(0);
            Focusable = false;
            Content = SetMessage(message);
        }

        private TextBlock SetMessage(string message) {
            return new TextBlock()
            {
                FontSize = 30,
                Text = message,
                FontWeight = FontWeights.Bold,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            };
        }
    }
}
