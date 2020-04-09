using System;
using System.Collections.Generic;
using System.Drawing;

namespace MapGenerator
{
    public class Cell
    {
        #region Static Resources
        private static Dictionary<float, Color> ElevationColors;
        private static Dictionary<Color, SolidBrush> ElevationBrushes;
        public static void ClearElevationBrushes() { ElevationColors?.Clear(); ElevationBrushes?.Clear(); }

        private static Dictionary<float, Color> TemperatureColors;
        private static Dictionary<Color, SolidBrush> TemperatureBrushes;
        public static void ClearTemperatureBrushes() { TemperatureColors?.Clear(); TemperatureBrushes?.Clear(); }

        private static Dictionary<float, Color> RainfallColors;
        private static Dictionary<Color, SolidBrush> RainfallBrushes;
        public static void ClearRainfallBrushes() { RainfallColors?.Clear(); RainfallBrushes?.Clear(); }

        private static Dictionary<Biome, Color> BiomeColors;
        private static Dictionary<Color, SolidBrush> BiomeBrushes;
        public static void ClearBiomeBrushes() { BiomeColors?.Clear(); BiomeBrushes?.Clear(); }
        #endregion

        #region Fields
        Map ParentMap;
        public int X;
        public int Y;
        public bool IsSample;
        private float _Elevation;
        public Color ElevationColor;
        public Color RiverColor = Color.FromArgb(50, 50, 255);
        public Color LakeColor = Color.FromArgb(50, 50, 255);
        public RectangleF ThisRect;

        public Biome CellBiome;
        public Color BiomeColor;

        private SolidBrush ElevationBrush;
        private SolidBrush TemperatureBrush;
        private SolidBrush RainfallBrush;
        private SolidBrush BiomeBrush;

        private float _Temperature;
        public Color TemperatureColor;

        private float _Rainfall;
        public Color RainfallColor;
        #endregion

        public enum Biome
        {
            Ocean = 1, // - mid blue, based on depth
            River = 2, // - greenish blue
            Lake = 3, // - greenish blue

            // low temp
            Polar = 4, // low temp low rain - white
            Frozen_Ocean = 5, // tundra, but on water - soft greyish blue
            Tundra = 6, // cold temp, low to mid rain - white w/ dark brown? (dark teal, brown, light blue) 

            // mid temp
            Boreal_Forest = 7, // less rain - dark green or dark brown
            Prairie = 8, // mid rain - soft green
            Woods_And_Shrubs = 9, // more rain - mid green

            // high temp
            Desert = 10, // any temp, low rain - reddish tan, light brown
            Savanna = 11, // low to mid rain - tan / brown
            Tropical_Rainforest = 12, // mid to high rain - bright green
        }

