using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallastCalculator
{
    class PanelBase  :BasicDimensions 
    {
        public int BaseID; 
        public int PanelID;
        public string EdgeID;
        public int LoadShare;
        public double LoadValue;
        public int BallastLocation;
        public Tuple<double, double> CenterPoint = new Tuple<double, double>(0,0); 
    }
}
