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
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton_temp = new System.Windows.Forms.RadioButton();
            this.radioButton_Error = new System.Windows.Forms.RadioButton();
            this.radioButton_none = new System.Windows.Forms.RadioButton();
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
            this.showWhenLaserOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.showWhenLaserOff.Location = new System.Drawing.Point(19, 402);
            this.showWhenLaserOff.Name = "showWhenLaserOff";
            this.showWhenLaserOff.Size = new System.Drawing.Size(108, 16);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(15, 250);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "Color By:";
            // 
            // radioButton_temp
            // 
            this.radioButton_temp.AutoSize = true;
            this.radioButton_temp.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_temp.Location = new System.Drawing.Point(19, 266);
            this.radioButton_temp.Name = "radioButton_temp";
            this.radioButton_temp.Size = new System.Drawing.Size(75, 16);
            this.radioButton_temp.TabIndex = 7;
            this.radioButton_temp.TabStop = true;
            this.radioButton_temp.Text = "Temperature";
            this.radioButton_temp.UseVisualStyleBackColor = true;
            this.radioButton_temp.CheckedChanged += new System.EventHandler(this.radioButton_temp_CheckedChanged);
            // 
            // radioButton_Error
            // 
            this.radioButton_Error.AutoSize = true;
            this.radioButton_Error.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_Error.Location = new System.Drawing.Point(19, 288);
            this.radioButton_Error.Name = "radioButton_Error";
            this.radioButton_Error.Size = new System.Drawing.Size(43, 16);
            this.radioButton_Error.TabIndex = 8;
            this.radioButton_Error.TabStop = true;
            this.radioButton_Error.Text = "Error";
            this.radioButton_Error.UseVisualStyleBackColor = true;
            this.radioButton_Error.CheckedChanged += new System.EventHandler(this.radioButton_Error_CheckedChanged);
            // 
            // radioButton_none
            // 
            this.radioButton_none.AutoSize = true;
            this.radioButton_none.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_none.Location = new System.Drawing.Point(19, 310);
            this.radioButton_none.Name = "radioButton_none";
            this.radioButton_none.Size = new System.Drawing.Size(45, 16);
            this.radioButton_none.TabIndex = 9;
            this.radioButton_none.TabStop = true;
            this.radioButton_none.Text = "None";
            this.radioButton_none.UseVisualStyleBackColor = true;
            this.radioButton_none.CheckedChanged += new System.EventHandler(this.radioButton_none_CheckedChanged);
            // 
            // CCAT_E
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(219, 430);
            this.Controls.Add(this.radioButton_none);
            this.Controls.Add(this.radioButton_Error);
            this.Controls.Add(this.radioButton_temp);
            this.Controls.Add(this.label2);
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
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.RadioButton radioButton_temp;
        public System.Windows.Forms.RadioButton radioButton_Error;
        public System.Windows.Forms.RadioButton radioButton_none;
    }
}