        #region Properties
        public float Elevation
        {
            get { return _Elevation; }
            set
            {
                _Elevation = value;
            }
        }
        public float Temperature
        {
            get { return _Temperature; }
            set
            {

                _Temperature = value;
            }
        }
        public float Rainfall
        {
            get { return _Rainfall; }
            set
            {

                _Rainfall = value;
            }
        }
        public bool IsRiver
        {
            get { return (this.CellBiome == Biome.River); }
            set
            {
                this.CellBiome = Biome.River;
                SetBrushByEnvironment();
            }
        }
        public bool IsLake
        {
            get { return (this.CellBiome == Biome.Lake); }
            set
            {
                this.CellBiome = Biome.Lake;
                SetBrushByEnvironment();
            }
        }
        public float ActualElevation { get { return (Elevation * ParentMap.MaxElevation); } }
        public float ActualTemperature { get { return (Temperature * ParentMap.MaxTemperature); } }
        public float ActualRainfall { get { return (Rainfall * ParentMap.MaxRainfall); } }
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
        public void SetBrushByEnvironment()
        {
            #region elevation
            if(ElevationColors == null)
            {
                ElevationColors = new Dictionary<float, Color>();
            }

            if(ElevationColors.ContainsKey((float)Math.Round((decimal)Elevation, 3)))
            {
                this.ElevationColor = ElevationColors[(float)Math.Round((decimal)Elevation, 3)];
            }
            else
            {
                // grey
                //int v = (int)(Elevation * 255 / ParentMap.maxElevation);
                //int v = (int)(Elevation * 255); // assume value to be between 0 and 1
                //Color c = Color.FromArgb(v, v, v);
                //BrushColor = new SolidBrush(c);

                // below water
                if (ActualElevation < ParentMap.WaterElevation)
                {
                    // at 0 elevation: 0,0,255
                    // at waterElevation: 0,100,255

                    //int v = (int)(Elevation * 100 / ParentMap.WaterElevation);
                    int v = (int)(Elevation * 100);
                    int b = (int)((ActualElevation / ParentMap.WaterElevation) * 125) + 130;
                    ElevationColor = Color.FromArgb(0, Math.Max(Math.Min(v, 255), 0), b);
                }
                // within 10% of water elevation
                else if (ActualElevation < (ParentMap.WaterElevation * 1.1))
                {
                    ElevationColor = Color.Tan;
                }
                else // above water
                {
                    // at waterElevation: 0,140,0 // light green
                    // at maxElevation: 255,255,255 // white
                    // green gradient
                    //int v = (int)(Elevation * 140 / ParentMap.maxElevation);
                    int v = (int)(Elevation * 140);
                    ElevationColor = Color.FromArgb(v,
                                                    (Math.Max(((255 - v) / 2) + v, 140)), 
                                                    v);
                }

                ElevationColors.Add((float)Math.Round((decimal)Elevation, 3), ElevationColor);
            } // end else, not in static collection

            // now set the actual drawing brush
            if(ElevationBrushes == null)
            {
                ElevationBrushes = new Dictionary<Color, SolidBrush>();
            }


            Color ThisColor;
            if (this.IsLake)
            {
                ThisColor = LakeColor;
            }
            else if (this.IsRiver)
            {
                ThisColor = RiverColor;
            }
            else
            {
                ThisColor = ElevationColor;
            }

            if(!ElevationBrushes.ContainsKey(ThisColor))
            {
                ElevationBrushes.Add(ThisColor, new SolidBrush(ThisColor));
            }

            // now check for an actual difference before assigning
            if(ElevationBrush == null || ElevationBrush.Color != ThisColor)
            {
                ElevationBrush = ElevationBrushes[ThisColor];
            }


            #endregion

            #region Temperature
            if (TemperatureColors == null)
            {
                TemperatureColors = new Dictionary<float, Color>();
            }

            if (TemperatureColors.ContainsKey((float)Math.Round((decimal)Temperature, 3)))
            {
                this.TemperatureColor = TemperatureColors[(float)Math.Round((decimal)Temperature, 3)];
            }
            else
            {
                // Maroon       128,0,  0   0-10%
                // red          255,0,  0   10-20%
                // orange       255,128,0   20-30%
                // yellow       255,255,0   30-40%
                // lime         128,255,0   40-50%
                // green        0,  255,0   50-60%
                // teal         0,255,128   60-70%
                // aqua         0,255,255   70-80%
                // light blue   0,128,255   80-90%
                // dark blue    0,  0,255   90-100%

                float perc = ActualTemperature / ParentMap.MaxTemperature;
                int r, g, b;
                if      (perc < .10) { r = 0; g = 128; b = 255; }
                else if (perc < .20) { r = 0; g = 255; b = 255; }
                else if (perc < .30) { r = 0; g = 255; b = 128; }
                else if (perc < .40) { r = 0; g = 255; b = 0; }
                else if (perc < .50) { r = 128; g = 255; b = 0; }
                else if (perc < .60) { r = 255; g = 255; b = 0; }
                else if (perc < .70) { r = 255; g = 128; b = 0; }
                else if (perc < .80) { r = 255; g = 0; b = 0; }
                else if (perc < .90) { r = 128; g = 0; b = 0; }
                else { r = 0; g = 0; b = 255; }

                TemperatureColor = Color.FromArgb(r, g, b);

                TemperatureColors.Add((float)Math.Round((decimal)Temperature, 3), TemperatureColor);
            } // end else, not in static collection

            // set the brush
            if(TemperatureBrushes == null)
            {
                TemperatureBrushes = new Dictionary<Color, SolidBrush>();
            }

            // cache the brush
            if(!TemperatureBrushes.ContainsKey(TemperatureColor))
            {
                TemperatureBrushes.Add(TemperatureColor, new SolidBrush(TemperatureColor));
            }

            // now, only assign if it is actually different
            if(TemperatureBrush == null || TemperatureBrush.Color != TemperatureColor )
            {
                TemperatureBrush = TemperatureBrushes[TemperatureColor];
            }
            #endregion

            #region Rainfall
            if (RainfallColors == null)
            {
                RainfallColors = new Dictionary<float, Color>();
            }

            if (RainfallColors.ContainsKey((float)Math.Round((decimal)Rainfall, 3)))
            {
                this.RainfallColor = RainfallColors[(float)Math.Round((decimal)Rainfall, 3)];
            }
            else
            {
                // Maroon       128,0,  0   0-10%
                // red          255,0,  0   10-20%
                // orange       255,128,0   20-30%
                // yellow       255,255,0   30-40%
                // lime         128,255,0   40-50%
                // green        0,  255,0   50-60%
                // teal         0,255,128   60-70%
                // aqua         0,255,255   70-80%
                // light blue   0,128,255   80-90%
                // dark blue    0,  0,255   90-100%

                float perc = ActualRainfall / ParentMap.MaxRainfall;
                int r, g, b;
                if (perc < .10)      { r = 128; g = 0; b = 0; }
                else if (perc < .20) { r = 255; g = 0; b = 0; }
                else if (perc < .30) { r = 255; g = 128; b = 0; }
                else if (perc < .40) { r = 255; g = 255; b = 0; }
                else if (perc < .50) { r = 128; g = 255; b = 0; }
                else if (perc < .60) { r = 0; g = 255; b = 0; }
                else if (perc < .70) { r = 0; g = 255; b = 128; }
                else if (perc < .80) { r = 0; g = 255; b = 255; }
                else if (perc < .90) { r = 0; g = 128; b = 255; }
                else                 { r = 0; g = 0; b = 255; }

                RainfallColor = Color.FromArgb(r, g, b);

                RainfallColors.Add((float)Math.Round((decimal)Rainfall, 3), RainfallColor);
            } // end else, not in static collection

            // set up the cache
            if(RainfallBrushes == null)
            {
                RainfallBrushes = new Dictionary<Color, SolidBrush>();
            }

            // check for the existance, or add it to the cache
            if(!RainfallBrushes.ContainsKey(RainfallColor))
            {
                RainfallBrushes.Add(RainfallColor, new SolidBrush(RainfallColor));
            }

            // and only reassign it if it is new
            if(RainfallBrush == null || RainfallBrush.Color != RainfallColor)
            {
                RainfallBrush = new SolidBrush(RainfallColor);
            }
            #endregion

            #region Biome
            // combine temperature, rainfall, and elevation to determine the biome
            if (!IsLake && !IsRiver)
            {
                if (ActualElevation < ParentMap.WaterElevation)
                {
                    if (ActualTemperature <= 0)
                    {
                        CellBiome = Biome.Frozen_Ocean;
                    }
                    else
                    {
                        CellBiome = Biome.Ocean;
                    }
                }
                else // not water
                {
                    if (Temperature < 0.2)
                    {
                        this.CellBiome = Biome.Polar;
                    }
                    else if (Temperature < 0.4)
                    {
                        if (Rainfall < 0.4)
                        {
                            // low temp, low rain
                            this.CellBiome = Biome.Desert;
                        }
                        else if (Rainfall > 0.7)
                        {
                            // low temp, high rain
                            this.CellBiome = Biome.Boreal_Forest;
                        }
                        else // mid rainfall
                        {
                            // low temp, high rain
                            this.CellBiome = Biome.Tundra;
                        }
                    }
                    else if (Temperature > 0.7)
                    {
                        if (Rainfall < 0.4)
                        {
                            // high temp, low rain
                            this.CellBiome = Biome.Desert;
                        }
                        else if (Rainfall > 0.7)
                        {
                            // high temp, high rain
                            this.CellBiome = Biome.Tropical_Rainforest;
                        }
                        else
                        {
                            // high temp, mid rain
                            this.CellBiome = Biome.Savanna;
                        }
                    }
                    else // mid temp
                    {
                        if (Rainfall < 0.4)
                        {
                            // mid temp, low rain
                            this.CellBiome = Biome.Prairie;
                        }
                        else if (Rainfall > 0.7)
                        {
                            // mid temp, high rain
                            this.CellBiome = Biome.Woods_And_Shrubs;
                        }
                        else
                        {
                            // mid temp, mid rain
                            this.CellBiome = Biome.Woods_And_Shrubs;
                        }
                    }
                }
            }

            // now establish the colors and brushes accordingly.
            if (BiomeColors == null)
            {
                BiomeColors = new Dictionary<Biome, Color>();
            }

            // set color by biome
            if (BiomeColors.ContainsKey(this.CellBiome))
            {
                this.BiomeColor = BiomeColors[this.CellBiome];
            }
            else
            {
                int br = 0, bg = 0, bb = 0;
                // now that we have the biome, set the brush.
                switch (this.CellBiome)
                {
                    case Biome.Boreal_Forest:
                        br = 2; bg = 48; bb = 0;
                        break;
                    case Biome.Desert:
                        br = 230; bg = 130; bb = 30;
                        break;
                    case Biome.Frozen_Ocean:
                        br = 180; bg = 250; bb = 245;
                        break;
                    case Biome.Lake:
                        br = 40; bg = 160; bb = 250;
                        break;
                    case Biome.Ocean:
                        br = 0; bg = 0; bb = 255;
                        break;
                    case Biome.Polar:
                        br = 255; bg = 250; bb = 255;
                        break;
                    case Biome.Prairie:
                        br = 130; bg = 200; bb = 75;
                        break;
                    case Biome.River:
                        br = 40; bg = 160; bb = 250;
                        break;
                    case Biome.Savanna:
                        br = 170; bg = 75; bb = 40;
                        break;
                    case Biome.Tropical_Rainforest:
                        br = 90; bg = 160; bb = 90;
                        break;
                    case Biome.Tundra:
                        br = 237; bg = 237; bb = 237;
                        break;
                    case Biome.Woods_And_Shrubs:
                        br = 40; bg = 120; bb = 20;
                        break;
                    default:
                        break;
                }

                BiomeColor = Color.FromArgb(br, bg, bb);
                BiomeColors.Add(this.CellBiome, BiomeColor);
            }

            // set brush by color
            if (BiomeBrushes == null)
            {
                BiomeBrushes = new Dictionary<Color, SolidBrush>();
            }

            // check for caching
            if(!BiomeBrushes.ContainsKey(BiomeColor))
            {
                BiomeBrushes.Add(BiomeColor, new SolidBrush(BiomeColor));
            }

            // check for a difference before assigning
            if(BiomeBrush == null || BiomeBrush.Color != BiomeColor)
            {
                BiomeBrush = BiomeBrushes[BiomeColor];
            }
            #endregion
        }

