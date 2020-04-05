using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleFramework.Utils
{
    public class Angle
    {
        private double radians;

        public Angle(double radians)
        {
            this.radians = radians;
        }

        public double Radians
        {
            get => this.radians;
            set => this.radians = value;
        }

        public double Degrees
        {
            get => Converter.RadiansToDegrees(this.radians);
            set => this.radians = Converter.DegreesToRadians(value);
        }
    }
}
