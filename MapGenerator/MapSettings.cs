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
        public float Smoothness { get { return (float)this.nudSmoothness.Value; } }
        public bool AddSmooth01 { get { return true; } }
        public bool AddSmooth02 { get { return cbAdd02.Checked; } }
        public bool AddSmooth03 { get { return cbAdd03.Checked; } }
        public bool AddSmooth04 { get { return cbAdd04.Checked; } }
        public float SmoothnessFactor { get { return (float)nudSmoothness.Value; } }
        public float SmoothnessFactor02 { get { return (float)nudGrain02.Value; } }
        public float SmoothnessFactor03 { get { return (float)nudGrain03.Value; } }
        public float SmoothnessFactor04 { get { return (float)nudGrain04.Value; } }
        public float Amp01 { get { return 1.0f; } }
        public float Amp02 { get { return (float)nudAmp02.Value; } }
        public float Amp03 { get { return (float)nudAmp03.Value; } }
        public float Amp04 { get { return (float)nudAmp04.Value; } }

        public float ContinentBias { get { return (float)nudContinentBias.Value; } }
        public float RiverBias { get { return (float)nudRiverBias.Value; } }
        public float RiverSourceElevationMin { get { return (float)nudRiverSourceElevationMin.Value; } }
        public int LakeSize { get { return (int)nudLakeSize.Value; } }

        public bool AddTSmooth01 { get { return true; } }
        public bool AddTSmooth02 { get { return cbAddTempSmooth02.Checked; } }
        public bool AddTSmooth03 { get { return cbAddTempSmooth03.Checked; } }
        public bool AddTSmooth04 { get { return cbAddTSmooth04.Checked; } }
        public float TSmoothnessFactor { get { return (float)nudSmoothness.Value; } }
        public float TSmoothnessFactor02 { get { return (float)nudTGrain02.Value; } }
        public float TSmoothnessFactor03 { get { return (float)nudTGrain03.Value; } }
        public float TSmoothnessFactor04 { get { return (float)nudTGrain04.Value; } }
        public float TAmp01 { get { return 0.5f; } }
        public float TAmp02 { get { return (float)nudTIntensity02.Value; } }
        public float TAmp03 { get { return (float)nudTIntensity03.Value; } }
        public float TAmp04 { get { return (float)nudTIntensity04.Value; } }

        public float PolarBias { get { return (float)nudTPolarBias.Value; } }
        public float ElevationBias { get { return (float)nudTempElevationBias.Value; } }
        public int ElevationMax { get { return (int)nudEMax.Value; } }
        public float ElevationWater { get { return (float)this.nudEWater.Value; } }
        public bool AddRGrain01 { get { return true; } }
        public bool AddRGrain02 { get { return cbRAdd02.Checked; } }
        public bool AddRGrain03 { get { return cbRAdd03.Checked; } }
        public bool AddRGrain04 { get { return cbRAdd04.Checked; } }
        public float RSmoothnessFactor01 { get { return (float)nudSmoothness.Value; } }
        public float RSmoothnessFactor02 { get { return (float)nudRGrain02.Value; } }
        public float RSmoothnessFactor03 { get { return (float)nudRGrain03.Value; } }
        public float RSmoothnessFactor04 { get { return (float)nudRGrain04.Value; } }
        public float RAmp01 { get { return 0.5f; } }
        public float RAmp02 { get { return (float)nudRAmp02.Value; } }
        public float RAmp03 { get { return (float)nudRAmp03.Value; } }
        public float RAmp04 { get { return (float)nudRAmp04.Value; } }

        public float RainfallBias { get { return (float)nudRBias.Value; } }

        public bool DrawBiomes { get { return this.rbDrawBiomes.Checked; } }
        public bool DrawElevation { get { return this.rbDrawElevation.Checked; } }
        public bool DrawTemperature { get { return this.rbDrawTemperature.Checked; } }
        public bool DrawRainfall { get { return this.rbDrawRainfall.Checked; } }

        public MapSettings()
        {
            InitializeComponent();
        }

        private void MapSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Visible = false;
            e.Cancel = true;
        }
    }
}
