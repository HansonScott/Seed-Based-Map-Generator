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

        private SolidBrush b;

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
        public void SetBrushByElevationAndType()
        {
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
                b = new SolidBrush(LakeColor);
            }
            else if (this.IsRiver)
            {
                b = new SolidBrush(RiverColor);
            }
            else
            {
                b = new SolidBrush(ElevationColor);
            }
        }
        internal void PaintCell(Graphics g)
        {
            g.FillRectangle(b, ThisRect);
        }

        internal string[] GetInfo()
        {
            List<string> info = new List<string>();

            // add all interesting info
            info.Add("Elevation: ");
            info.Add(ActualElevation.ToString());
            if(IsRiver)
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
