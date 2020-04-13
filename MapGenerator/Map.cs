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

        #region Fields and Properties
        private int seed;
        private Rectangle size;

        private Random RandLvl1;
        private List<Random> RandLvl2;
        private Cell[][] Cells;
        private List<Cell> LakeBases;
        #endregion

        private enum DetailedRandomizer
        {
            ColorRand = 0,
            ElevationRand = 1,
            TemperatureRand = 2,
            RainfallRand = 3,
        }

        #region Parameters
        public bool DrawBiomes = true;
        public bool DrawElevation = true;
        public bool DrawTemperature = true;
        public bool DrawRainfall = true;

        public int MaxElevation;
        public int MaxTemperature = 40; // Celsius is much easier to program :)
        public int FreezeTemperature = 0;
        public int MaxRainfall = 100; // not sure of the rainfall units or range, but 100 seems easy to program :)
        public float WaterElevation;
        public float RiverSourceElevationMinimum; // close to the top, but not extreme?

        public bool AddSmooth01, AddSmooth02, AddSmooth03, AddSmooth04;
        public float SmoothnessFactor, SmoothnessFactor02, SmoothnessFactor03, SmoothnessFactor04;
        public float Amp01, Amp02, Amp03, Amp04;
        public float ContinentBias;
        public float RiverBias;
        public int LakeSize;

        public bool AddTSmooth01, AddTSmooth02, AddTSmooth03, AddTSmooth04;
        public float TSmoothnessFactor, TSmoothnessFactor02, TSmoothnessFactor03, TSmoothnessFactor04;
        public float TAmp01, TAmp02, TAmp03, TAmp04;
        public float PolarBias;
        public float ElevationBias;

        public bool AddRSmooth01, AddRSmooth02, AddRSmooth03, AddRSmooth04;
        public float RSmoothnessFactor, RSmoothnessFactor02, RSmoothnessFactor03, RSmoothnessFactor04;
        public float RAmp01, RAmp02, RAmp03, RAmp04;
        public float RainfallBias;
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

            LakeBases = new List<Cell>();

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
            AddRivers(); // which also fills lakes
            SetTemperature();
            SetRainfall();
        }

        #region Elevation
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
                Cells1 = ApplyPseudoPerlinSmoothing(eRand, Cells, SmoothnessFactor, Amp01);
            }
            Cell[][] Cells2 = null;
            if (AddSmooth02)
            {
                RaiseLog("Adjusting elevation, level 2/4...");
                Cells2 = ApplyPseudoPerlinSmoothing(eRand, Cells, (SmoothnessFactor * SmoothnessFactor02), Amp02);
            }
            Cell[][] Cells3 = null;
            if (AddSmooth03)
            {
                RaiseLog("Adjusting elevation, level 3/4...");
                Cells3 = ApplyPseudoPerlinSmoothing(eRand, Cells, (SmoothnessFactor * SmoothnessFactor03), Amp03);
            }
            Cell[][] Cells4 = null;
            if (AddSmooth04)
            {
                RaiseLog("Adjusting elevation, level 4/4...");
                Cells4 = ApplyPseudoPerlinSmoothing(eRand, Cells, (SmoothnessFactor * SmoothnessFactor04), Amp04);
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
            RaiseLog("Adjusting elevations to within max limit.");
            // use algebra: y/actual = ?/max => max(y/actual) = ?
            // NOTE: max is always 1, and actual is a decimal (1.5ish)
            for (int x = 0; x < Cells.Length; x++)
            {
                for (int y = 0; y < Cells[x].Length; y++)
                {
                    Cells[x][y].Elevation = (Cells[x][y].Elevation / actualMaxElevation);
                }
            }

            // now that we're all done calculating, set up for drawing.
            RaiseLog("Setting paint brush by elevation.");
            for (int x = 0; x < Cells.Length; x++)
            {
                for (int y = 0; y < Cells[x].Length; y++)
                {
                    Cells[x][y].SetBrushByEnvironment();
                }
            }



            RaiseLog("Done with elevation.");
        }
        private Cell[][] ApplyPseudoPerlinSmoothing(Random thisRand, Cell[][] Cells, float granularity, float amplitude)
        {
            Cell[][] results = Copy(Cells);            

            //float sampleSize = (float)Math.Pow(2, granularity); // calculates 2 ^ k
            float sampleSize = (float)granularity;
            float sampleFrequency = 1.0f / sampleSize;
            float x0,x1,y0,y1; // sample nodes to hit
            float h_blend, v_blend;
            float top, bottom;

            // set the random value for each sell, adjusting for the possible amplitude allowed in this pass.
            for (int x = 0; x < results.Length; x++) // for each column
            {
                for (int y = 0; y < results[x].Length; y++) // for each row in the X'th column
                {
                    float v = ((float)thisRand.NextDouble() * amplitude) + (0.5f - amplitude / 2f);
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
        #endregion

        #region Rivers and Lakes
        private void AddRivers()
        {
            if (RiverBias == 0.0f) { return; }

            RaiseLog("Adding rivers...");

            // theory: find a high point, and wander down hill until we hit a water tile.
            List<Cell> sourceLocations = GetHighLocationForRiverSource(RiverBias);

            RaiseLog($"Created {sourceLocations.Count} river sources, running rivers downhill...");
            RunRiversDownHill(sourceLocations, true);
        }
        private List<Cell> GetHighLocationForRiverSource(float riverBias)
        {
            List<Cell> result = new List<Cell>();

            try
            {
                int marginW = (int)(this.size.Width * 0.05); // don't draw right on the edge of the map, it looks weird.
                int marginH = (int)(this.size.Height * 0.05); // don't draw right on the edge of the map, it looks weird.

                // use the bias param establish minimum distance between river sources = spread
                float dm = DistanceBetweenCells(new Cell(null, marginW, marginH), new Cell(null, (this.size.Width - marginW), (this.size.Height - marginH)));
                float spread = (1 - riverBias) * dm;

                bool FitsCriteriaForRiverSource = true;

                // checking every cell
                for (int x = 0; x < Cells.Length; x++)
                {
                    if(x < marginW || x > this.size.Width - marginW) { continue; }

                    for (int y = 0; y < Cells[x].Length; y++)
                    {
                        if (y < marginH || y > this.size.Height - marginH) { continue; }

                        Cell c = Cells[x][y];
                        FitsCriteriaForRiverSource = true;

                        // qualifications: 
                        // cell must be high enough
                        if (c.Elevation < RiverSourceElevationMinimum) { continue; } // not high enough

                        if (c.CellBiome == Cell.Biome.Polar ||
                            c.CellBiome == Cell.Biome.Tundra) { continue; } // no rivers from polar biome

                        // cell must not be close to an existing source - use bias for distance
                        foreach (Cell s in result)
                        {
                            if (DistanceBetweenCells(c, s) < spread) { FitsCriteriaForRiverSource = false; break; } // too close to existing, skip this one
                        }

                        if (!FitsCriteriaForRiverSource) { continue; } // then we've lost.

                        // if cell meets qualifications
                        c.IsRiver = true;
                        result.Add(c);
                    }
                }
            }
            catch(Exception ex)
            {
                RaiseLog($"Exception: {ex.Message}");
            }
            return result;
        }
        private void RunRiversDownHill(List<Cell> sources, bool CreateLakeBase)
        {
            for (int i = 0; i < sources.Count; i++)
            {
                RaiseLog($"Running river {i + 1} of {sources.Count}...");
                RunRiverDownHill(sources[i], true);
            }
        }
        private void RunRiverDownHill(Cell c, bool CreateLakes)
        {
            List<Cell> neighbors = GetCellNeighbors(c);

            // draw them all as rivers - expands the visuals of a river by 3 cells
            //for(int n = 0; n < neighbors.Count; n++)
            //{
            //    if(!neighbors[n].IsWater)
            //    {
            //        neighbors[n].IsRiver = true;
            //    }
            //}

            // start by getting neighbors with water, to check for coastline
            List<Cell> lowerNeighbors = GetLowerCellNeighbors(neighbors, c, true);

            // need to avoid coastline creep
            foreach(Cell ln in lowerNeighbors)
            {
                if(ln.CellBiome == Cell.Biome.Ocean || 
                    ln.CellBiome == Cell.Biome.Frozen_Ocean)
                    // if one of our neighbors is ocean, we're done, just stop.
                { return; }
            }

            // get the neighbors without water
            lowerNeighbors = GetLowerCellNeighbors(neighbors, c, false);

            Cell lowest = null;
            if(lowerNeighbors.Count == 0)
            {
                if (CreateLakes)
                {
                    c.IsLake = true;
                    MakeALake(c);
                }

                return;
            }
            else if (lowerNeighbors.Count == 1)
            {
                lowest = lowerNeighbors[0];
            }
            else // multiple lower neighbors
            {
                for (int i = 0; i < lowerNeighbors.Count; i++)
                {
                    //if (lowerNeighbors[i].IsRiver || 
                    if (lowerNeighbors[i].IsLake) { continue; } // don't bother go backwards, we would have already checked that cell.

                    if (lowest == null ||
                        lowest.Elevation > lowerNeighbors[i].Elevation)
                    {
                        lowest = lowerNeighbors[i];
                    }
                }
            }

            // if the lowest neighbor is still above this one, then there's nowhere to go.
            if (lowest.Elevation > c.Elevation)
            {
                // try 1: make neighbors into a lake
                for (int i = 0; i < neighbors.Count; i++)
                {
                    Cell n = neighbors[i];
                    n.IsRiver = false;
                    n.IsLake = true;
                }
            }
            else // the lowest one does continue.
            {
                if(lowest.Elevation < WaterElevation){ return; } // then we've hit water, discontinue.
                else
                {
                    // recursively call more river making.
                    lowest.IsRiver = true;
                    RunRiverDownHill(lowest, true);
                }
            }
        }

        private void MakeALake(Cell c)
        {
            List<Cell> lake = new List<Cell>() { c };

            Cell outlet = null;
            while(outlet == null || outlet.IsWater)
            {
                lake = ExpandLake(lake);
                if (lake != null && lake.Count > 0)
                {
                    outlet = LookForOutlet(lake);
                }

                if(lake.Count == 0) { outlet = null;  break; } // there's no where for this lake to expand to, just end.
            }

            if(outlet != null &&
                outlet.Elevation > WaterElevation &&
                !outlet.IsLake)
            {
                outlet.IsRiver = true;
                RunRiverDownHill(outlet, true);
            }
        }

        private List<Cell> ExpandLake(List<Cell> lake)
        {
            List<Cell> results = new List<Cell>();

            // get the next ring of cells around the current lake
            List<Cell> outerRing = GetCellNeighbors(lake, true);
            for (int n = 0; n < outerRing.Count; n++)
            {
                if (outerRing[n].IsLake) { continue; }

                outerRing[n].IsLake = true;
                results.Add(outerRing[n]);
            } // end neighbors

            return results;
        }

        private Cell LookForOutlet(List<Cell> lake)
        {
            if (lake == null || lake.Count == 0) { return null; }

            List<Cell> outerRing = GetCellNeighbors(lake, false);
            if(outerRing == null || outerRing.Count == 0) { return null; }

            Cell LowestFromOuterRing = GetLowestCell(outerRing);
            Cell HighestFromLakeRing = GetHighestCell(lake);
            //Cell LowestFromLakeRing = GetLowestCell(lake);

            // check if there's a lower cell in the outer ring
            if (LowestFromOuterRing.Elevation < HighestFromLakeRing.Elevation)
            {
                return LowestFromOuterRing;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Temperature
        private void SetTemperature()
        {
            // before we reset the elevations, make sure we clear the resulting brushes
            Cell.ClearTemperatureBrushes();

            Random tRand = RandLvl2[(int)DetailedRandomizer.TemperatureRand];

            // learned that doing it multiple times is the right way, to round out the edges using different granularity
            Cell[][] Cells1 = null;
            if (AddTSmooth01)
            {
                RaiseLog("Setting initial temperature...");
                Cells1 = ApplyPseudoPerlinSmoothing(tRand, Cells, TSmoothnessFactor, TAmp01);
            }
            Cell[][] Cells2 = null;
            if (AddTSmooth02)
            {
                RaiseLog("Adjusting temperature, level 2/4...");
                Cells2 = ApplyPseudoPerlinSmoothing(tRand, Cells, (SmoothnessFactor * TSmoothnessFactor02), TAmp02);
            }
            Cell[][] Cells3 = null;
            if (AddTSmooth03)
            {
                RaiseLog("Adjusting temperature, level 3/4...");
                Cells3 = ApplyPseudoPerlinSmoothing(tRand, Cells, (SmoothnessFactor * TSmoothnessFactor03), TAmp03);
            }
            Cell[][] Cells4 = null;
            if (AddTSmooth04)
            {
                RaiseLog("Adjusting temperature, level 4/4...");
                Cells4 = ApplyPseudoPerlinSmoothing(tRand, Cells, (SmoothnessFactor * TSmoothnessFactor04), TAmp04);
            }

            float actualMaxTemperature = 0f;

            // now add them all together.
            RaiseLog("Adjusting temperature, combining all adjustments...");
            float result = 0;
            List<double> vals = new List<double>();
            for (int x = 0; x < Cells.Length; x++)
            {
                for (int y = 0; y < Cells[x].Length; y++)
                {
                    // NOTE: since the default perlin noise function sets elevation, just use that property instead.
                    result = Cells1[x][y].Elevation; // start with our first random value

                    vals.Clear();
                    if (AddTSmooth02) { vals.Add(Cells2[x][y].Elevation); }
                    if (AddTSmooth03) { vals.Add(Cells3[x][y].Elevation); }
                    if (AddTSmooth04) { vals.Add(Cells4[x][y].Elevation); }

                    foreach (float v in vals)
                    {
                        // v is between 0 and 1, with the +- 0.5 representing the % movement away from flat.
                        // so, pulling out the 0.5 gives us a +- random amplitude for the pass
                        // so, multiplying the result will nudge the result up or down by betweeen zero and +- the amplitude for the pass.
                        result = result * (1f - (v - 0.5f));
                    }

                    // invert, for some reason the high values result in a lower temp...
                    Cells[x][y].Temperature = 1 - result;

                    // store this for the continental bias.
                    actualMaxTemperature = Math.Max(actualMaxTemperature, result);
                }
            }

            if (PolarBias> 0.0f)
            {
                RaiseLog("Adjusting for polar bias.");
                float Xm = Cells.Length / 2;
                float Ym = Cells[0].Length / 2;
                for (int x = 0; x < Cells.Length; x++)
                {
                    for (int y = 0; y < Cells[x].Length; y++)
                    {
                        float e = Cells[x][y].Temperature;

                        // note: since this is a single dimention, no need for formulas.
                        float distFromCenter = Ym - y;
                        float distFromCenterPerc = distFromCenter / Ym;

                        // by default, it would be 0.5 aross the map.
                        // so the bias pulls the outside down, and the middle up.
                        e = this.InterpolatePoly(e - (PolarBias / 2), e + (PolarBias / 2), (1 - (distFromCenterPerc)));
                        e = Math.Min(Math.Max(e, 0), actualMaxTemperature); // keep within existing limits

                        Cells[x][y].Temperature = e;
                    }
                }
            }

            if(ElevationBias > 0.0f)
            {
                RaiseLog("Adjusting for elevation bias.");
                // theory: higher is colder, lower is not necessarily warmer...
                // try 1: reduce temp by elevation % - resulted in too much temp reduction, not enough specificity
                // try 2: adjust temp by elevation away from midpoint (max - water)

                // elevation between water and max is the midpoint
                //float midpoint = (float)((MaxElevation - WaterElevation) / 2 + WaterElevation) / (float)MaxElevation; // we want the percentage decimal, not the actual, so devide by the max
                //float midpoint = (float)((MaxElevation - 0) / 2 + WaterElevation) / (float)MaxElevation; // we want the percentage decimal, not the actual, so devide by the max
                //float midpoint = (float)((MaxElevation - 0) / 2 + 0) / (float)MaxElevation; // we want the percentage decimal, not the actual, so devide by the max
                float midpoint = WaterElevation;

                // % adjustment +/- % from the midpoint to max or water(min)
                float deviationFromMidPoint;

                for (int x = 0; x < Cells.Length; x++)
                {
                    for (int y = 0; y < Cells[x].Length; y++)
                    {
                        deviationFromMidPoint = midpoint - Cells[x][y].Elevation; // we want a positive deviation in elevation to be a negative deviation in temperature
                        Cells[x][y].Temperature = Cells[x][y].Temperature + (Cells[x][y].Temperature * deviationFromMidPoint * ElevationBias);
                    }
                }
            }

            // now that we're all done calculating, set up for drawing.
            RaiseLog("Setting paint brush by temperature.");
            for (int x = 0; x < Cells.Length; x++)
            {
                for (int y = 0; y < Cells[x].Length; y++)
                {
                    Cells[x][y].SetBrushByEnvironment();
                }
            }

            RaiseLog("Done with temperature.");
        }
        #endregion

        #region Rainfall
        private void SetRainfall()
        {
            // before we reset the elevations, make sure we clear the resulting brushes
            Cell.ClearRainfallBrushes();

            Random rRand = RandLvl2[(int)DetailedRandomizer.RainfallRand];

            // learned that doing it multiple times is the right way, to round out the edges using different granularity
            Cell[][] Cells1 = null;
            if (AddRSmooth01)
            {
                RaiseLog("Setting initial rainfall...");
                Cells1 = ApplyPseudoPerlinSmoothing(rRand, Cells, RSmoothnessFactor, RAmp01);
            }
            Cell[][] Cells2 = null;
            if (AddRSmooth02)
            {
                RaiseLog("Adjusting rainfall, level 2/4...");
                Cells2 = ApplyPseudoPerlinSmoothing(rRand, Cells, (SmoothnessFactor * RSmoothnessFactor02), RAmp02);
            }
            Cell[][] Cells3 = null;
            if (AddRSmooth03)
            {
                RaiseLog("Adjusting rainfall, level 3/4...");
                Cells3 = ApplyPseudoPerlinSmoothing(rRand, Cells, (SmoothnessFactor * RSmoothnessFactor03), RAmp03);
            }
            Cell[][] Cells4 = null;
            if (AddRSmooth04)
            {
                RaiseLog("Adjusting rainfall, level 4/4...");
                Cells4 = ApplyPseudoPerlinSmoothing(rRand, Cells, (SmoothnessFactor * RSmoothnessFactor04), RAmp04);
            }

            float actualMaxRainfall = 0f;

            // now add them all together.
            RaiseLog("Adjusting rainfall, combining all adjustments...");
            float result = 0;
            List<double> vals = new List<double>();
            for (int x = 0; x < Cells.Length; x++)
            {
                for (int y = 0; y < Cells[x].Length; y++)
                {
                    // NOTE: since the default perlin noise function sets elevation, just use that property instead.
                    result = Cells1[x][y].Elevation; // start with our first random value

                    vals.Clear();
                    if (AddTSmooth02) { vals.Add(Cells2[x][y].Elevation); }
                    if (AddTSmooth03) { vals.Add(Cells3[x][y].Elevation); }
                    if (AddTSmooth04) { vals.Add(Cells4[x][y].Elevation); }

                    foreach (float v in vals)
                    {
                        // v is between 0 and 1, with the +- 0.5 representing the % movement away from flat.
                        // so, pulling out the 0.5 gives us a +- random amplitude for the pass
                        // so, multiplying the result will nudge the result up or down by betweeen zero and +- the amplitude for the pass.
                        result = result * (1f - (v - 0.5f));
                    }

                    Cells[x][y].Rainfall = result;

                    // store this for the continental bias.
                    actualMaxRainfall = Math.Max(actualMaxRainfall, result);
                }
            }

            if (RainfallBias != 0.0f)
            {
                RaiseLog("Adjusting for rainfall bias.");
                for (int x = 0; x < Cells.Length; x++)
                {
                    for (int y = 0; y < Cells[x].Length; y++)
                    {
                        float rVal = Cells[x][y].Rainfall;

                        // theory - more rain on the edge of the bias.
                        float distFromEdge = Cells.Length - x;
                        if(RainfallBias < 0.0)
                        {
                            distFromEdge = x;
                        }

                        float distFromEdgePerc = distFromEdge / Cells.Length;

                        // figure out the percentage along the curve we should be
                        float e = this.InterpolatePoly(0, 1, distFromEdgePerc);
                        e = e - 0.5f; // we want to either add or subtract from the random number (so, +- 0.5, rather than 0-1)
                        float adjust = e * Math.Abs(RainfallBias); // temper the adjustment by the bias
                        rVal = rVal + adjust; // adjust rVal by a %
                        rVal = Math.Min(Math.Max(rVal, 0), actualMaxRainfall); // keep within existing limits

                        Cells[x][y].Rainfall = rVal;
                    }
                }
            }

            // now that we're all done calculating, set up for drawing.
            RaiseLog("Setting paint brush by rainfall.");
            for (int x = 0; x < Cells.Length; x++)
            {
                for (int y = 0; y < Cells[x].Length; y++)
                {
                    Cells[x][y].SetBrushByEnvironment();
                }
            }

            RaiseLog("Done with rainfall.");
        }
        #endregion

        #region Utility functions
        private float DistanceBetweenCells(Cell c1, Cell c2)
        {
            return (float)Math.Sqrt(((c2.X - c1.X) * (c2.X - c1.X)) + ((c2.Y - c1.Y) * (c2.Y - c1.Y)));
        }
        private List<Cell> GetCellNeighbors(List<Cell> cells, bool IncludeWater)
        {
            List<Cell> results = new List<Cell>();
            List<Cell> neighbors = new List<Cell>();

            for (int c = 0; c < cells.Count; c++)
            {
                neighbors = GetCellNeighbors(cells[c]);
                for(int i = 0; i < neighbors.Count; i++)
                {
                    // avoid duplicates
                    if (!cells.Contains(neighbors[i]))
                    {
                        // catch the water flag
                        if (IncludeWater || !neighbors[i].IsWater)
                        {
                            results.Add(neighbors[i]);
                        }
                    }
                }
            }

            return results;
        }
        private List<Cell> GetCellNeighbors(Cell c)
        {
            return GetCellNeighbors(c, 1, new List<Cell>() { c });
        }
        private List<Cell> GetCellNeighbors(Cell c, int dist, List<Cell> excludeTheseCells)
        {
            List<Cell> results = new List<Cell>();

            bool[] Xs = new bool[10]; // 1-based and pos 5 is c
            bool[] Ys = new bool[10]; // 1-based pos 5 is c

            // because c exists, then
            Xs[2] = true;
            Xs[8] = true;
            Ys[4] = true;
            Ys[6] = true;

            // look west
            if(c.X - 1 >= 0)
            {
                Xs[1] = true;
                Xs[4] = true;
                Xs[7] = true;
            }

            // look east
            if(c.X + 1 < Cells.Length)
            {
                Xs[3] = true;
                Xs[6] = true;
                Xs[9] = true;
            }

            // look north
            if (c.Y - 1 >= 0)
            {
                Ys[1] = true;
                Ys[2] = true;
                Ys[3] = true;
            }

            // look south
            if (c.Y + 1 < Cells[0].Length)
            {
                Ys[7] = true;
                Ys[8] = true;
                Ys[9] = true;
            }

            Cell CellOfInterest = null;
            for(int i = 1; i < 10; i++)
            {
                if(Xs[i] && Ys[i])
                {
                    switch(i)
                    {
                        case 1:
                            CellOfInterest = Cells[c.X - 1][c.Y - 1];
                            break;
                        case 2:
                            CellOfInterest = Cells[c.X][c.Y - 1];
                            break;
                        case 3:
                            CellOfInterest = Cells[c.X + 1][c.Y - 1];
                            break;
                        case 4:
                            CellOfInterest = Cells[c.X - 1][c.Y];
                            break;
                        case 6:
                            CellOfInterest = Cells[c.X + 1][c.Y];
                            break;
                        case 7:
                            CellOfInterest = Cells[c.X - 1][c.Y + 1];
                            break;
                        case 8:
                            CellOfInterest = Cells[c.X][c.Y + 1];
                            break;
                        case 9:
                            CellOfInterest = Cells[c.X + 1][c.Y + 1];
                            break;
                        default:
                            break;
                    } // end switch
                    if(CellOfInterest != null &&
                        !excludeTheseCells.Contains(CellOfInterest))
                    {
                        results.Add(CellOfInterest);
                    }
                }
            }

            // keep going, until we run out of distance.
            if (dist > 1)
            {
                List<Cell> tempResults = new List<Cell>();
                tempResults.AddRange(results); // first off, copy the known ones into the growing list.

                // recursively call this with each neighbor
                foreach (Cell n in results)
                {
                    List<Cell> cells = GetCellNeighbors(n, dist - 1, tempResults); // exclude our temp list, so we don't backtrack
                    foreach (Cell cn in cells)
                    {
                        if (!tempResults.Contains(cn))
                        {
                            tempResults.Add(cn);
                        }
                    }
                }

                // now set the resulting collection back to the final result.
                results.AddRange(tempResults);
            }

            return results;
        }
        private List<Cell> GetLowerCellNeighbors(List<Cell> neighbors, Cell c, bool IncludeWater)
        {
            List<Cell> results = new List<Cell>();

            // trim the list for only lower neighbors
            foreach (Cell n in neighbors)
            {
                if (!IncludeWater && (n.IsRiver || n.IsLake || n.Elevation < WaterElevation))
                {
                    continue;
                }

                if (n.Elevation < c.Elevation) { results.Add(n); }
            }

            return results;
        }
        private Cell GetLowestCell(List<Cell> list)
        {
            Cell r = null;
            foreach (Cell c in list)
            {
                // capture the lowest cell among the neighbors
                if (r == null ||
                    r.Elevation > c.Elevation)
                {
                    r = c;
                }
            }

            return r;
        }
        private Cell GetHighestCell(List<Cell> list)
        {
            Cell r = null;
            foreach (Cell c in list)
            {
                // capture the lowest cell among the neighbors
                if (r == null ||
                    r.Elevation < c.Elevation)
                {
                    r = c;
                }
            }

            return r;
        }
        public string[] GetInfoAt(int x, int y)
        {
            if (Cells.Length > x &&
                Cells[x].Length > y)
            {
                return Cells[x][y].GetInfo();
            }
            else
            {
                return new string[] { };
            }
        }

        internal void PaintMap(Graphics g)
        {
            foreach(Cell[] rows in Cells)
            {
                foreach(Cell c in rows)
                {
                    if(DrawBiomes)
                    {
                        c.PaintBiomes(g);
                    }
                    else if(DrawElevation)
                    {
                        c.PaintElevation(g);
                    }
                    else if(DrawTemperature)
                    {
                        c.PaintTemp(g);
                    }
                    else if(DrawRainfall)
                    {
                        c.PainRainfall(g);
                    }
                }
            }
        }
        #endregion
    }
}