        internal void PaintElevation(Graphics g)
        {
            g.FillRectangle(ElevationBrush, ThisRect);
        }

        internal void PaintBiomes(Graphics g)
        {
            g.FillRectangle(BiomeBrush, ThisRect);
        }

        internal void PaintTemp(Graphics g)
        {
            g.FillRectangle(TemperatureBrush, ThisRect);
        }

        internal void PainRainfall(Graphics g)
        {
            g.FillRectangle(RainfallBrush, ThisRect);
        }

        internal string[] GetInfo()
        {
            List<string> info = new List<string>();

            // add all interesting info
            info.Add($"Elevation: {(int)ActualElevation}");
            //info.Add(((int)ActualElevation).ToString());
            info.Add($"Temperature: {(int)ActualTemperature} C");
            //info.Add(((int)ActualTemperature).ToString());
            info.Add($"Rainfall: {(int)ActualRainfall}");
            //info.Add(((int)ActualRainfall).ToString());
            info.Add($"Biome: {CellBiome}");
            //info.Add(CellBiome.ToString());

            if (IsRiver)
            {
                info.Add("River");
            }
            else if(IsLake)
            {
                info.Add("Lake");
            }
            else if(ActualElevation <= ParentMap.WaterElevation)
            {
                info.Add("Water");
            }

            return info.ToArray();
        }
        #endregion

        #region Private Functions
        #endregion
    }
}
