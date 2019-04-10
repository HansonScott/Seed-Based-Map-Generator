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
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.btnSettings = new System.Windows.Forms.Button();
            this.tbInfo = new System.Windows.Forms.TextBox();
            this.btnSaveImage = new System.Windows.Forms.Button();
            this.btnRedraw = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // pMap
            // 
            this.pMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pMap.Cursor = System.Windows.Forms.Cursors.Cross;
            this.pMap.Location = new System.Drawing.Point(109, 0);
            this.pMap.Name = "pMap";
            this.pMap.Size = new System.Drawing.Size(691, 361);
            this.pMap.TabIndex = 0;
            this.pMap.Paint += new System.Windows.Forms.PaintEventHandler(this.pMap_Paint);
            this.pMap.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pMap_MouseMove);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(2, 12);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(100, 23);
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
            // tbOutput
            // 
            this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutput.BackColor = System.Drawing.Color.Black;
            this.tbOutput.ForeColor = System.Drawing.Color.White;
            this.tbOutput.Location = new System.Drawing.Point(109, 367);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbOutput.Size = new System.Drawing.Size(691, 77);
            this.tbOutput.TabIndex = 8;
            // 
            // btnSettings
            // 
            this.btnSettings.Location = new System.Drawing.Point(5, 222);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(100, 23);
            this.btnSettings.TabIndex = 9;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // tbInfo
            // 
            this.tbInfo.BackColor = System.Drawing.SystemColors.Control;
            this.tbInfo.Enabled = false;
            this.tbInfo.Location = new System.Drawing.Point(5, 273);
            this.tbInfo.Multiline = true;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.Size = new System.Drawing.Size(100, 171);
            this.tbInfo.TabIndex = 10;
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Location = new System.Drawing.Point(3, 100);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(100, 23);
            this.btnSaveImage.TabIndex = 11;
            this.btnSaveImage.Text = "Save as Image";
            this.btnSaveImage.UseVisualStyleBackColor = true;
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            // 
            // btnRedraw
            // 
            this.btnRedraw.Location = new System.Drawing.Point(4, 125);
            this.btnRedraw.Name = "btnRedraw";
            this.btnRedraw.Size = new System.Drawing.Size(100, 23);
            this.btnRedraw.TabIndex = 12;
            this.btnRedraw.Text = "Redraw";
            this.btnRedraw.UseVisualStyleBackColor = true;
            this.btnRedraw.Click += new System.EventHandler(this.btnRedraw_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnRedraw);
            this.Controls.Add(this.btnSaveImage);
            this.Controls.Add(this.tbInfo);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.tbSeed);
            this.Controls.Add(this.lblSeed);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.pMap);
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Random Map Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pMap;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblSeed;
        private System.Windows.Forms.TextBox tbSeed;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.TextBox tbInfo;
        private System.Windows.Forms.Button btnSaveImage;
        private System.Windows.Forms.Button btnRedraw;
    }
}

