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

            // now smooth it out.
            //for (int x = 0; x < Cells.Length; x++)
            //{
            //    for (int y = 0; y < Cells[x].Length; y++)
            //    {
            //        Cells[x][y].Elevation = GetAverageForAllSurroundingCells(x, y);
            //    }
            //}

            // learned that doing it multiple times is much more effective, to round out the edges
            ApplyPerlianSmoothing(SmoothnessFactor);
            ApplyPerlianSmoothing((SmoothnessFactor * 0.8));
            ApplyPerlianSmoothing((SmoothnessFactor * 0.5));
            ApplyPerlianSmoothing((SmoothnessFactor * 0.3));
        }

        private void ApplyPerlianSmoothing(double granularity)
        {
            double samplePeriod = Math.Pow(2, granularity); // calculates 2 ^ k
            float sampleFrequency = 1.0f / (float)samplePeriod;

            for (int x = 0; x < Cells.Length; x++)
            {
                //calculate the horizontal sampling indices
                float sample_i0 = (float)((int)(x / samplePeriod) * samplePeriod);
                float sample_i1 = (float)((sample_i0 + samplePeriod) % Cells.Length); //wrap around
                float horizontal_blend = (x - sample_i0) * sampleFrequency;

                for (int y = 0; y < Cells[x].Length; y++)
                {
                    //calculate the vertical sampling indices
                    double sample_j0 = (int)(y / samplePeriod) * samplePeriod;
                    float sample_j1 = (float)((sample_j0 + samplePeriod) % Cells[x].Length); //wrap around
                    double vertical_blend = (y - sample_j0) * sampleFrequency;

                    //blend the top two corners
                    double top = Interpolate(Cells[(int)sample_i0][(int)sample_j0].Elevation,
                       Cells[(int)sample_i1][(int)sample_j0].Elevation, horizontal_blend);

                    //blend the bottom two corners
                    double bottom = Interpolate(Cells[(int)sample_i0][(int)sample_j1].Elevation,
                       Cells[(int)sample_i1][(int)sample_j1].Elevation, horizontal_blend);

                    //final blend
                    Cells[x][y].Elevation = (int)Interpolate(top, bottom, vertical_blend);
                }
            }
        }

        double Interpolate(double x0, double x1, double alpha)
        {
            return x0 * (1 - alpha) + alpha * x1;
        }


        private int GetAverageForAllSurroundingCells(int x, int y)
        {
            int rangeToAverage = 5;
            int count = 0;
            int total = 0;
            for(int row = (Math.Max(0, x - rangeToAverage)); row < Math.Min(x + rangeToAverage, Cells.Length); row++)
            {
                for(int col = (Math.Max(0, y - rangeToAverage)); col < Math.Min(y + rangeToAverage, Cells[row].Length); col++)
                {
                    total += Cells[row][col].Elevation;
                    count++;
                }
            }

            return (int)(total / count);
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
