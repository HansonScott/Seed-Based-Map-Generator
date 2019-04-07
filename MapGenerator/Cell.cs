using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapGenerator
{
    public class Cell
    {
        #region Static Resources
        private static Dictionary<float, SolidBrush> ElevationBrushes;
        #endregion

        #region Fields
        Map ParentMap;
        public int X;
        public int Y;
        public bool IsSample;
        private float _Elevation;
        public SolidBrush BrushColor;
        //public SolidBrush TracerBrushColor = new SolidBrush(Color.Blue);
        public RectangleF ThisRect;
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

            if(ElevationBrushes.ContainsKey(Elevation))
            {
                this.BrushColor = ElevationBrushes[Elevation];
                return;
            }
            else
            {
                // grey
                //int v = (int)(Elevation * 255 / ParentMap.maxElevation);
                //int v = (int)(Elevation * 255); // assume value to be between 0 and 1
                //Color c = Color.FromArgb(v, v, v);
                //BrushColor = new SolidBrush(c);

                double actualElevation = (Elevation * ParentMap.maxElevation);

                // below water
                if (actualElevation < ParentMap.WaterElevation)
                {
                    // at 0 elevation: 0,0,255
                    // at waterElevation: 0,100,255

                    //int v = (int)(Elevation * 100 / ParentMap.WaterElevation);
                    int v = (int)(Elevation * 100);
                    Color c = Color.FromArgb(0, v, 255);
                    BrushColor = new SolidBrush(c);
                }
                // within 10% of water elevation
                else if (actualElevation < (ParentMap.WaterElevation * 1.1))
                {
                    BrushColor = new SolidBrush(Color.Tan);
                }
                else // above water
                {
                    // at waterElevation: 0,140,0
                    // at maxElevation: 255,255,255
                    // green gradient
                    //int v = (int)(Elevation * 140 / ParentMap.maxElevation);
                    int v = (int)(Elevation * 140);
                    Color c = Color.FromArgb(v, (Math.Max(((255 - v) / 2) + v, 140)), v);
                    BrushColor = new SolidBrush(c);
                }

                ElevationBrushes.Add(Elevation, BrushColor);
            } // end else, not in static collection
        }
        internal void PaintCell(Graphics g)
        {
            // capture if this is a sample pixel, and overwrite the dynamic color for a tracer color - for testing only
            //if (this.IsSample)
            //{
            //    g.FillRectangle(TracerBrushColor, ThisRect);
            //}
            //else
            //{
                g.FillRectangle(BrushColor, ThisRect);
            //}
        }
        #endregion

        #region Private Functions
        #endregion
    }
}
