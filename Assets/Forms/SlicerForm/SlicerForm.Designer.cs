

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
            this.SlicerEC = new MakarovDev.ExpandCollapsePanel.ExpandCollapsePanel();
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
            this.ShowSloxels = new System.Windows.Forms.CheckBox();
            this.ShowGCD = new System.Windows.Forms.CheckBox();
            this.SlicerEC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SloxelNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LayerTrackbar)).BeginInit();
            this.SuspendLayout();
            // 
            // SlicerEC
            // 
            this.SlicerEC.ButtonSize = MakarovDev.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonSize.Normal;
            this.SlicerEC.ButtonStyle = MakarovDev.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonStyle.Circle;
            this.SlicerEC.Controls.Add(this.ShowGCD);
            this.SlicerEC.Controls.Add(this.ShowSloxels);
            this.SlicerEC.Controls.Add(this.SloxelReadout);
            this.SlicerEC.Controls.Add(this.label1);
            this.SlicerEC.Controls.Add(this.labelSlox);
            this.SlicerEC.Controls.Add(this.SloxelNumber);
            this.SlicerEC.Controls.Add(this.LayerTrackbar);
            this.SlicerEC.Controls.Add(this.singleStep);
            this.SlicerEC.Controls.Add(this.SliceButton);
            this.SlicerEC.Controls.Add(this.WallThickness);
            this.SlicerEC.Controls.Add(this.None);
            this.SlicerEC.Controls.Add(this.ByGcdLayers);
            this.SlicerEC.ExpandedHeight = 0;
            this.SlicerEC.IsExpanded = true;
            this.SlicerEC.Location = new System.Drawing.Point(491, 12);
            this.SlicerEC.Name = "SlicerEC";
            this.SlicerEC.Size = new System.Drawing.Size(331, 433);
            this.SlicerEC.TabIndex = 0;
            this.SlicerEC.Text = "Slicer";
            this.SlicerEC.UseAnimation = true;
            this.SlicerEC.Paint += new System.Windows.Forms.PaintEventHandler(this.SlicerEC_Paint);
            // 
            // SloxelReadout
            // 
            this.SloxelReadout.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.SloxelReadout.Location = new System.Drawing.Point(118, 212);
            this.SloxelReadout.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.SloxelReadout.Name = "SloxelReadout";
            this.SloxelReadout.Size = new System.Drawing.Size(213, 221);
            this.SloxelReadout.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(4, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Layer: ";
            // 
            // labelSlox
            // 
            this.labelSlox.AutoSize = true;
            this.labelSlox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelSlox.Location = new System.Drawing.Point(4, 168);
            this.labelSlox.Name = "labelSlox";
            this.labelSlox.Size = new System.Drawing.Size(41, 13);
            this.labelSlox.TabIndex = 8;
            this.labelSlox.Text = "Sloxel: ";
            // 
            // SloxelNumber
            // 
            this.SloxelNumber.Location = new System.Drawing.Point(3, 137);
            this.SloxelNumber.Maximum = 100;
            this.SloxelNumber.Name = "SloxelNumber";
            this.SloxelNumber.Size = new System.Drawing.Size(253, 45);
            this.SloxelNumber.TabIndex = 7;
            this.SloxelNumber.Scroll += new System.EventHandler(this.SloxelNumber_Scroll);
            // 
            // LayerTrackbar
            // 
            this.LayerTrackbar.Location = new System.Drawing.Point(3, 86);
            this.LayerTrackbar.Name = "LayerTrackbar";
            this.LayerTrackbar.Size = new System.Drawing.Size(253, 45);
            this.LayerTrackbar.TabIndex = 2;
            this.LayerTrackbar.Scroll += new System.EventHandler(this.LayerTrackbar_Scroll);
            // 
            // singleStep
            // 
            this.singleStep.AutoSize = true;
            this.singleStep.Location = new System.Drawing.Point(3, 207);
            this.singleStep.Name = "singleStep";
            this.singleStep.Size = new System.Drawing.Size(88, 19);
            this.singleStep.TabIndex = 3;
            this.singleStep.TabStop = true;
            this.singleStep.Text = "Single Step";
            this.singleStep.UseVisualStyleBackColor = true;
            this.singleStep.CheckedChanged += new System.EventHandler(this.radioButtonSingleStep_CheckedChanged);
            // 
            // SliceButton
            // 
            this.SliceButton.Location = new System.Drawing.Point(3, 57);
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
            this.WallThickness.Location = new System.Drawing.Point(3, 257);
            this.WallThickness.Name = "WallThickness";
            this.WallThickness.Size = new System.Drawing.Size(104, 19);
            this.WallThickness.TabIndex = 5;
            this.WallThickness.TabStop = true;
            this.WallThickness.Text = "WallThickness";
            this.WallThickness.UseVisualStyleBackColor = true;
            this.WallThickness.CheckedChanged += new System.EventHandler(this.WallThickness_CheckedChanged);
            // 
            // None
            // 
            this.None.AutoSize = true;
            this.None.Location = new System.Drawing.Point(3, 282);
            this.None.Name = "None";
            this.None.Size = new System.Drawing.Size(55, 19);
            this.None.TabIndex = 6;
            this.None.TabStop = true;
            this.None.Text = "None";
            this.None.UseVisualStyleBackColor = true;
            this.None.CheckedChanged += new System.EventHandler(this.None_CheckedChanged);
            // 
            // ByGcdLayers
            // 
            this.ByGcdLayers.AutoSize = true;
            this.ByGcdLayers.Location = new System.Drawing.Point(3, 232);
            this.ByGcdLayers.Name = "ByGcdLayers";
            this.ByGcdLayers.Size = new System.Drawing.Size(90, 19);
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
            // 
            // ShowSloxels
            // 
            this.ShowSloxels.AutoSize = true;
            this.ShowSloxels.Checked = true;
            this.ShowSloxels.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowSloxels.Location = new System.Drawing.Point(3, 323);
            this.ShowSloxels.Name = "ShowSloxels";
            this.ShowSloxels.Size = new System.Drawing.Size(100, 19);
            this.ShowSloxels.TabIndex = 11;
            this.ShowSloxels.Text = "Show Sloxels";
            this.ShowSloxels.UseVisualStyleBackColor = true;
            this.ShowSloxels.CheckedChanged += new System.EventHandler(this.ShowSloxels_CheckedChanged);
            // 
            // ShowGCD
            // 
            this.ShowGCD.AutoSize = true;
            this.ShowGCD.Checked = true;
            this.ShowGCD.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowGCD.Location = new System.Drawing.Point(3, 348);
            this.ShowGCD.Name = "ShowGCD";
            this.ShowGCD.Size = new System.Drawing.Size(86, 19);
            this.ShowGCD.TabIndex = 12;
            this.ShowGCD.Text = "Show GCD";
            this.ShowGCD.UseVisualStyleBackColor = true;
            this.ShowGCD.CheckedChanged += new System.EventHandler(this.ShowGCD_CheckedChanged);
            // 
            // SlicerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 457);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.SlicerEC);
            this.Name = "SlicerForm";
            this.Text = "Form1";
            this.SlicerEC.ResumeLayout(false);
            this.SlicerEC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SloxelNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LayerTrackbar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MakarovDev.ExpandCollapsePanel.ExpandCollapsePanel SlicerEC;
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
        private System.Windows.Forms.CheckBox ShowGCD;
        private System.Windows.Forms.CheckBox ShowSloxels;
    }
}

