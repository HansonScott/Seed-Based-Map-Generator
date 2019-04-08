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
        #region Custom Logging Events
        public delegate void LoggingHandler(object sender, MapLoggingEventArgs e);
        public event LoggingHandler OnLog;

        internal void RaiseLog(string message)
        {
            RaiseLog(new MapLog(message));
        }
        internal void RaiseLog(MapLog log)
        {
            // using this inline vs checking for null is more thread safe
            OnLog?.Invoke(this, new MapLoggingEventArgs(log));
        }
        public class MapLog
        {
            public string Message;
            public System.Diagnostics.TraceLevel Severity;
            public DateTime When;

            public MapLog(string Message) : this(Message, System.Diagnostics.TraceLevel.Verbose) { }
            public MapLog(string Message, System.Diagnostics.TraceLevel Severity) : this(Message, Severity, DateTime.Now) { }
            public MapLog(string Message, System.Diagnostics.TraceLevel Severity, DateTime When)
            {
                this.Message = Message;
                this.Severity = Severity;
                this.When = When;
            }
        }
        public class MapLoggingEventArgs : EventArgs
        {
            public MapLog Log;

            public MapLoggingEventArgs(MapLog log)
            {
                this.Log = log;
            }
        }
        #endregion

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

        public bool AddSmooth01, AddSmooth02, AddSmooth03, AddSmooth04;
        public float SmoothnessFactor, SmoothnessFactor02, SmoothnessFactor03, SmoothnessFactor04;
        public float Amp01, Amp02, Amp03, Amp04;
        public float ContinentBias;
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
            RaiseLog("Generating map...");
            SetElevation();
        }

        private void SetElevation()
        {
            // before we reset the elevations, make sure we clear the resulting brushes
            Cell.ClearElevationBrushes();

            Random eRand = RandLvl2[(int)DetailedRandomizer.ElevationRand];

            // learned that doing it multiple times is the right way, to round out the edges using different granularity
            Cell[][] Cells1 = null;
            if (AddSmooth01)
            {
                RaiseLog("Setting initial elevation...");
                Cells1 = ApplyPseudoPerlinSmoothing(Cells, SmoothnessFactor, Amp01);
            }
            Cell[][] Cells2 = null;
            if (AddSmooth02)
            {
                RaiseLog("Adjusting elevation, level 2...");
                Cells2 = ApplyPseudoPerlinSmoothing(Cells, (SmoothnessFactor * SmoothnessFactor02), Amp02);
            }
            Cell[][] Cells3 = null;
            if (AddSmooth03)
            {
                RaiseLog("Adjusting elevation, level 3...");
                Cells3 = ApplyPseudoPerlinSmoothing(Cells, (SmoothnessFactor * SmoothnessFactor03), Amp03);
            }
            Cell[][] Cells4 = null;
            if (AddSmooth04)
            {
                RaiseLog("Adjusting elevation, level 4...");
                Cells4 = ApplyPseudoPerlinSmoothing(Cells, (SmoothnessFactor * SmoothnessFactor04), Amp04);
            }

            float actualMaxElevation = 0f;

            // now add them all together.
            RaiseLog("Adjusting elevation, combining all adjustments...");
            float result = 0;
            List<double> vals = new List<double>();
            for (int x = 0; x < Cells.Length; x++)
            {
                for (int y = 0; y < Cells[x].Length; y++)
                {
                    result = Cells1[x][y].Elevation; // start with our first random value

                    vals.Clear();
                    if (AddSmooth02) { vals.Add(Cells2[x][y].Elevation); }
                    if (AddSmooth03) { vals.Add(Cells3[x][y].Elevation); }
                    if (AddSmooth04) { vals.Add(Cells4[x][y].Elevation); }

                    foreach (float v in vals)
                    {
                        // v is between 0 and 1, with the +- 0.5 representing the % movement away from flat.
                        // so, pulling out the 0.5 gives us a +- random amplitude for the pass
                        // so, multiplying the result will nudge the result up or down by betweeen zero and +- the amplitude for the pass.
                        result = result * (1f - (v - 0.5f));
                    }

                    Cells[x][y].Elevation = result;

                    // store this for the continental bias.
                    actualMaxElevation = Math.Max(actualMaxElevation, result);
                }
            }

            if(ContinentBias > 0.0f)
            {
                RaiseLog("Adjusting for continental bias.");
                float Xm = Cells.Length / 2;
                float Ym = Cells[0].Length / 2;
                for (int x = 0; x < Cells.Length; x++)
                {
                    for (int y = 0; y < Cells[x].Length; y++)
                    {
                        float e = Cells[x][y].Elevation;

                        // a2 + b2 = c2
                        float distFromCenter = (float)(Math.Sqrt((Xm - x) * (Xm - x) + (Ym - y) * (Ym - y))); // calculate distance from center
                        float distFromCenterPerc = distFromCenter / (float)(Math.Sqrt((Xm * Xm) + (Ym * Ym)));

                        // in this case:
                        // x0 = is the height at the middle
                        // x1 = is the height at the edge
                        // t  = is the percentage (float) of the distance between the two

                        // by default, it would be 0.5 aross the map.
                        // so the bias pulls the outside down, and the middle up.

                        e = this.InterpolatePoly(e - (ContinentBias / 2), e + (ContinentBias / 2), (1-distFromCenterPerc));
                        e = Math.Min(Math.Max(e, 0), actualMaxElevation); // keep within existing limits

                        Cells[x][y].Elevation = e;
                    }
                }
            }

            // now that we're all done calculating, set up for drawing.
            RaiseLog("Setting paint brush by elevation.");
            for (int x = 0; x < Cells.Length; x++)
            {
                for (int y = 0; y < Cells[x].Length; y++)
                {
                    Cells[x][y].SetBrushByElevation();
                }
            }

            RaiseLog("Done with elevation.");
        }

        internal string[] GetInfoAt(int x, int y)
        {
            return Cells[x][y].GetInfo();
        }

        private Cell[][] ApplyPseudoPerlinSmoothing(Cell[][] Cells, float granularity, float amplitude)
        {
            Cell[][] results = Copy(Cells);            

            //float sampleSize = (float)Math.Pow(2, granularity); // calculates 2 ^ k
            float sampleSize = (float)granularity;
            float sampleFrequency = 1.0f / sampleSize;
            float x0,x1,y0,y1; // sample nodes to hit
            float h_blend, v_blend;
            float top, bottom;

            // set the random value for each sell, adjusting for the possible amplitude allowed in this pass.
            Random eRand = RandLvl2[(int)DetailedRandomizer.ElevationRand];
            for (int x = 0; x < results.Length; x++) // for each column
            {
                for (int y = 0; y < results[x].Length; y++) // for each row in the X'th column
                {
                    float v = ((float)eRand.NextDouble() * amplitude) + (0.5f - amplitude / 2f);
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
                }
            }

            return results;
        }

        float Interpolate(float x0, float x1, float alpha)
        {
            //return InterpolateLinear(x0, x1, alpha);
            return InterpolatePoly(x0, x1, alpha);
        }
        float InterpolateLinear(float x0, float x1, float alpha)
        {
            // this looks like a sort of dot product calculation, but certainly linear
            return x0 * (1 - alpha) + alpha * x1;
        }
        float InterpolatePoly(float x0, float x1, float t)
        {
            // first, figure out how much weight we should give to the movement along the curve from X0 to X1
            //6t^5-15t^4+10t^3
            //double y = t * t * t * (t * (6 * t - 15) + 10); // NOTE: y is between 0 and 1, given that t is between 0 and 1

            // 3t^2 - 2t^3 - gives a more rounded look to the first perlin smoothing pass than the 5th order, above.
            float y = t * t * (3f - 2f * t);

            // then apply the weight to the values X0, and X1
            float diff = x1 - x0;

            float partialDiff = diff * y; // determine how far we should move along the difference.
            
            //and lastly, adjust the partial diff for where we started.
            float adjustedPartialDiff = partialDiff + x0;
            
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
