namespace CCAT_Explorer
{
    partial class CCAT_E
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
            this.label1 = new System.Windows.Forms.Label();
            this.showWhenLaserOff = new System.Windows.Forms.CheckBox();
            this.ErrorThreshold = new System.Windows.Forms.TrackBar();
            this.ErrorThresholdLabel = new System.Windows.Forms.Label();
            this.VisibilityLabel = new System.Windows.Forms.Label();
            this.Visibility = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Visibility)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.ForeColor = System.Drawing.Color.Chartreuse;
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(5);
            this.label1.Size = new System.Drawing.Size(192, 100);
            this.label1.TabIndex = 0;
            // 
            // showWhenLaserOff
            // 
            this.showWhenLaserOff.AutoSize = true;
            this.showWhenLaserOff.Location = new System.Drawing.Point(17, 261);
            this.showWhenLaserOff.Name = "showWhenLaserOff";
            this.showWhenLaserOff.Size = new System.Drawing.Size(122, 17);
            this.showWhenLaserOff.TabIndex = 1;
            this.showWhenLaserOff.Text = "Show when laser off";
            this.showWhenLaserOff.UseVisualStyleBackColor = true;
            // 
            // ErrorThreshold
            // 
            this.ErrorThreshold.AutoSize = false;
            this.ErrorThreshold.Location = new System.Drawing.Point(12, 194);
            this.ErrorThreshold.Maximum = 100;
            this.ErrorThreshold.Name = "ErrorThreshold";
            this.ErrorThreshold.Size = new System.Drawing.Size(192, 25);
            this.ErrorThreshold.TabIndex = 2;
            this.ErrorThreshold.Value = 0;
            this.ErrorThreshold.Scroll += new System.EventHandler(this.ErrorThreshold_Scroll);
            // 
            // ErrorThresholdLabel
            // 
            this.ErrorThresholdLabel.AutoSize = true;
            this.ErrorThresholdLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorThresholdLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ErrorThresholdLabel.Location = new System.Drawing.Point(15, 221);
            this.ErrorThresholdLabel.Name = "ErrorThresholdLabel";
            this.ErrorThresholdLabel.Size = new System.Drawing.Size(70, 12);
            this.ErrorThresholdLabel.TabIndex = 3;
            this.ErrorThresholdLabel.Text = "ErrorThreshold: ";
            // 
            // VisibilityLabel
            // 
            this.VisibilityLabel.AutoSize = true;
            this.VisibilityLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VisibilityLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.VisibilityLabel.Location = new System.Drawing.Point(15, 181);
            this.VisibilityLabel.Name = "VisibilityLabel";
            this.VisibilityLabel.Size = new System.Drawing.Size(45, 12);
            this.VisibilityLabel.TabIndex = 5;
            this.VisibilityLabel.Text = "Visibility: ";
            // 
            // Visibility
            // 
            this.Visibility.AutoSize = false;
            this.Visibility.Location = new System.Drawing.Point(12, 154);
            this.Visibility.Maximum = 100;
            this.Visibility.Name = "Visibility";
            this.Visibility.Size = new System.Drawing.Size(192, 25);
            this.Visibility.TabIndex = 4;
            this.Visibility.Value = 100;
            this.Visibility.Scroll += new System.EventHandler(this.Visibility_Scroll);
            // 
            // CCAT_E
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 290);
            this.Controls.Add(this.VisibilityLabel);
            this.Controls.Add(this.Visibility);
            this.Controls.Add(this.ErrorThresholdLabel);
            this.Controls.Add(this.ErrorThreshold);
            this.Controls.Add(this.showWhenLaserOff);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "CCAT_E";
            this.Text = "CCAT Explorer";
            ((System.ComponentModel.ISupportInitialize)(this.ErrorThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Visibility)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.CheckBox showWhenLaserOff;
        public System.Windows.Forms.TrackBar ErrorThreshold;
        private System.Windows.Forms.Label ErrorThresholdLabel;
        private System.Windows.Forms.Label VisibilityLabel;
        public System.Windows.Forms.TrackBar Visibility;
        public float et = 0;
    }
}

