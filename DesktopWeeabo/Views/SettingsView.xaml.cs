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
    public partial class SettingsView : UserControl
    {
        private DispatcherTimer savingTimer;
        private bool canSave = true;
        private bool wasValueChangedBySystem = false;

        public SettingsView()
        {
            InitializeComponent();
            wasValueChangedBySystem = true;
            backUpCheckBox.IsChecked = ConfigClass.BackUp;
            colorPickingComboBox.Background = ConfigClass.Color;
            for (var i = 0; i < colorPickingComboBox.Items.Count; i++)
            {
                if ((colorPickingComboBox.Items[i] as ComboBoxItem).Content.ToString() == ConfigClass.Color.ToString())
                {
                    colorPickingComboBox.SelectedIndex = i;
                    break;
                }
            }
            wasValueChangedBySystem = false;
        }

        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            if (canSave && !wasValueChangedBySystem)
            {
                canSave = false;
                backUpSaved.Visibility = Visibility.Visible;
                SortByComboBoxTimer(backUpSaved);
                ConfigClass.BackUp = backUpCheckBox.IsChecked ?? true;
            }            
        }

        private void ColorPickingComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (canSave && !wasValueChangedBySystem)
            {
                canSave = false;
                colorSaved.Visibility = Visibility.Visible;
                SortByComboBoxTimer(colorSaved);
                ConfigClass.Color = (SolidColorBrush)new BrushConverter().ConvertFromString((colorPickingComboBox.SelectedItem as ComboBoxItem).Content.ToString());
                colorPickingComboBox.Background = ConfigClass.Color;
                ProgramColorChangingClass.ChangeColors();
            }            
        }

        private void Element_MouseEnter(object sender, MouseEventArgs e)
        {
            //futureproofing
            if (sender.GetType() == typeof(CheckBox))
            {
                CheckBox obj = sender as CheckBox;
                string name = obj.Name;
                switch (name)
                {
                    case "backUpCheckBox":
                        explanation.Text = backUpCheckBox.ToolTip.ToString();
                        break;
                }
            }
            else if (sender.GetType() == typeof(TextBlock))
            {
                TextBlock obj = sender as TextBlock;
                string name = obj.Name;
                switch (name)
                {
                    case "colorTextBlock":
                        explanation.Text = colorTextBlock.ToolTip.ToString();
                        break;
                }
            }
            else if (sender.GetType() == typeof(ComboBox))
            {
                ComboBox obj = sender as ComboBox;
                string name = obj.Name;
                switch (name)
                {
                    case "colorPickingComboBox":
                        explanation.Text = colorPickingComboBox.ToolTip.ToString();
                        break;
                }
            }
            explanationHeader.Visibility = Visibility.Visible;            
        }

        private void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            explanationHeader.Visibility = Visibility.Hidden;
            explanation.Text = "";
        }

        private void SortByComboBoxTimer(TextBlock savedText)
        {
            if (savingTimer == null)
            {
                savingTimer = new DispatcherTimer();
                savingTimer.Interval = TimeSpan.FromMilliseconds(500);
                savingTimer.Tick += (s, args) => {
                    savingTimer.Stop();
                    savedText.Visibility = Visibility.Hidden;
                    canSave = true;
                };
            }
            savingTimer.Stop();
            savingTimer.Start();
        }
    }
}
