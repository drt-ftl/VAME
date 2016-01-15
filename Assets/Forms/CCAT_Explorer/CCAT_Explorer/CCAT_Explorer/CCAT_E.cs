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
            et /= 100f;
            ThresholdChanged(et);
            ccatExplorerMode = CcatExplorerMode.Error;
        }

        public enum CcatExplorerMode { Temperature, Error, None }
        public static CcatExplorerMode ccatExplorerMode;

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
            et /= 100f;
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

        public delegate void ThresholdChangedHandler(float _th);
        public static event ThresholdChangedHandler thresholdChanged;

        public static void ThresholdChanged(float _th)
        {
            if (thresholdChanged != null)
                thresholdChanged(_th);
        }

        private void radioButton_temp_CheckedChanged(object sender, EventArgs e)
        {
            ccatExplorerMode = CcatExplorerMode.Temperature;
            if (InspectorT.slicerForm != null)
                InspectorT.slicerForm.panel1.Invalidate();
        }

        private void radioButton_Error_CheckedChanged(object sender, EventArgs e)
        {
            ccatExplorerMode = CcatExplorerMode.Error;
            if (InspectorT.slicerForm != null)
                InspectorT.slicerForm.panel1.Invalidate();
        }

        private void radioButton_none_CheckedChanged(object sender, EventArgs e)
        {
            ccatExplorerMode = CcatExplorerMode.None;
            if (InspectorT.slicerForm != null)
                InspectorT.slicerForm.panel1.Invalidate();
        }
    }
}
