using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Linq;

namespace DesktopWeeabo
{
    public static class ConfigClass
    {
        private static bool backUp;
        private static SolidColorBrush color;
        private static ViewVariables toWatch;
        private static ViewVariables watched;
        private static ViewVariables watching;
        private static ViewVariables dropped;
        private static ViewVariables search;

        public static bool BackUp { get { return backUp; } set { backUp = value; } }
        public static SolidColorBrush Color { get { return color; } set { color = value; } }
        public static ViewVariables ToWatch { get { return toWatch; } set { toWatch = value; } }
        public static ViewVariables Watched { get { return watched; } set { watched = value; } }
        public static ViewVariables Watching { get { return watching; } set { watching = value; } }
        public static ViewVariables Dropped { get { return dropped; } set { dropped = value; } }
        public static ViewVariables Search { get { return search; } set { search = value; } }

        public static void SetVariables()
        {
            XDocument conf = ItemHandler.ManageSettings();
            XElement root = conf.Root;
            backUp = Convert.ToBoolean(root.Element("backup").Value);
            color = (SolidColorBrush)new BrushConverter().ConvertFromString(root.Element("color").Value);
            toWatch = new ViewVariables("towatch", root.Element("towatch").Element("orderby").Value, Convert.ToBoolean(root.Element("towatch").Element("descendingorderby").Value));
            watched = new ViewVariables("watched", root.Element("watched").Element("orderby").Value, Convert.ToBoolean(root.Element("watched").Element("descendingorderby").Value));
            watching = new ViewVariables("watching", root.Element("watching").Element("orderby").Value, Convert.ToBoolean(root.Element("watching").Element("descendingorderby").Value));
            dropped = new ViewVariables("dropped", root.Element("dropped").Element("orderby").Value, Convert.ToBoolean(root.Element("dropped").Element("descendingorderby").Value));
            search = new ViewVariables("search", root.Element("search").Element("orderby").Value, Convert.ToBoolean(root.Element("search").Element("descendingorderby").Value));
        }

        public static void SaveVariables()
        {
            ItemHandler.ManageSettings(backUp.ToString(), color, toWatch, watched, watching, dropped, search);
        }
    }
}
