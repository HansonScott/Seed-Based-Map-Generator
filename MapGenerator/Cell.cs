using System;
using System.Drawing;

namespace MapGenerator
{
    public class Cell
    {
        Map ParentMap;
        public int X;
        public int Y;
        private int _Elevation;
        public int Elevation
        {
            get { return _Elevation; }
            set
            {
                _Elevation = value;
                SetBrushByElevation();
            }
        }

        private void SetBrushByElevation()
        {
            // grey
            //int v = (int)(Elevation * 255 / Map.maxElevation);
            //Color c = Color.FromArgb(v, v, v);
            //BrushColor = new SolidBrush(c);

            // land vs water
            if (Elevation < ParentMap.WaterElevation)
            {
                // at 0 elevation: 0,0,255
                // at waterElevation: 0,100,255

                int v = (int)(Elevation * 100 / ParentMap.WaterElevation);
                Color c = Color.FromArgb(0, v, 255);
                BrushColor = new SolidBrush(c);
            }
            else
            {
                // at waterElevation: 0,140,0
                // at maxElevation: 255,255,255
                // green gradient
                int v = (int)(Elevation * 140 / ParentMap.maxElevation);
                Color c = Color.FromArgb(v, (Math.Max(((255-v)/2) + v,140)), v);
                BrushColor = new SolidBrush(c);
            }
        }

        public Brush BrushColor;
        public RectangleF ThisRect;

        public Cell(Map ParentMap, int X, int Y)
        {
            this.ParentMap = ParentMap;
            this.X = X;
            this.Y = Y;
            ThisRect = new RectangleF(X, Y, 1, 1);
        }

        internal void PaintCell(Graphics g)
        {
            g.FillRectangle(BrushColor, ThisRect);
        }
    }
}