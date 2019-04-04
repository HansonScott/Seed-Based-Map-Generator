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
        public const int maxElevation = 1000;
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
                    Cells[x][y] = new Cell(x, y);
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
            for (int x = 0; x < Cells.Length; x++)
            {
                for (int y = 0; y < Cells[x].Length; y++)
                {
                    Cells[x][y].Elevation = eRand.Next(maxElevation);
                }
            }
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
