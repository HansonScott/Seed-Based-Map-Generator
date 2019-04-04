using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapGenerator
{
    public partial class FormMain : Form
    {
        #region Main
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
        #endregion

        public Map CurrentMap;

        public FormMain()
        {
            InitializeComponent();
        }

        private void pMap_Paint(object sender, PaintEventArgs e)
        {
            CurrentMap?.PaintMap(e.Graphics);
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            int seed = GetSeedFromTextBox();
            CurrentMap = new Map(seed, pMap.ClientRectangle);

            // apply any parameters before generating...


            CurrentMap.GenerateMap();

            this.pMap.Refresh();
        }

        private int GetSeedFromTextBox()
        {
            int result = 0;
            if(!Int32.TryParse(tbSeed.Text, out result))
            {
                Random r = new Random();
                result = r.Next();
                tbSeed.Text = result.ToString();
            }
            return result;
        }
    }
}
