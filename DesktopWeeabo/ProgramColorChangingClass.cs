using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DesktopWeeabo
{
    public static class ProgramColorChangingClass
    {
        public static void ChangeColors()
        {
            Application.Current.Resources["AppColor"] = ConfigClass.Color;
            if (Application.Current.Resources["AppColor"].ToString().Equals("#FF931515"))
            {
                Application.Current.Resources["AppColorForText"] = Brushes.White;
            }
            else
            {
                Application.Current.Resources["AppColorForText"] = Brushes.Black;
            }            
            SolidColorBrush scb = new SolidColorBrush()
            {
                Color = Color.FromArgb(45, ConfigClass.Color.Color.R, ConfigClass.Color.Color.G, ConfigClass.Color.Color.B)
            };
            Application.Current.Resources["AppColorOpacic"] = scb;
        }
    }
}
