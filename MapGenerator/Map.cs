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
            size.Height = (int)(size.Height * 1.2);
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

            // learned that doing it multiple times is the right way, to round out the edges using different granularity
            Cell[][] Cells1 = ApplyPseudoPerlinSmoothing(Cells, SmoothnessFactor, 1.0);
            Cell[][] Cells2 = ApplyPseudoPerlinSmoothing(Cells, (SmoothnessFactor * 0.8), 0.7);
            Cell[][] Cells3 = ApplyPseudoPerlinSmoothing(Cells, (SmoothnessFactor * 0.5), 0.4);
            //Cell[][] Cells4 = ApplyPseudoPerlinSmoothing(Cells, (SmoothnessFactor * 0.15), 0.1);

            // now add them all together.
            double result = 0;
            List<double> vals = new List<double>();
            for (int x = 0; x < Cells.Length; x++)
            {
                for (int y = 0; y < Cells[x].Length; y++)
                {
                    //result = 0.5; // start at the middle
                    result = Cells1[x][y].Elevation; // start with our first value

                    vals.Clear();
                    //vals.Add(Cells1[x][y].Elevation);
                    vals.Add(Cells2[x][y].Elevation);
                    vals.Add(Cells3[x][y].Elevation);
                    //vals.Add(Cells4[x][y].Elevation);

                    foreach (double v in vals)
                    {
                        // v is between 0 and 1, with the +- 0.5 representing the % movement away from flat.
                        // so, pulling out the 0.5 gives us a +- random amplitude for the pass
                        // so, multiplying the result will nudge the result up or down by betweeen zero and +- the amplitude for the pass.
                        result = result * (1 - (v - 0.5));
                    }

                    Cells[x][y].Elevation = result;

                    // now that we're all done calculating, set up for drawing.
                    Cells[x][y].SetBrushByElevation();
                }
            }

        }

        private Cell[][] ApplyPseudoPerlinSmoothing(Cell[][] Cells, double granularity, double amplitude)
        {
            Cell[][] results = Copy(Cells);            

            //float sampleSize = (float)Math.Pow(2, granularity); // calculates 2 ^ k
            float sampleSize = (float)granularity;
            float sampleFrequency = 1.0f / sampleSize;
            float x0,x1,y0,y1; // sample nodes to hit
            float h_blend, v_blend;
            double top, bottom;

            // set the random value for each sell, adjusting for the possible amplitude allowed in this pass.
            Random eRand = RandLvl2[(int)DetailedRandomizer.ElevationRand];
            for (int x = 0; x < results.Length; x++) // for each column
            {
                for (int y = 0; y < results[x].Length; y++) // for each row in the X'th column
                {
                    double v = (eRand.NextDouble() * amplitude) + (0.5 - amplitude / 2);
                    results[x][y].Elevation = v; // results in +/- amplitude.
                }
            }

            for (int x = 0; x < results.Length; x++) // for each column
            {
                // calculate the horizontal sampling indicies
                x0 = ((int)(x / sampleSize) * sampleSize);
                x1 = ((x0 + sampleSize) % Cells.Length); //wrap around if we flow over
                h_blend = (x - x0) * sampleFrequency;

                for (int y = 0; y < results[x].Length; y++) // for each row in the X'th column
                {
                    //calculate the vertical sampling indices
                    y0 = ((int)(y / sampleSize) * sampleSize);
                    y1 = ((y0 + sampleSize) % results[x].Length); //wrap around
                    v_blend = (y - y0) * sampleFrequency;

                    if(h_blend == 0 && v_blend == 0) { results[x][y].IsSample = true; }
                    
                    //blend the top two corners
                    top = Interpolate(results[(int)x0][(int)y0].Elevation,
                       results[(int)x1][(int)y0].Elevation, h_blend);

                    //blend the bottom two corners
                    bottom = Interpolate(results[(int)x0][(int)y1].Elevation,
                       results[(int)x1][(int)y1].Elevation, h_blend);

                    //final blend
                    results[x][y].Elevation = Interpolate(top, bottom, v_blend);
                }
            }

            return results;
        }

        private Cell[][] Copy(Cell[][] cells)
        {
            Cell[][] results = new Cell[Cells.Length][];
            for (int x = 0; x < Cells.Length; x++)
            {
                results[x] = new Cell[Cells[x].Length];
                for(int y = 0; y < Cells[x].Length; y++)
                {
                    results[x][y] = new Cell(this, Cells[x][y].X, Cells[x][y].Y);

                    // NOTE: don't propigate the random values, let each pass figure it out.
                    //results[x][y].Elevation = Cells[x][y].Elevation;
                }
            }

            return results;
        }

        double Interpolate(double x0, double x1, double alpha)
        {
            //return InterpolateLinear(x0, x1, alpha);
            return InterpolatePoly(x0, x1, alpha);
        }
        double InterpolateLinear(double x0, double x1, double alpha)
        {
            // this looks like a sort of dot product calculation, but certainly linear
            return x0 * (1 - alpha) + alpha * x1;
        }
        double InterpolatePoly(double x0, double x1, double t)
        {
            // first, figure out how much weight we should give to the movement along the curve from X0 to X1
            //6t^5-15t^4+10t^3
            //double y = t * t * t * (t * (6 * t - 15) + 10); // NOTE: y is between 0 and 1, given that t is between 0 and 1

            // 3t^2 - 2t^3 - gives a more rounded look to the first perlin smoothing pass than the 5th order, above.
            double y = t * t * (3 - 2 * t);

            // then apply the weight to the values X0, and X1
            double diff = x1 - x0;

            double partialDiff = diff * y; // determine how far we shoudl move along the difference.
            
            //and lastly, adjust the partial diff for where we started.
            double adjustedPartialDiff = partialDiff + x0;
            
            return adjustedPartialDiff;
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
