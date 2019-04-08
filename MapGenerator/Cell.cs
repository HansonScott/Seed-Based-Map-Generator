using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapGenerator
{
    public class Cell
    {
        #region Static Resources
        private static Dictionary<float, SolidBrush> ElevationBrushes;
        public static void ClearElevationBrushes() { ElevationBrushes?.Clear(); }
        #endregion

        #region Fields
        Map ParentMap;
        public int X;
        public int Y;
        public bool IsSample;
        private float _Elevation;
        public SolidBrush ElevationBrushColor;
        public SolidBrush RiverBrushColor = new SolidBrush(Color.FromArgb(50, 50, 255));
        public SolidBrush LakeBrushColor = new SolidBrush(Color.FromArgb(50, 50, 255));
        public RectangleF ThisRect;

        public bool IsRiver;
        public bool IsLake;
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

        public float ActualElevation { get { return (Elevation * ParentMap.maxElevation); } }
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
        public void SetBrushByElevation()
        {
            if(ElevationBrushes == null)
            {
                ElevationBrushes = new Dictionary<float, SolidBrush>();
            }

            if(ElevationBrushes.ContainsKey((float)Math.Round((decimal)Elevation, 3)))
            {
                this.ElevationBrushColor = ElevationBrushes[(float)Math.Round((decimal)Elevation, 3)];
                return;
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
                    Color c = Color.FromArgb(0, Math.Max(Math.Min(v, 255), 0), b);
                    ElevationBrushColor = new SolidBrush(c);
                }
                // within 10% of water elevation
                else if (ActualElevation < (ParentMap.WaterElevation * 1.1))
                {
                    ElevationBrushColor = new SolidBrush(Color.Tan);
                }
                else // above water
                {
                    // at waterElevation: 0,140,0 // light green
                    // at maxElevation: 255,255,255 // white
                    // green gradient
                    //int v = (int)(Elevation * 140 / ParentMap.maxElevation);
                    int v = (int)(Elevation * 140);
                    Color c = Color.FromArgb(v,
                                            (Math.Max(((255 - v) / 2) + v, 140)), 
                                            v);

                    ElevationBrushColor = new SolidBrush(c);
                }

                ElevationBrushes.Add((float)Math.Round((decimal)Elevation, 3), ElevationBrushColor);
            } // end else, not in static collection
        }
        internal void PaintCell(Graphics g)
        {
            // capture if this is a sample pixel, and overwrite the dynamic color for a tracer color - for testing only
            if (this.IsLake)
            {
                g.FillRectangle(LakeBrushColor, ThisRect);
            }
            else if(this.IsRiver)
            {
                g.FillRectangle(RiverBrushColor, ThisRect);
            }
            else 
            {
                g.FillRectangle(ElevationBrushColor, ThisRect);
            }
        }

        internal string[] GetInfo()
        {
            List<string> info = new List<string>();

            // add all interesting info
            info.Add($"Elevation: {Elevation}");
            if(IsRiver)
            {
                info.Add("River");
            }

            return info.ToArray();
        }
        #endregion

        #region Private Functions
        #endregion
    }
}
