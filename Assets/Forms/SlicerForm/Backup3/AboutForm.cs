
using System;
using System.Windows.Forms;

namespace Sano.PersonalProjects.ColorPicker.Main {

	public class AboutForm : Form {
		private System.Windows.Forms.Label lblLogo;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label lblAbout;
		private System.Windows.Forms.Label lblApplicationName;

		private void InitializeComponent() {

			this.lblLogo = new System.Windows.Forms.Label();
			this.lblApplicationName = new System.Windows.Forms.Label();
			this.lblVersion = new System.Windows.Forms.Label();
			this.lblAbout = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// lblLogo
			// 
			this.lblLogo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblLogo.Location = new System.Drawing.Point(8, 11);
			this.lblLogo.Name = "lblLogo";
			this.lblLogo.Size = new System.Drawing.Size(64, 64);
			this.lblLogo.TabIndex = 0;
			this.lblLogo.Text = "Logo";
			this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblApplicationName
			// 
			this.lblApplicationName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lblApplicationName.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblApplicationName.Location = new System.Drawing.Point(78, 11);
			this.lblApplicationName.Name = "lblApplicationName";
			this.lblApplicationName.Size = new System.Drawing.Size(178, 16);
			this.lblApplicationName.TabIndex = 3;
			// 
			// lblVersion
			// 
			this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lblVersion.Font = new System.Drawing.Font("Verdana", 7F);
			this.lblVersion.Location = new System.Drawing.Point(80, 29);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(172, 22);
			this.lblVersion.TabIndex = 2;
			// 
			// lblAbout
			// 
			this.lblAbout.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.lblAbout.Location = new System.Drawing.Point(80, 48);
			this.lblAbout.Name = "lblAbout";
			this.lblAbout.Size = new System.Drawing.Size(170, 30);
			this.lblAbout.TabIndex = 4;
			// 
			// AboutForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(266, 87);
			this.Controls.Add(this.lblAbout);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.lblApplicationName);
			this.Controls.Add(this.lblLogo);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.Text = "About ColorPicker.NET";
			this.ResumeLayout(false);

		}

		public AboutForm() {

			InitializeComponent();
			
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.CenterParent;

			lblApplicationName.Text = System.Windows.Forms.Application.ProductName;
			lblVersion.Text = String.Format( "v.{0}", System.Windows.Forms.Application.ProductVersion );
			lblAbout.Text = "Copyright © 2004-05 Chris Sano\ncsano@microsoft.com"; 

		}

	}

}