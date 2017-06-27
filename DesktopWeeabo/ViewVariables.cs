using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopWeeabo
{
    public class ViewVariables
    {
        private string viewVal;
        private string orderByVal;
        private bool descendingVal;

        public string View { get { return viewVal; } set { viewVal = value; } }
        public string OrderBy { get { return orderByVal; } set { orderByVal = value; } }
        public bool Descending { get { return descendingVal; } set { descendingVal = value; } }

        public ViewVariables(string _viewVal, string _orderByVal, bool _descendingVal)
        {
            viewVal = _viewVal;
            orderByVal = _orderByVal;
            descendingVal = _descendingVal;
        }
    }
}
