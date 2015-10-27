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
            this.SliceButton = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.SlicerEC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // SlicerEC
            // 
            this.SlicerEC.ButtonSize = MakarovDev.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonSize.Normal;
            this.SlicerEC.ButtonStyle = MakarovDev.ExpandCollapsePanel.ExpandCollapseButton.ExpandButtonStyle.Circle;
            this.SlicerEC.Controls.Add(this.trackBar1);
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
            // SliceButton
            // 
            this.SliceButton.Location = new System.Drawing.Point(23, 57);
            this.SliceButton.Name = "SliceButton";
            this.SliceButton.Size = new System.Drawing.Size(75, 23);
            this.SliceButton.TabIndex = 1;
            this.SliceButton.Text = "Slice";
            this.SliceButton.UseVisualStyleBackColor = true;
            this.SliceButton.Click += new System.EventHandler(this.SliceButton_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(23, 117);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(220, 45);
            this.trackBar1.TabIndex = 2;
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
            this.ResumeLayout(false);

        }

        #endregion

        private MakarovDev.ExpandCollapsePanel.ExpandCollapsePanel SlicerEC;
        public System.Windows.Forms.Button SliceButton;
        public System.Windows.Forms.TrackBar trackBar1;
    }
}

