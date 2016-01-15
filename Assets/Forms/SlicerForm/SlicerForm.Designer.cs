

namespace SlicerForm
{
    partial class SlicerForm
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
            this.ShowGCD = new System.Windows.Forms.CheckBox();
            this.ShowSloxels = new System.Windows.Forms.CheckBox();
            this.SloxelReadout = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelSlox = new System.Windows.Forms.Label();
            this.SloxelNumber = new System.Windows.Forms.TrackBar();
            this.LayerTrackbar = new System.Windows.Forms.TrackBar();
            this.singleStep = new System.Windows.Forms.RadioButton();
            this.SliceButton = new System.Windows.Forms.Button();
            this.WallThickness = new System.Windows.Forms.RadioButton();
            this.None = new System.Windows.Forms.RadioButton();
            this.ByGcdLayers = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Play = new System.Windows.Forms.Button();
            this.wtLabel = new System.Windows.Forms.Label();
            this.wtSlider = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.ResUpDown = new System.Windows.Forms.NumericUpDown();
            this.ShowCsection = new System.Windows.Forms.CheckBox();
            this.SloxelUpDown = new System.Windows.Forms.NumericUpDown();
            this.LayerUpDown = new System.Windows.Forms.NumericUpDown();
            this.ShowBalls = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.SloxelNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LayerTrackbar)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wtSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SloxelUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LayerUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // ShowGCD
            // 
            this.ShowGCD.AutoSize = true;
            this.ShowGCD.Checked = true;
            this.ShowGCD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowGCD.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowGCD.Location = new System.Drawing.Point(220, 355);
            this.ShowGCD.Name = "ShowGCD";
            this.ShowGCD.Size = new System.Drawing.Size(42, 14);
            this.ShowGCD.TabIndex = 12;
            this.ShowGCD.Text = "GCD";
            this.ShowGCD.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.ShowGCD.UseVisualStyleBackColor = true;
            this.ShowGCD.CheckedChanged += new System.EventHandler(this.ShowGCD_CheckedChanged);
            // 
            // ShowSloxels
            // 
            this.ShowSloxels.AutoSize = true;
            this.ShowSloxels.Checked = true;
            this.ShowSloxels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowSloxels.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowSloxels.Location = new System.Drawing.Point(120, 355);
            this.ShowSloxels.Name = "ShowSloxels";
            this.ShowSloxels.Size = new System.Drawing.Size(49, 14);
            this.ShowSloxels.TabIndex = 11;
            this.ShowSloxels.Text = "Sloxels";
            this.ShowSloxels.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.ShowSloxels.UseVisualStyleBackColor = true;
            this.ShowSloxels.CheckedChanged += new System.EventHandler(this.ShowSloxels_CheckedChanged);
            // 
            // SloxelReadout
            // 
            this.SloxelReadout.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SloxelReadout.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.SloxelReadout.Location = new System.Drawing.Point(118, 122);
            this.SloxelReadout.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.SloxelReadout.Name = "SloxelReadout";
            this.SloxelReadout.Size = new System.Drawing.Size(213, 221);
            this.SloxelReadout.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(6, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "Layer: ";
            // 
            // labelSlox
            // 
            this.labelSlox.AutoSize = true;
            this.labelSlox.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSlox.Location = new System.Drawing.Point(7, 107);
            this.labelSlox.Name = "labelSlox";
            this.labelSlox.Size = new System.Drawing.Size(35, 12);
            this.labelSlox.TabIndex = 8;
            this.labelSlox.Text = "Sloxel: ";
            // 
            // SloxelNumber
            // 
            this.SloxelNumber.AutoSize = false;
            this.SloxelNumber.Location = new System.Drawing.Point(6, 87);
            this.SloxelNumber.Maximum = 100;
            this.SloxelNumber.Name = "SloxelNumber";
            this.SloxelNumber.Size = new System.Drawing.Size(253, 20);
            this.SloxelNumber.TabIndex = 7;
            this.SloxelNumber.Scroll += new System.EventHandler(this.SloxelNumber_Scroll);
            // 
            // LayerTrackbar
            // 
            this.LayerTrackbar.AutoSize = false;
            this.LayerTrackbar.Location = new System.Drawing.Point(6, 48);
            this.LayerTrackbar.Name = "LayerTrackbar";
            this.LayerTrackbar.Size = new System.Drawing.Size(253, 20);
            this.LayerTrackbar.TabIndex = 2;
            this.LayerTrackbar.Scroll += new System.EventHandler(this.LayerTrackbar_Scroll);
            // 
            // singleStep
            // 
            this.singleStep.AutoSize = true;
            this.singleStep.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.singleStep.Location = new System.Drawing.Point(6, 122);
            this.singleStep.Name = "singleStep";
            this.singleStep.Size = new System.Drawing.Size(69, 16);
            this.singleStep.TabIndex = 3;
            this.singleStep.TabStop = true;
            this.singleStep.Text = "Single Step";
            this.singleStep.UseVisualStyleBackColor = true;
            this.singleStep.CheckedChanged += new System.EventHandler(this.radioButtonSingleStep_CheckedChanged);
            // 
            // SliceButton
            // 
            this.SliceButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SliceButton.Location = new System.Drawing.Point(6, 19);
            this.SliceButton.Name = "SliceButton";
            this.SliceButton.Size = new System.Drawing.Size(75, 23);
            this.SliceButton.TabIndex = 1;
            this.SliceButton.Text = "Slice";
            this.SliceButton.UseVisualStyleBackColor = true;
            this.SliceButton.Click += new System.EventHandler(this.SliceButton_Click);
            // 
            // WallThickness
            // 
            this.WallThickness.AutoSize = true;
            this.WallThickness.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WallThickness.Location = new System.Drawing.Point(6, 162);
            this.WallThickness.Name = "WallThickness";
            this.WallThickness.Size = new System.Drawing.Size(83, 16);
            this.WallThickness.TabIndex = 5;
            this.WallThickness.TabStop = true;
            this.WallThickness.Text = "WallThickness";
            this.WallThickness.UseVisualStyleBackColor = true;
            this.WallThickness.CheckedChanged += new System.EventHandler(this.WallThickness_CheckedChanged);
            // 
            // None
            // 
            this.None.AutoSize = true;
            this.None.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.None.Location = new System.Drawing.Point(6, 182);
            this.None.Name = "None";
            this.None.Size = new System.Drawing.Size(45, 16);
            this.None.TabIndex = 6;
            this.None.TabStop = true;
            this.None.Text = "None";
            this.None.UseVisualStyleBackColor = true;
            this.None.CheckedChanged += new System.EventHandler(this.None_CheckedChanged);
            // 
            // ByGcdLayers
            // 
            this.ByGcdLayers.AutoSize = true;
            this.ByGcdLayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ByGcdLayers.Location = new System.Drawing.Point(6, 142);
            this.ByGcdLayers.Name = "ByGcdLayers";
            this.ByGcdLayers.Size = new System.Drawing.Size(74, 16);
            this.ByGcdLayers.TabIndex = 4;
            this.ByGcdLayers.TabStop = true;
            this.ByGcdLayers.Text = "GCD Layers";
            this.ByGcdLayers.UseVisualStyleBackColor = true;
            this.ByGcdLayers.CheckedChanged += new System.EventHandler(this.radioButtonByGCD_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.Location = new System.Drawing.Point(13, 13);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(433, 433);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ShowBalls);
            this.groupBox1.Controls.Add(this.Play);
            this.groupBox1.Controls.Add(this.wtLabel);
            this.groupBox1.Controls.Add(this.wtSlider);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.ResUpDown);
            this.groupBox1.Controls.Add(this.ShowCsection);
            this.groupBox1.Controls.Add(this.SloxelUpDown);
            this.groupBox1.Controls.Add(this.LayerUpDown);
            this.groupBox1.Controls.Add(this.SliceButton);
            this.groupBox1.Controls.Add(this.singleStep);
            this.groupBox1.Controls.Add(this.ShowGCD);
            this.groupBox1.Controls.Add(this.SloxelNumber);
            this.groupBox1.Controls.Add(this.ShowSloxels);
            this.groupBox1.Controls.Add(this.WallThickness);
            this.groupBox1.Controls.Add(this.LayerTrackbar);
            this.groupBox1.Controls.Add(this.labelSlox);
            this.groupBox1.Controls.Add(this.SloxelReadout);
            this.groupBox1.Controls.Add(this.None);
            this.groupBox1.Controls.Add(this.ByGcdLayers);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(453, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(355, 432);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Slice Parameters";
            // 
            // Play
            // 
            this.Play.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Play.Location = new System.Drawing.Point(14, 348);
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(75, 23);
            this.Play.TabIndex = 20;
            this.Play.Text = "Play";
            this.Play.UseVisualStyleBackColor = true;
            this.Play.Click += new System.EventHandler(this.Play_Click);
            // 
            // wtLabel
            // 
            this.wtLabel.AutoSize = true;
            this.wtLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.wtLabel.Location = new System.Drawing.Point(119, 400);
            this.wtLabel.Name = "wtLabel";
            this.wtLabel.Size = new System.Drawing.Size(72, 12);
            this.wtLabel.TabIndex = 19;
            this.wtLabel.Text = "Wall Thickness: ";
            // 
            // wtSlider
            // 
            this.wtSlider.AutoSize = false;
            this.wtSlider.Location = new System.Drawing.Point(121, 377);
            this.wtSlider.Maximum = 100;
            this.wtSlider.Name = "wtSlider";
            this.wtSlider.Size = new System.Drawing.Size(210, 20);
            this.wtSlider.TabIndex = 18;
            this.wtSlider.Scroll += new System.EventHandler(this.wtSlider_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(88, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "Resolution: ";
            // 
            // ResUpDown
            // 
            this.ResUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResUpDown.Location = new System.Drawing.Point(144, 21);
            this.ResUpDown.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.ResUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ResUpDown.Name = "ResUpDown";
            this.ResUpDown.Size = new System.Drawing.Size(46, 18);
            this.ResUpDown.TabIndex = 16;
            this.ResUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.ResUpDown.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // ShowCsection
            // 
            this.ShowCsection.AutoSize = true;
            this.ShowCsection.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowCsection.Location = new System.Drawing.Point(170, 355);
            this.ShowCsection.Name = "ShowCsection";
            this.ShowCsection.Size = new System.Drawing.Size(35, 14);
            this.ShowCsection.TabIndex = 15;
            this.ShowCsection.Text = "CS";
            this.ShowCsection.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.ShowCsection.UseVisualStyleBackColor = true;
            this.ShowCsection.CheckedChanged += new System.EventHandler(this.ShowCsection_CheckedChanged);
            // 
            // SloxelUpDown
            // 
            this.SloxelUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SloxelUpDown.Location = new System.Drawing.Point(266, 87);
            this.SloxelUpDown.Name = "SloxelUpDown";
            this.SloxelUpDown.Size = new System.Drawing.Size(83, 18);
            this.SloxelUpDown.TabIndex = 14;
            this.SloxelUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SloxelUpDown.ValueChanged += new System.EventHandler(this.SloxelUpDown_ValueChanged);
            // 
            // LayerUpDown
            // 
            this.LayerUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LayerUpDown.Location = new System.Drawing.Point(266, 48);
            this.LayerUpDown.Name = "LayerUpDown";
            this.LayerUpDown.Size = new System.Drawing.Size(83, 18);
            this.LayerUpDown.TabIndex = 13;
            this.LayerUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.LayerUpDown.ValueChanged += new System.EventHandler(this.LayerUpDown_ValueChanged);
            // 
            // ShowBalls
            // 
            this.ShowBalls.AutoSize = true;
            this.ShowBalls.Checked = true;
            this.ShowBalls.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowBalls.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowBalls.Location = new System.Drawing.Point(270, 355);
            this.ShowBalls.Name = "ShowBalls";
            this.ShowBalls.Size = new System.Drawing.Size(46, 14);
            this.ShowBalls.TabIndex = 21;
            this.ShowBalls.Text = "CCAT";
            this.ShowBalls.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.ShowBalls.UseVisualStyleBackColor = true;
            this.ShowBalls.CheckedChanged += new System.EventHandler(this.ShowBalls_CheckedChanged);
            // 
            // SlicerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 457);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "SlicerForm";
            this.Text = " ";
            ((System.ComponentModel.ISupportInitialize)(this.SloxelNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LayerTrackbar)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.wtSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ResUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SloxelUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LayerUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Button SliceButton;
        public System.Windows.Forms.TrackBar LayerTrackbar;
        public System.Windows.Forms.RadioButton None;
        public System.Windows.Forms.RadioButton WallThickness;
        public System.Windows.Forms.RadioButton ByGcdLayers;
        public System.Windows.Forms.RadioButton singleStep;
        public System.Windows.Forms.TrackBar SloxelNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelSlox;
        private System.Windows.Forms.Label SloxelReadout;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.CheckBox ShowGCD;
        private System.Windows.Forms.CheckBox ShowSloxels;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown SloxelUpDown;
        public System.Windows.Forms.NumericUpDown LayerUpDown;
        public System.Windows.Forms.CheckBox ShowCsection;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown ResUpDown;
        private System.Windows.Forms.Label wtLabel;
        public System.Windows.Forms.TrackBar wtSlider;
        public System.Windows.Forms.Button Play;
        public System.Windows.Forms.CheckBox ShowBalls;
    }
}

