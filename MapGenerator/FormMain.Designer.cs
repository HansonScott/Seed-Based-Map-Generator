namespace MapGenerator
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pMap = new System.Windows.Forms.Panel();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lblSeed = new System.Windows.Forms.Label();
            this.tbSeed = new System.Windows.Forms.TextBox();
            this.lblSmoothness = new System.Windows.Forms.Label();
            this.nudSmoothness = new System.Windows.Forms.NumericUpDown();
            this.nudWater = new System.Windows.Forms.NumericUpDown();
            this.lblWater = new System.Windows.Forms.Label();
            this.tbOutput = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudSmoothness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWater)).BeginInit();
            this.SuspendLayout();
            // 
            // pMap
            // 
            this.pMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pMap.Location = new System.Drawing.Point(109, 0);
            this.pMap.Name = "pMap";
            this.pMap.Size = new System.Drawing.Size(691, 418);
            this.pMap.TabIndex = 0;
            this.pMap.Paint += new System.Windows.Forms.PaintEventHandler(this.pMap_Paint);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(12, 12);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 1;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lblSeed
            // 
            this.lblSeed.AutoSize = true;
            this.lblSeed.Location = new System.Drawing.Point(1, 58);
            this.lblSeed.Name = "lblSeed";
            this.lblSeed.Size = new System.Drawing.Size(35, 13);
            this.lblSeed.TabIndex = 2;
            this.lblSeed.Text = "Seed:";
            // 
            // tbSeed
            // 
            this.tbSeed.Location = new System.Drawing.Point(3, 74);
            this.tbSeed.Name = "tbSeed";
            this.tbSeed.Size = new System.Drawing.Size(100, 20);
            this.tbSeed.TabIndex = 3;
            // 
            // lblSmoothness
            // 
            this.lblSmoothness.AutoSize = true;
            this.lblSmoothness.Location = new System.Drawing.Point(6, 119);
            this.lblSmoothness.Name = "lblSmoothness";
            this.lblSmoothness.Size = new System.Drawing.Size(37, 13);
            this.lblSmoothness.TabIndex = 4;
            this.lblSmoothness.Text = "Zoom:";
            // 
            // nudSmoothness
            // 
            this.nudSmoothness.DecimalPlaces = 1;
            this.nudSmoothness.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudSmoothness.Location = new System.Drawing.Point(3, 135);
            this.nudSmoothness.Name = "nudSmoothness";
            this.nudSmoothness.Size = new System.Drawing.Size(100, 20);
            this.nudSmoothness.TabIndex = 5;
            this.nudSmoothness.Value = new decimal(new int[] {
            40,
            0,
            0,
            0});
            // 
            // nudWater
            // 
            this.nudWater.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudWater.Location = new System.Drawing.Point(3, 186);
            this.nudWater.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudWater.Name = "nudWater";
            this.nudWater.Size = new System.Drawing.Size(100, 20);
            this.nudWater.TabIndex = 7;
            this.nudWater.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // lblWater
            // 
            this.lblWater.AutoSize = true;
            this.lblWater.Location = new System.Drawing.Point(12, 170);
            this.lblWater.Name = "lblWater";
            this.lblWater.Size = new System.Drawing.Size(68, 13);
            this.lblWater.TabIndex = 6;
            this.lblWater.Text = "Water Level:";
            // 
            // tbOutput
            // 
            this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutput.Location = new System.Drawing.Point(109, 424);
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.Size = new System.Drawing.Size(691, 20);
            this.tbOutput.TabIndex = 8;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.nudWater);
            this.Controls.Add(this.lblWater);
            this.Controls.Add(this.nudSmoothness);
            this.Controls.Add(this.lblSmoothness);
            this.Controls.Add(this.tbSeed);
            this.Controls.Add(this.lblSeed);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.pMap);
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Random Map Generator";
            ((System.ComponentModel.ISupportInitialize)(this.nudSmoothness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWater)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pMap;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblSeed;
        private System.Windows.Forms.TextBox tbSeed;
        private System.Windows.Forms.Label lblSmoothness;
        private System.Windows.Forms.NumericUpDown nudSmoothness;
        private System.Windows.Forms.NumericUpDown nudWater;
        private System.Windows.Forms.Label lblWater;
        private System.Windows.Forms.TextBox tbOutput;
    }
}

