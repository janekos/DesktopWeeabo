﻿using System;
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
            Height = height-10;
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
