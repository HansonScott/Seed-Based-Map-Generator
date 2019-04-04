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
            // land vs water
            if (Elevation < ParentMap.WaterElevation)
            {
                BrushColor = new SolidBrush(Color.Blue);
            }
            else
            {
                // green between 100 - 255),
                // r/g between 0 - 255;
                // green gradient
                int v = (int)(Elevation * 200 / Map.maxElevation);
                Color c = Color.FromArgb(v, 255, v);
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