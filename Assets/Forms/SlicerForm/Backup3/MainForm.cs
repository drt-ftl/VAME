using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Diagnostics;

using Sano.PersonalProjects.ColorPicker.Controls;
using Sano.Utility;

namespace Sano.PersonalProjects.ColorPicker.Main {
	/// <summary>
	/// Summary description for MainForm.
	/// </summary>
	public class MainForm : Form {
		private ColorPanel colorPanel1;
		private MenuItem menuItem1;
		private MenuItem menuItem2;
		private MainMenu mainMenu;
		private MenuItem menuItem3;

		public MainForm() {
			InitializeComponent();
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.Run(new MainForm());
		}

		private void menuItem2_Click(object sender, System.EventArgs e) {
		
			using ( AboutForm af = new AboutForm() ) {
				af.ShowDialog( this );
			}

		}

		private void menuItem3_Click(object sender, System.EventArgs e) {
			System.Diagnostics.Process.Start( "http://sano.dotnetgeeks.net/colorpicker" );		
		}

		private void MainForm_Resize(object sender, System.EventArgs e) {
		
			if ( this.WindowState == FormWindowState.Minimized ) {
				this.Hide();
			}
		}

		private void cmExit_Click(object sender, System.EventArgs e) {
			this.Close();
		}

		private void mnuClose_Click(object sender, System.EventArgs e) {
			this.Close();
		}

		#region Dispose

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (colorPanel1 != null) {
					colorPanel1.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion Dispose

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.colorPanel1 = new Sano.PersonalProjects.ColorPicker.Controls.ColorPanel();
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// colorPanel1
			// 
			this.colorPanel1.AllowDrop = true;
			this.colorPanel1.Location = new System.Drawing.Point(2, 14);
			this.colorPanel1.Name = "colorPanel1";
			this.colorPanel1.Size = new System.Drawing.Size(572, 272);
			this.colorPanel1.TabIndex = 0;
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuItem1});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem2,
																					  this.menuItem3});
			this.menuItem1.Text = "&Help";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "About ColorPicker.NET";
			this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.Text = "Homepage";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(576, 301);
			this.Controls.Add(this.colorPanel1);
			this.MaximizeBox = false;
			this.Menu = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "ColorPicker.NET";
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.ResumeLayout(false);

		}
		#endregion

	}
}
