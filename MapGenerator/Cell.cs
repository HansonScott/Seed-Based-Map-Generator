using System;
using System.Drawing;

namespace MapGenerator
{
    public class Cell
    {
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
            int v = (int)(Elevation * 255 / Map.maxElevation);
            Color c = Color.FromArgb(v, v, v);
            BrushColor = new SolidBrush(c);
        }

        public Brush BrushColor;
        public RectangleF ThisRect;

        public Cell(int X, int Y)
        {
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