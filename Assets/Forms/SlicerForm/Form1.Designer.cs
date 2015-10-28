namespace SlicerForm
{
    partial class Form1
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
            this.None = new System.Windows.Forms.RadioButton();
            this.WallThickness = new System.Windows.Forms.RadioButton();
            this.ByGcdLayers = new System.Windows.Forms.RadioButton();
            this.singleStep = new System.Windows.Forms.RadioButton();
            this.SliceButton = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.transparency = new System.Windows.Forms.TrackBar();
            this.labelVis = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SlicerEC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.transparency)).BeginInit();
            this.SuspendLayout();
            // 
            // SlicerEC
            // 
            this.SlicerEC.ButtonSize = MakarovDev.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonSize.Normal;
            this.SlicerEC.ButtonStyle = MakarovDev.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonStyle.Circle;
            this.SlicerEC.Controls.Add(this.label1);
            this.SlicerEC.Controls.Add(this.labelVis);
            this.SlicerEC.Controls.Add(this.transparency);
            this.SlicerEC.Controls.Add(this.trackBar1);
            this.SlicerEC.Controls.Add(this.None);
            this.SlicerEC.Controls.Add(this.WallThickness);
            this.SlicerEC.Controls.Add(this.ByGcdLayers);
            this.SlicerEC.Controls.Add(this.singleStep);
            this.SlicerEC.Controls.Add(this.SliceButton);
            this.SlicerEC.ExpandedHeight = 0;
            this.SlicerEC.IsExpanded = true;
            this.SlicerEC.Location = new System.Drawing.Point(13, 13);
            this.SlicerEC.Name = "SlicerEC";
            this.SlicerEC.Size = new System.Drawing.Size(259, 319);
            this.SlicerEC.TabIndex = 0;
            this.SlicerEC.Text = "Slicer";
            this.SlicerEC.UseAnimation = true;
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
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(3, 144);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(253, 45);
            this.trackBar1.TabIndex = 2;
            // 
            // transparency
            // 
            this.transparency.Location = new System.Drawing.Point(3, 86);
            this.transparency.Maximum = 100;
            this.transparency.Name = "transparency";
            this.transparency.Size = new System.Drawing.Size(253, 45);
            this.transparency.TabIndex = 7;
            // 
            // labelVis
            // 
            this.labelVis.AutoSize = true;
            this.labelVis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelVis.Location = new System.Drawing.Point(4, 117);
            this.labelVis.Name = "labelVis";
            this.labelVis.Size = new System.Drawing.Size(49, 13);
            this.labelVis.TabIndex = 8;
            this.labelVis.Text = "Visibility: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(4, 177);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Number: ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 457);
            this.Controls.Add(this.SlicerEC);
            this.Name = "Form1";
            this.Text = "Form1";
            this.SlicerEC.ResumeLayout(false);
            this.SlicerEC.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.transparency)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private MakarovDev.ExpandCollapsePanel.ExpandCollapsePanel SlicerEC;
        public System.Windows.Forms.Button SliceButton;
        public System.Windows.Forms.TrackBar trackBar1;
        public System.Windows.Forms.RadioButton None;
        public System.Windows.Forms.RadioButton WallThickness;
        public System.Windows.Forms.RadioButton ByGcdLayers;
        public System.Windows.Forms.RadioButton singleStep;
        public System.Windows.Forms.TrackBar transparency;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelVis;
    }
}

