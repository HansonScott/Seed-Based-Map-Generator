using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapGenerator
{
    public partial class MapSettings : Form
    {
        public double Smoothness
        {
            get
            {
                return (double)this.nudSmoothness.Value;
            }
        }
        public int WaterLevel
        {
            get
            {
                return (int)this.nudWater.Value;
            }
        }

        public bool AddSmooth01 { get { return true; } }
        public bool AddSmooth02 { get { return cbAdd02.Checked; } }
        public bool AddSmooth03 { get { return cbAdd03.Checked; } }
        public bool AddSmooth04 { get { return cbAdd04.Checked; } }
        public double SmoothnessFactor { get { return (double)nudSmoothness.Value; } }
        public double SmoothnessFactor02 { get { return (double)nudGrain02.Value; } }
        public double SmoothnessFactor03 { get { return (double)nudGrain03.Value; } }
        public double SmoothnessFactor04 { get { return (double)nudGrain04.Value; } }
        public double Amp01 { get { return 1.0; } }
        public double Amp02 { get { return (double)nudAmp02.Value; } }
        public double Amp03 { get { return (double)nudAmp03.Value; } }
        public double Amp04 { get { return (double)nudAmp04.Value; } }

        public MapSettings()
        {
            InitializeComponent();
        }
    }
}
