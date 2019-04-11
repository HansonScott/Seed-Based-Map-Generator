using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapGenerator
{
    public class Cell
    {
        #region Static Resources
        private static Dictionary<float, Color> ElevationColors;
        public static void ClearElevationBrushes() { ElevationColors?.Clear(); }
        private static Dictionary<float, Color> TemperatureColors;
        public static void ClearTemperatureBrushes() { TemperatureColors?.Clear(); }
        private static Dictionary<float, Color> RainfallColors;
        public static void ClearRainfallBrushes() { RainfallColors?.Clear(); }
        #endregion

        #region Fields
        Map ParentMap;
        public int X;
        public int Y;
        public bool IsSample;
        private float _Elevation;
        public Color ElevationColor;
        public Color RiverColor = Color.FromArgb(50, 50, 255);
        public Color LakeColor = Color.FromArgb(50, 50, 255);
        public RectangleF ThisRect;

        private bool _IsRiver;
        private bool _IsLake;

        private SolidBrush ElevationBrush;
        private SolidBrush TemperatureBrush;
        private SolidBrush RainfallBrush;

        private float _Temperature;
        public Color TemperatureColor;

        private float _Rainfall;
        public Color RainfallColor;
        #endregion

        #region Properties
        public float Elevation
        {
            get { return _Elevation; }
            set
            {
                _Elevation = value;
            }
        }
        public float Temperature
        {
            get { return _Temperature; }
            set
            {

                _Temperature = value;
            }
        }
        public float Rainfall
        {
            get { return _Rainfall; }
            set
            {

                _Rainfall = value;
            }
        }
        public bool IsRiver
        {
            get { return _IsRiver; }
            set
            {
                _IsRiver = value;
                SetBrushByEnvironment();
            }
        }
        public bool IsLake
        {
            get { return _IsLake; }
            set
            {
                _IsLake = value;
                SetBrushByEnvironment();
            }
        }
        public float ActualElevation { get { return (Elevation * ParentMap.MaxElevation); } }
        public float ActualTemperature { get { return (Temperature * ParentMap.MaxTemperature); } }
        public float ActualRainfall { get { return (Rainfall * ParentMap.MaxRainfall); } }
        #endregion

        #region Constructor and Setup
        public Cell(Map ParentMap, int X, int Y)
        {
            this.ParentMap = ParentMap;
            this.X = X;
            this.Y = Y;
            ThisRect = new RectangleF(X, Y, 1, 1);
        }
        #endregion

        #region Public Methods
        public void SetBrushByEnvironment()
        {
            #region elevation
            if(ElevationColors == null)
            {
                ElevationColors = new Dictionary<float, Color>();
            }

            if(ElevationColors.ContainsKey((float)Math.Round((decimal)Elevation, 3)))
            {
                this.ElevationColor = ElevationColors[(float)Math.Round((decimal)Elevation, 3)];
            }
            else
            {
                // grey
                //int v = (int)(Elevation * 255 / ParentMap.maxElevation);
                //int v = (int)(Elevation * 255); // assume value to be between 0 and 1
                //Color c = Color.FromArgb(v, v, v);
                //BrushColor = new SolidBrush(c);

                // below water
                if (ActualElevation < ParentMap.WaterElevation)
                {
                    // at 0 elevation: 0,0,255
                    // at waterElevation: 0,100,255

                    //int v = (int)(Elevation * 100 / ParentMap.WaterElevation);
                    int v = (int)(Elevation * 100);
                    int b = (int)((ActualElevation / ParentMap.WaterElevation) * 125) + 130;
                    ElevationColor = Color.FromArgb(0, Math.Max(Math.Min(v, 255), 0), b);
                }
                // within 10% of water elevation
                else if (ActualElevation < (ParentMap.WaterElevation * 1.1))
                {
                    ElevationColor = Color.Tan;
                }
                else // above water
                {
                    // at waterElevation: 0,140,0 // light green
                    // at maxElevation: 255,255,255 // white
                    // green gradient
                    //int v = (int)(Elevation * 140 / ParentMap.maxElevation);
                    int v = (int)(Elevation * 140);
                    ElevationColor = Color.FromArgb(v,
                                                    (Math.Max(((255 - v) / 2) + v, 140)), 
                                                    v);
                }

                ElevationColors.Add((float)Math.Round((decimal)Elevation, 3), ElevationColor);
            } // end else, not in static collection

            // now set the actual drawing brush
            if (this.IsLake)
            {
                ElevationBrush = new SolidBrush(LakeColor);
            }
            else if (this.IsRiver)
            {
                ElevationBrush = new SolidBrush(RiverColor);
            }
            else
            {
                ElevationBrush = new SolidBrush(ElevationColor);
            }
            #endregion

            #region Temperature
            if (TemperatureColors == null)
            {
                TemperatureColors = new Dictionary<float, Color>();
            }

            if (TemperatureColors.ContainsKey((float)Math.Round((decimal)Temperature, 3)))
            {
                this.TemperatureColor = TemperatureColors[(float)Math.Round((decimal)Temperature, 3)];
            }
            else
            {
                // at max temp: 255, 255,  - white
                // at 0 temp: 0, 0, 0  - black

                int b = (int)(Temperature * 255);
                int v = Math.Max(Math.Min(b, 255), 0);
                TemperatureColor = Color.FromArgb(v, v, v);

                TemperatureColors.Add((float)Math.Round((decimal)Temperature, 3), TemperatureColor);
            } // end else, not in static collection

            TemperatureBrush = new SolidBrush(TemperatureColor);
            #endregion

            #region Rainfall
            if (RainfallColors == null)
            {
                RainfallColors = new Dictionary<float, Color>();
            }

            if (RainfallColors.ContainsKey((float)Math.Round((decimal)Rainfall, 3)))
            {
                this.RainfallColor = RainfallColors[(float)Math.Round((decimal)Rainfall, 3)];
            }
            else
            {
                // Maroon       128,0,  0   0-10%
                // red          255,0,  0   10-20%
                // orange       255,128,0   20-30%
                // yellow       255,255,0   30-40%
                // lime         128,255,0   40-50%
                // green        0,  255,0   50-60%
                // teal         0,255,128   60-70%
                // aqua         0,255,255   70-80%
                // light blue   0,128,255   80-90%
                // dark blue    0,  0,255   90-100%

                float perc = ActualRainfall / ParentMap.MaxRainfall;
                int r, g, b;
                if (perc < .10)      { r = 128; g = 0; b = 0; }
                else if (perc < .20) { r = 255; g = 0; b = 0; }
                else if (perc < .30) { r = 255; g = 128; b = 0; }
                else if (perc < .40) { r = 255; g = 255; b = 0; }
                else if (perc < .50) { r = 128; g = 255; b = 0; }
                else if (perc < .60) { r = 0; g = 255; b = 0; }
                else if (perc < .70) { r = 0; g = 255; b = 128; }
                else if (perc < .80) { r = 0; g = 255; b = 255; }
                else if (perc < .90) { r = 0; g = 128; b = 255; }
                else                 { r = 0; g = 0; b = 255; }

                RainfallColor = Color.FromArgb(r, g, b);

                RainfallColors.Add((float)Math.Round((decimal)Rainfall, 3), RainfallColor);
            } // end else, not in static collection

            RainfallBrush = new SolidBrush(RainfallColor);
            #endregion
        }

        internal void PaintElevation(Graphics g)
        {
            g.FillRectangle(ElevationBrush, ThisRect);
        }

        internal void PaintTemp(Graphics g)
        {
            g.FillRectangle(TemperatureBrush, ThisRect);
        }

        internal void PainRainfall(Graphics g)
        {
            g.FillRectangle(RainfallBrush, ThisRect);
        }

        internal string[] GetInfo()
        {
            List<string> info = new List<string>();

            // add all interesting info
            info.Add("Elevation: ");
            info.Add(((int)ActualElevation).ToString());
            info.Add("Temperature: ");
            info.Add(((int)ActualTemperature).ToString());
            info.Add("Rainfall: ");
            info.Add(((int)ActualRainfall).ToString());

            if (IsRiver)
            {
                info.Add("River");
            }
            else if(IsLake)
            {
                info.Add("Lake");
            }
            else if(ActualElevation <= ParentMap.WaterElevation)
            {
                info.Add("Water");
            }

            return info.ToArray();
        }
        #endregion

        #region Private Functions
        #endregion
    }
}
