using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace WindowsApplication1
{
	/// <summary>
	/// Summary description for ColorSliderForm.
	/// </summary>
public class ColorSliderForm : System.Windows.Forms.Form
{
	private Sano.PersonalProjects.ColorPicker.Controls.ColorSlider colorSlider1;
	private System.Windows.Forms.TextBox textBox1;
	/// <summary>
	/// Required designer variable.
	/// </summary>
	private System.ComponentModel.Container components = null;

	public ColorSliderForm()
	{
		//
		// Required for Windows Form Designer support
		//
		InitializeComponent();

		textBox1.Text = "255";
		
		using ( Graphics g = Graphics.FromImage( colorSlider1.ColorBitmap ) ) {

			Color startColor = Color.FromArgb( 0, 0, 0 );
			Color endColor = Color.FromArgb( 255, 0, 0 );
			
			Rectangle region = new Rectangle( 0, 0, 18, 300 );
			using ( LinearGradientBrush lgb = new LinearGradientBrush( region, startColor, endColor, 270f ) ) {
				g.FillRectangle( lgb, region );
			}
		
		}
	
	}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ColorSliderForm));
			this.colorSlider1 = new Sano.PersonalProjects.ColorPicker.Controls.ColorSlider();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// colorSlider1
			// 
			this.colorSlider1.Location = new System.Drawing.Point(8, 16);
			this.colorSlider1.Name = "colorSlider1";
			this.colorSlider1.Size = new System.Drawing.Size(64, 328);
			this.colorSlider1.TabIndex = 0;
			this.colorSlider1.ValueChanged += new Sano.PersonalProjects.ColorPicker.Controls.ValueChangedHandler(this.colorSlider1_ValueChanged);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(80, 24);
			this.textBox1.Name = "textBox1";
			this.textBox1.TabIndex = 1;
			this.textBox1.Text = "textBox1";
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(208, 293);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.colorSlider1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new ColorSliderForm());
		}

		private void colorSlider1_ValueChanged(object sender, Sano.PersonalProjects.ColorPicker.Controls.ValueChangedEventArgs e) {
			textBox1.Text = e.Value.ToString();
		}
	}
}
