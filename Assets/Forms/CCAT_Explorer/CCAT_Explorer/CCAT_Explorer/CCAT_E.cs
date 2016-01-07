using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCAT_Explorer
{
    public partial class CCAT_E : Form
    {
        public CCAT_E()
        {
            InitializeComponent();
            et = (float)ErrorThreshold.Value;
            et /= 10000f;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            e.Cancel = true;
            this.Hide();
        }

        private void ErrorThreshold_Scroll(object sender, EventArgs e)
        {
            et = (float)ErrorThreshold.Value;
            et /= 10000f;
            ErrorThresholdLabel.Text = "Error Threshold: " + et.ToString("f3") + "in";
            ThresholdChanged(et);
        }

        private void Visibility_Scroll(object sender, EventArgs e)
        {
            VisibilityLabel.Text = "Visibility: " + Visibility.Value.ToString() + "%";
            VisChanged((float)Visibility.Value / 100f);
        }

        public delegate void VisChangedHandler(float _vis);
        public static event VisChangedHandler visChanged;

        public static void VisChanged(float _vis)
        {
            if (visChanged != null)
                visChanged(_vis);
        }

        public delegate void ThresholdChangedHandler(float _vis);
        public static event ThresholdChangedHandler thresholdChanged;

        public static void ThresholdChanged(float _vis)
        {
            if (thresholdChanged != null)
                thresholdChanged(_vis);
        }
    }
}
