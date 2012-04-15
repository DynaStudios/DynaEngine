using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynaStudios.Blocks
{
    public class Direction
    {
        private double x = 0.0;
        public double X {
            get { return x; }
            set { x = adjust(value); }
        }
        private double y = 0.0;
        public double Y {
            get { return y; }
            set { y = adjust(value); }
        }
        private double rotation = 0.0;
        public double Rotation {
            get { return rotation; }
            set { rotation = adjust(value); }
        }

        private double adjust(double value)
        {
            double ret = value;
            if (value > 0) {
                while (ret >= 360) {
                    ret -= 360;
                }
            } else {
                while (ret <= -360) {
                    ret += 360;
                }
            }
            return ret;
        }
    }
}
