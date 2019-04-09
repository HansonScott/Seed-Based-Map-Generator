namespace MapGenerator
{
    partial class MapSettings
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
            this.nudWater = new System.Windows.Forms.NumericUpDown();
            this.lblWater = new System.Windows.Forms.Label();
            this.nudSmoothness = new System.Windows.Forms.NumericUpDown();
            this.lblSmoothness = new System.Windows.Forms.Label();
            this.gbElevation = new System.Windows.Forms.GroupBox();
            this.lblContinent = new System.Windows.Forms.Label();
            this.nudContinentBias = new System.Windows.Forms.NumericUpDown();
            this.cbAdd04 = new System.Windows.Forms.CheckBox();
            this.cbAdd03 = new System.Windows.Forms.CheckBox();
            this.cbAdd02 = new System.Windows.Forms.CheckBox();
            this.nudAmp04 = new System.Windows.Forms.NumericUpDown();
            this.nudGrain04 = new System.Windows.Forms.NumericUpDown();
            this.nudAmp03 = new System.Windows.Forms.NumericUpDown();
            this.nudGrain03 = new System.Windows.Forms.NumericUpDown();
            this.lblAmplitude = new System.Windows.Forms.Label();
            this.nudAmp02 = new System.Windows.Forms.NumericUpDown();
            this.lblGrain = new System.Windows.Forms.Label();
            this.nudGrain02 = new System.Windows.Forms.NumericUpDown();
            this.gbRivers = new System.Windows.Forms.GroupBox();
            this.nudLakeSize = new System.Windows.Forms.NumericUpDown();
            this.lblLakeSize = new System.Windows.Forms.Label();
            this.nudRiverSourceElevationMin = new System.Windows.Forms.NumericUpDown();
            this.lblRiverSourceElevationMin = new System.Windows.Forms.Label();
            this.nudRiverBias = new System.Windows.Forms.NumericUpDown();
            this.lblRivers = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudWater)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSmoothness)).BeginInit();
            this.gbElevation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudContinentBias)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmp04)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrain04)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmp03)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrain03)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmp02)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrain02)).BeginInit();
            this.gbRivers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLakeSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRiverSourceElevationMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRiverBias)).BeginInit();
            this.SuspendLayout();
            // 
            // nudWater
            // 
            this.nudWater.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudWater.Location = new System.Drawing.Point(12, 76);
            this.nudWater.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudWater.Name = "nudWater";
            this.nudWater.Size = new System.Drawing.Size(100, 20);
            this.nudWater.TabIndex = 11;
            this.nudWater.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
            // 
            // lblWater
            // 
            this.lblWater.AutoSize = true;
            this.lblWater.Location = new System.Drawing.Point(21, 60);
            this.lblWater.Name = "lblWater";
            this.lblWater.Size = new System.Drawing.Size(68, 13);
            this.lblWater.TabIndex = 10;
            this.lblWater.Text = "Water Level:";
            // 
            // nudSmoothness
            // 
            this.nudSmoothness.DecimalPlaces = 1;
            this.nudSmoothness.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudSmoothness.Location = new System.Drawing.Point(12, 25);
            this.nudSmoothness.Name = "nudSmoothness";
            this.nudSmoothness.Size = new System.Drawing.Size(100, 20);
            this.nudSmoothness.TabIndex = 9;
            this.nudSmoothness.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // lblSmoothness
            // 
            this.lblSmoothness.AutoSize = true;
            this.lblSmoothness.Location = new System.Drawing.Point(15, 9);
            this.lblSmoothness.Name = "lblSmoothness";
            this.lblSmoothness.Size = new System.Drawing.Size(69, 13);
            this.lblSmoothness.TabIndex = 8;
            this.lblSmoothness.Text = "Zoom (grain):";
            // 
            // gbElevation
            // 
            this.gbElevation.Controls.Add(this.lblContinent);
            this.gbElevation.Controls.Add(this.nudContinentBias);
            this.gbElevation.Controls.Add(this.cbAdd04);
            this.gbElevation.Controls.Add(this.cbAdd03);
            this.gbElevation.Controls.Add(this.cbAdd02);
            this.gbElevation.Controls.Add(this.nudAmp04);
            this.gbElevation.Controls.Add(this.nudGrain04);
            this.gbElevation.Controls.Add(this.nudAmp03);
            this.gbElevation.Controls.Add(this.nudGrain03);
            this.gbElevation.Controls.Add(this.lblAmplitude);
            this.gbElevation.Controls.Add(this.nudAmp02);
            this.gbElevation.Controls.Add(this.lblGrain);
            this.gbElevation.Controls.Add(this.nudGrain02);
            this.gbElevation.Location = new System.Drawing.Point(127, 12);
            this.gbElevation.Name = "gbElevation";
            this.gbElevation.Size = new System.Drawing.Size(341, 169);
            this.gbElevation.TabIndex = 12;
            this.gbElevation.TabStop = false;
            this.gbElevation.Text = "Elevation";
            // 
            // lblContinent
            // 
            this.lblContinent.AutoSize = true;
            this.lblContinent.Location = new System.Drawing.Point(6, 128);
            this.lblContinent.Name = "lblContinent";
            this.lblContinent.Size = new System.Drawing.Size(75, 13);
            this.lblContinent.TabIndex = 25;
            this.lblContinent.Text = "Continent Bias";
            // 
            // nudContinentBias
            // 
            this.nudContinentBias.DecimalPlaces = 5;
            this.nudContinentBias.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudContinentBias.Location = new System.Drawing.Point(97, 126);
            this.nudContinentBias.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudContinentBias.Name = "nudContinentBias";
            this.nudContinentBias.Size = new System.Drawing.Size(74, 20);
            this.nudContinentBias.TabIndex = 24;
            this.nudContinentBias.Value = new decimal(new int[] {
            9,
            0,
            0,
            65536});
            // 
            // cbAdd04
            // 
            this.cbAdd04.AutoSize = true;
            this.cbAdd04.Checked = true;
            this.cbAdd04.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAdd04.Location = new System.Drawing.Point(6, 88);
            this.cbAdd04.Name = "cbAdd04";
            this.cbAdd04.Size = new System.Drawing.Size(45, 17);
            this.cbAdd04.TabIndex = 23;
            this.cbAdd04.Text = "Add";
            this.cbAdd04.UseVisualStyleBackColor = true;
            // 
            // cbAdd03
            // 
            this.cbAdd03.AutoSize = true;
            this.cbAdd03.Checked = true;
            this.cbAdd03.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAdd03.Location = new System.Drawing.Point(6, 63);
            this.cbAdd03.Name = "cbAdd03";
            this.cbAdd03.Size = new System.Drawing.Size(45, 17);
            this.cbAdd03.TabIndex = 22;
            this.cbAdd03.Text = "Add";
            this.cbAdd03.UseVisualStyleBackColor = true;
            // 
            // cbAdd02
            // 
            this.cbAdd02.AutoSize = true;
            this.cbAdd02.Checked = true;
            this.cbAdd02.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAdd02.Location = new System.Drawing.Point(6, 36);
            this.cbAdd02.Name = "cbAdd02";
            this.cbAdd02.Size = new System.Drawing.Size(45, 17);
            this.cbAdd02.TabIndex = 21;
            this.cbAdd02.Text = "Add";
            this.cbAdd02.UseVisualStyleBackColor = true;
            // 
            // nudAmp04
            // 
            this.nudAmp04.DecimalPlaces = 5;
            this.nudAmp04.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudAmp04.Location = new System.Drawing.Point(233, 88);
            this.nudAmp04.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudAmp04.Name = "nudAmp04";
            this.nudAmp04.Size = new System.Drawing.Size(75, 20);
            this.nudAmp04.TabIndex = 20;
            this.nudAmp04.Value = new decimal(new int[] {
            3,
            0,
            0,
            65536});
            // 
            // nudGrain04
            // 
            this.nudGrain04.DecimalPlaces = 5;
            this.nudGrain04.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudGrain04.Location = new System.Drawing.Point(97, 88);
            this.nudGrain04.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudGrain04.Name = "nudGrain04";
            this.nudGrain04.Size = new System.Drawing.Size(74, 20);
            this.nudGrain04.TabIndex = 19;
            this.nudGrain04.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            // 
            // nudAmp03
            // 
            this.nudAmp03.DecimalPlaces = 5;
            this.nudAmp03.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudAmp03.Location = new System.Drawing.Point(233, 62);
            this.nudAmp03.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudAmp03.Name = "nudAmp03";
            this.nudAmp03.Size = new System.Drawing.Size(75, 20);
            this.nudAmp03.TabIndex = 18;
            this.nudAmp03.Value = new decimal(new int[] {
            6,
            0,
            0,
            65536});
            // 
            // nudGrain03
            // 
            this.nudGrain03.DecimalPlaces = 5;
            this.nudGrain03.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudGrain03.Location = new System.Drawing.Point(97, 62);
            this.nudGrain03.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudGrain03.Name = "nudGrain03";
            this.nudGrain03.Size = new System.Drawing.Size(74, 20);
            this.nudGrain03.TabIndex = 17;
            this.nudGrain03.Value = new decimal(new int[] {
            3,
            0,
            0,
            65536});
            // 
            // lblAmplitude
            // 
            this.lblAmplitude.AutoSize = true;
            this.lblAmplitude.Location = new System.Drawing.Point(230, 20);
            this.lblAmplitude.Name = "lblAmplitude";
            this.lblAmplitude.Size = new System.Drawing.Size(70, 13);
            this.lblAmplitude.TabIndex = 16;
            this.lblAmplitude.Text = "Intensity (0-1)";
            // 
            // nudAmp02
            // 
            this.nudAmp02.DecimalPlaces = 5;
            this.nudAmp02.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudAmp02.Location = new System.Drawing.Point(233, 36);
            this.nudAmp02.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudAmp02.Name = "nudAmp02";
            this.nudAmp02.Size = new System.Drawing.Size(75, 20);
            this.nudAmp02.TabIndex = 15;
            this.nudAmp02.Value = new decimal(new int[] {
            8,
            0,
            0,
            65536});
            // 
            // lblGrain
            // 
            this.lblGrain.AutoSize = true;
            this.lblGrain.Location = new System.Drawing.Point(94, 20);
            this.lblGrain.Name = "lblGrain";
            this.lblGrain.Size = new System.Drawing.Size(56, 13);
            this.lblGrain.TabIndex = 14;
            this.lblGrain.Text = "Grain (0-1)";
            // 
            // nudGrain02
            // 
            this.nudGrain02.DecimalPlaces = 5;
            this.nudGrain02.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudGrain02.Location = new System.Drawing.Point(97, 36);
            this.nudGrain02.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudGrain02.Name = "nudGrain02";
            this.nudGrain02.Size = new System.Drawing.Size(74, 20);
            this.nudGrain02.TabIndex = 13;
            this.nudGrain02.Value = new decimal(new int[] {
            8,
            0,
            0,
            65536});
            // 
            // gbRivers
            // 
            this.gbRivers.Controls.Add(this.nudLakeSize);
            this.gbRivers.Controls.Add(this.lblLakeSize);
            this.gbRivers.Controls.Add(this.nudRiverSourceElevationMin);
            this.gbRivers.Controls.Add(this.lblRiverSourceElevationMin);
            this.gbRivers.Controls.Add(this.nudRiverBias);
            this.gbRivers.Controls.Add(this.lblRivers);
            this.gbRivers.Location = new System.Drawing.Point(474, 17);
            this.gbRivers.Name = "gbRivers";
            this.gbRivers.Size = new System.Drawing.Size(314, 164);
            this.gbRivers.TabIndex = 13;
            this.gbRivers.TabStop = false;
            this.gbRivers.Text = "Rivers";
            // 
            // nudLakeSize
            // 
            this.nudLakeSize.Location = new System.Drawing.Point(69, 70);
            this.nudLakeSize.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudLakeSize.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudLakeSize.Name = "nudLakeSize";
            this.nudLakeSize.Size = new System.Drawing.Size(100, 20);
            this.nudLakeSize.TabIndex = 29;
            this.nudLakeSize.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // lblLakeSize
            // 
            this.lblLakeSize.AutoSize = true;
            this.lblLakeSize.Location = new System.Drawing.Point(6, 72);
            this.lblLakeSize.Name = "lblLakeSize";
            this.lblLakeSize.Size = new System.Drawing.Size(57, 13);
            this.lblLakeSize.TabIndex = 28;
            this.lblLakeSize.Text = "Lake Size:";
            // 
            // nudRiverSourceElevationMin
            // 
            this.nudRiverSourceElevationMin.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudRiverSourceElevationMin.Location = new System.Drawing.Point(158, 41);
            this.nudRiverSourceElevationMin.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudRiverSourceElevationMin.Name = "nudRiverSourceElevationMin";
            this.nudRiverSourceElevationMin.Size = new System.Drawing.Size(100, 20);
            this.nudRiverSourceElevationMin.TabIndex = 27;
            this.nudRiverSourceElevationMin.Value = new decimal(new int[] {
            900,
            0,
            0,
            0});
            // 
            // lblRiverSourceElevationMin
            // 
            this.lblRiverSourceElevationMin.AutoSize = true;
            this.lblRiverSourceElevationMin.Location = new System.Drawing.Point(6, 43);
            this.lblRiverSourceElevationMin.Name = "lblRiverSourceElevationMin";
            this.lblRiverSourceElevationMin.Size = new System.Drawing.Size(146, 13);
            this.lblRiverSourceElevationMin.TabIndex = 26;
            this.lblRiverSourceElevationMin.Text = "Min elevation for river source:";
            // 
            // nudRiverBias
            // 
            this.nudRiverBias.DecimalPlaces = 5;
            this.nudRiverBias.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.nudRiverBias.Location = new System.Drawing.Point(67, 13);
            this.nudRiverBias.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudRiverBias.Name = "nudRiverBias";
            this.nudRiverBias.Size = new System.Drawing.Size(74, 20);
            this.nudRiverBias.TabIndex = 25;
            this.nudRiverBias.Value = new decimal(new int[] {
            9,
            0,
            0,
            65536});
            // 
            // lblRivers
            // 
            this.lblRivers.AutoSize = true;
            this.lblRivers.Location = new System.Drawing.Point(6, 16);
            this.lblRivers.Name = "lblRivers";
            this.lblRivers.Size = new System.Drawing.Size(55, 13);
            this.lblRivers.TabIndex = 0;
            this.lblRivers.Text = "River Bias";
            // 
            // MapSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gbRivers);
            this.Controls.Add(this.gbElevation);
            this.Controls.Add(this.nudWater);
            this.Controls.Add(this.lblWater);
            this.Controls.Add(this.nudSmoothness);
            this.Controls.Add(this.lblSmoothness);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MapSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MapSettings";
            ((System.ComponentModel.ISupportInitialize)(this.nudWater)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSmoothness)).EndInit();
            this.gbElevation.ResumeLayout(false);
            this.gbElevation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudContinentBias)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmp04)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrain04)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmp03)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrain03)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmp02)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrain02)).EndInit();
            this.gbRivers.ResumeLayout(false);
            this.gbRivers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLakeSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRiverSourceElevationMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRiverBias)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudWater;
        private System.Windows.Forms.Label lblWater;
        private System.Windows.Forms.NumericUpDown nudSmoothness;
        private System.Windows.Forms.Label lblSmoothness;
        private System.Windows.Forms.GroupBox gbElevation;
        private System.Windows.Forms.Label lblGrain;
        private System.Windows.Forms.NumericUpDown nudGrain02;
        private System.Windows.Forms.Label lblAmplitude;
        private System.Windows.Forms.NumericUpDown nudAmp02;
        private System.Windows.Forms.CheckBox cbAdd02;
        private System.Windows.Forms.NumericUpDown nudAmp04;
        private System.Windows.Forms.NumericUpDown nudGrain04;
        private System.Windows.Forms.NumericUpDown nudAmp03;
        private System.Windows.Forms.NumericUpDown nudGrain03;
        private System.Windows.Forms.CheckBox cbAdd04;
        private System.Windows.Forms.CheckBox cbAdd03;
        private System.Windows.Forms.Label lblContinent;
        private System.Windows.Forms.NumericUpDown nudContinentBias;
        private System.Windows.Forms.GroupBox gbRivers;
        private System.Windows.Forms.NumericUpDown nudRiverBias;
        private System.Windows.Forms.Label lblRivers;
        private System.Windows.Forms.NumericUpDown nudRiverSourceElevationMin;
        private System.Windows.Forms.Label lblRiverSourceElevationMin;
        private System.Windows.Forms.NumericUpDown nudLakeSize;
        private System.Windows.Forms.Label lblLakeSize;
    }
}