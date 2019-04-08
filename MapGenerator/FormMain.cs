using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapGenerator
{
    public partial class FormMain : Form
    {
        #region Main
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
        #endregion

        public Map CurrentMap;
        public MapSettings Settings;
        public FormMain()
        {
            InitializeComponent();

            Settings = new MapSettings();
        }

        private void pMap_Paint(object sender, PaintEventArgs e)
        {
            CurrentMap?.PaintMap(e.Graphics);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Output("Generating map...");
            int seed = GetSeedFromTextBox();
            CurrentMap = new Map(seed, pMap.ClientRectangle);

            // apply any parameters before generating...
            ApplySettings(CurrentMap, Settings);

            CurrentMap.OnLog += CurrentMap_OnLog;

            CurrentMap.GenerateMap();

            Output("Generating map done, drawing...");
            this.pMap.Refresh();
            Output("Drawing map done");
            Cursor.Current = Cursors.Default;
        }

        private void CurrentMap_OnLog(object sender, Map.MapLoggingEventArgs e)
        {
            Output(e.Log.Message);
        }

        private void ApplySettings(Map m, MapSettings s)
        {
            #region Elevation
            m.WaterElevation = s.WaterLevel;

            m.SmoothnessFactor = s.Smoothness;
            m.AddSmooth01 = true; // hard coded, must have at least 1 smoothing
            m.AddSmooth02 = s.AddSmooth02;
            m.AddSmooth03 = s.AddSmooth03;
            m.AddSmooth04 = s.AddSmooth04;

            m.SmoothnessFactor = s.SmoothnessFactor;
            m.SmoothnessFactor02 = s.SmoothnessFactor02;
            m.SmoothnessFactor03 = s.SmoothnessFactor03;
            m.SmoothnessFactor04 = s.SmoothnessFactor04;

            m.Amp01 = s.Amp01;
            m.Amp02 = s.Amp02;
            m.Amp03 = s.Amp03;
            m.Amp04 = s.Amp04;

            m.ContinentBias = s.ContinentBias;
            #endregion

        }

        private int GetSeedFromTextBox()
        {
            int result = 0;
            if(!Int32.TryParse(tbSeed.Text, out result))
            {
                Random r = new Random();
                result = r.Next();
                lblSeed.Text = "Seed: " + result.ToString();
                tbSeed.Text = result.ToString();
            }
            return result;
        }
        private void Output(string msg)
        {
            tbOutput.Text = msg;
            tbOutput.Refresh();
            Thread.Sleep(10);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Settings.Show(this);
        }

        private void pMap_MouseEnter(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Cross;
        }

        private void pMap_MouseLeave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;
        }

        private void pMap_MouseMove(object sender, MouseEventArgs e)
        {
            if(CurrentMap == null) { return; }

            string[] info = CurrentMap.GetInfoAt(e.X, e.Y);
            string result = string.Empty;
            foreach(string i in info)
            {
                if(result.Length > 0)
                {
                    result += Environment.NewLine;
                }

                result += i;
            }

            tbInfo.Text = result;
            tbInfo.Refresh();
            Thread.Sleep(10);
        }
    }
}
