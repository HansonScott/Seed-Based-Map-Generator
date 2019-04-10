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

        private float _Temperature;
        public Color TemperatureColor;
        private float _Moisture;
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
        public float Moisture
        {
            get { return _Moisture; }
            set
            {

                _Moisture = value;
            }
        }
        public bool IsRiver
        {
            get { return _IsRiver; }
            set
            {
                _IsRiver = value;
                SetBrushByElevationAndType();
            }
        }
        public bool IsLake
        {
            get { return _IsLake; }
            set
            {
                _IsLake = value;
                SetBrushByElevationAndType();
            }
        }
        public float ActualElevation { get { return (Elevation * ParentMap.MaxElevation); } }
        public float ActualTemperature { get { return (Temperature * ParentMap.MaxTemperature); } }
        public float ActualMoisture { get { return (Moisture * ParentMap.MaxMoisture); } }
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
        public void SetBrushByElevationAndType()
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
        }

        internal void PaintElevation(Graphics g)
        {
            g.FillRectangle(ElevationBrush, ThisRect);
        }

        internal void PaintTemp(Graphics g)
        {
            g.FillRectangle(TemperatureBrush, ThisRect);
        }

        internal string[] GetInfo()
        {
            List<string> info = new List<string>();

            // add all interesting info
            info.Add("Elevation: ");
            info.Add(((int)ActualElevation).ToString());
            info.Add("Temperature: ");
            info.Add(((int)ActualTemperature).ToString());

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
