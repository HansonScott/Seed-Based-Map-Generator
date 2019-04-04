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
            // grey gradient
            //int v = (int)(Elevation * 255 / Map.maxElevation);
            //Color c = Color.FromArgb(v, v, v);
            //BrushColor = new SolidBrush(c);

            // land vs water
            if (Elevation < Map.waterElevation)
            {
                BrushColor = new SolidBrush(Color.Blue);
            }
            else
            {
                BrushColor = new SolidBrush(Color.Tan);
            }
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