using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;


namespace BiedapolyGUI
{
    public interface RefreshField
    {
        void Refresh();

        void SetMargin(double a, double b, double c, double d);

        Canvas getCanvas();
    }
}
