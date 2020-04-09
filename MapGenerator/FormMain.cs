using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

        public enum SessionStates
        {
            Waiting = 0,
            GeneratingMap = 1,
        }
        public SessionStates CurrentState;
        public Map CurrentMap;
        public MapSettings Settings;

        public FormMain()
        {
            InitializeComponent();

            Settings = new MapSettings();
            CurrentState = SessionStates.Waiting;
        }

        private void pMap_Paint(object sender, PaintEventArgs e)
        {
            CurrentMap?.PaintMap(e.Graphics);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if(CurrentState == SessionStates.GeneratingMap) { return; }
            CurrentState = SessionStates.GeneratingMap;

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
            CurrentState = SessionStates.Waiting;
        }

        private void CurrentMap_OnLog(object sender, Map.MapLoggingEventArgs e)
        {
            Output(e.Log.Message);
        }

        private void ApplySettings(Map m, MapSettings s)
        {
            if(m == null) { return; }

            m.DrawBiomes = s.DrawBiomes;
            m.DrawElevation = s.DrawElevation;
            m.DrawTemperature = s.DrawTemperature;
            m.DrawRainfall = s.DrawRainfall;

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
            m.RiverBias = s.RiverBias;
            m.RiverSourceElevationMinimum = s.RiverSourceElevationMin;
            m.LakeSize = s.LakeSize;
            #endregion

            #region Temparature
            m.TSmoothnessFactor = s.Smoothness;
            m.AddTSmooth01 = true; // hard coded, must have at least 1 smoothing
            m.AddTSmooth02 = s.AddTSmooth02;
            m.AddTSmooth03 = s.AddTSmooth03;
            m.AddTSmooth04 = s.AddTSmooth04;

            m.TSmoothnessFactor = s.TSmoothnessFactor;
            m.TSmoothnessFactor02 = s.TSmoothnessFactor02;
            m.TSmoothnessFactor03 = s.TSmoothnessFactor03;
            m.TSmoothnessFactor04 = s.TSmoothnessFactor04;

            m.TAmp01 = s.TAmp01;
            m.TAmp02 = s.TAmp02;
            m.TAmp03 = s.TAmp03;
            m.TAmp04 = s.TAmp04;

            m.PolarBias = s.PolarBias;
            #endregion

            #region Rainfall
            m.RSmoothnessFactor = s.Smoothness;
            m.AddRSmooth01 = true; // hard coded, must have at least 1 smoothing
            m.AddRSmooth02 = s.AddRGrain02;
            m.AddRSmooth03 = s.AddRGrain03;
            m.AddRSmooth04 = s.AddRGrain04;

            m.RSmoothnessFactor = s.SmoothnessFactor;
            m.RSmoothnessFactor02 = s.RSmoothnessFactor02;
            m.RSmoothnessFactor03 = s.RSmoothnessFactor03;
            m.RSmoothnessFactor04 = s.RSmoothnessFactor04;

            m.RAmp01 = s.RAmp01;
            m.RAmp02 = s.RAmp02;
            m.RAmp03 = s.RAmp03;
            m.RAmp04 = s.RAmp04;

            m.RainfallBias = s.RainfallBias;
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
            tbOutput.AppendText(Environment.NewLine);
            tbOutput.AppendText(msg);
            tbOutput.Refresh();
            Thread.Sleep(10);
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Settings = new MapSettings();
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

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Image Files | *.bmp";
            sfd.DefaultExt = "bmp";

            string folder = Application.StartupPath + Path.DirectorySeparatorChar + "Images";
            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            sfd.InitialDirectory = folder;
            sfd.FileName = GetSeedFromTextBox().ToString() + ".bmp";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string filename = sfd.FileName;
                Bitmap b = new Bitmap(pMap.Width, pMap.Height);
                this.pMap.DrawToBitmap(b, pMap.DisplayRectangle);
                b.Save(filename);
            }
        }

        private void btnRedraw_Click(object sender, EventArgs e)
        {
            ApplySettings(CurrentMap, Settings);
            pMap.Refresh();
        }
    }
}
