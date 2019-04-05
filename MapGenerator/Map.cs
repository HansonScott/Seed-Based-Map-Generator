using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGenerator
{
    public class Map
    {
        private int seed;
        private Rectangle size;

        private Random RandLvl1;
        private List<Random> RandLvl2;
        private Cell[][] Cells;

        private enum DetailedRandomizer
        {
            ColorRand = 0,
            ElevationRand = 1,
        }

        Color backColor;

        #region Parameters
        public int maxElevation = 1000;
        public int WaterElevation = 500;

        public double SmoothnessFactor = 0.5;

        #endregion

        #region Constructor and Setup
        public Map(int seed, Rectangle size)
        {
            this.seed = seed;

            // adjust size on the Y axis for the pseudo perlin noise, as it chops up when it gets at the bottom
            size.Height = (int)(size.Height * 1.1);
            this.size = size;
            BuildCells();

            RandLvl1 = new Random(seed); // set the parent randomizer
            PopulateRandLvl2();
        }
        private void BuildCells()
        {
            Cells = new Cell[size.Width][];
            for (int x = 0; x < Cells.Length; x++)
            {
                Cells[x] = new Cell[size.Height];
                for(int y = 0; y < Cells[x].Length; y++)
                {
                    Cells[x][y] = new Cell(this, x, y);
                }
            }
        }
        private void PopulateRandLvl2()
        {
            int[] detailedRandomizers = (int[])Enum.GetValues(typeof(DetailedRandomizer));
            RandLvl2 = new List<Random>(detailedRandomizers.Length);
            foreach (int i in detailedRandomizers)
            {
                RandLvl2.Add(new Random(RandLvl1.Next()));
            }
        }
        #endregion

        public void GenerateMap()
        {
            BuildMapDetails();
        }

        private void BuildMapDetails()
        {
            SetElevation();
        }

        private void SetElevation()
        {
            Random eRand = RandLvl2[(int)DetailedRandomizer.ElevationRand];

            // start with entirely random elevation
            for(int x = 0; x < Cells.Length; x++)
            {
                for(int y = 0; y < Cells[x].Length; y++)
                {
                    Cells[x][y].Elevation = eRand.Next(maxElevation);
                }
            }

            // learned that doing it multiple times is much more effective, to round out the edges using different granularity
            ApplyPseudoPerlinSmoothing(SmoothnessFactor);
            ApplyPseudoPerlinSmoothing((SmoothnessFactor * 0.8));
            ApplyPseudoPerlinSmoothing((SmoothnessFactor * 0.5));
            ApplyPseudoPerlinSmoothing((SmoothnessFactor * 0.15));
        }

        private void ApplyPseudoPerlinSmoothing(double granularity)
        {
            //float sampleSize = (float)Math.Pow(2, granularity); // calculates 2 ^ k
            float sampleSize = (float)granularity;
            float sampleFrequency = 1.0f / sampleSize;
            float x0,x1,y0,y1; // sample nodes to hit
            float h_blend, v_blend;
            double top, bottom;

            for (int x = 0; x < Cells.Length; x++) // for each column
            {
                x0 = ((int)(x / sampleSize) * sampleSize);
                x1 = ((x0 + sampleSize) % Cells.Length); //wrap around if we flow over
                h_blend = (x - x0) * sampleFrequency;

                for (int y = 0; y < Cells[x].Length; y++) // for each row in the X'th column
                {
                    //calculate the vertical sampling indices
                    y0 = ((int)(y / sampleSize) * sampleSize);
                    y1 = ((y0 + sampleSize) % Cells[x].Length); //wrap around
                    v_blend = (y - y0) * sampleFrequency;

                    //blend the top two corners
                    top = Interpolate(Cells[(int)x0][(int)y0].Elevation,
                       Cells[(int)x1][(int)y0].Elevation, h_blend);

                    //blend the bottom two corners
                    bottom = Interpolate(Cells[(int)x0][(int)y1].Elevation,
                       Cells[(int)x1][(int)y1].Elevation, h_blend);

                    //final blend
                    Cells[x][y].Elevation = (int)Interpolate(top, bottom, v_blend);
                }
            }
        }

        double Interpolate(double x0, double x1, double alpha)
        {
            return x0 * (1 - alpha) + alpha * x1;
        }
        
        private void SetBackColor()
        {
            Random randColor = RandLvl2[(int)DetailedRandomizer.ColorRand];
            int r = randColor.Next(0, 255);
            int g = randColor.Next(0, 255);
            int b = randColor.Next(0, 255);
            backColor = Color.FromArgb(r, g, b);
        }

        internal void PaintMap(Graphics g)
        {
            foreach(Cell[] rows in Cells)
            {
                foreach(Cell c in rows)
                {
                    c.PaintCell(g);
                }
            }
        }
    }
}
