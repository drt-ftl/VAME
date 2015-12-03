using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using UnityEngine;

namespace SlicerForm
{
    public partial class SlicerForm : Form
    {
        private bool sliced = false;
        float scale = 300f;

        public SlicerForm()
        {
            InitializeComponent();
        }

        public delegate void ButtonHandler(string _name, bool _toggle);
        public static event ButtonHandler buttonPressed;

        public static void ButtonPressed(string _name, bool _toggle)
        {
            if (buttonPressed != null)
                buttonPressed(_name, _toggle);
        }

        private void SliceButton_Click(object sender, EventArgs e)
        {
            if (sliced) return;
            ButtonPressed("Slice", true);
            sliced = true;
        }

        private void radioButtonSingleStep_CheckedChanged(object sender, EventArgs e)
        {
            ButtonPressed("Radio_step", true);
        }

        private void radioButtonByGCD_CheckedChanged(object sender, EventArgs e)
        {
            ButtonPressed("Radio_gcd", true);
        }

        private void None_CheckedChanged(object sender, EventArgs e)
        {
            ButtonPressed("Radio_none", true);
        }

        private void WallThickness_CheckedChanged(object sender, EventArgs e)
        {
            ButtonPressed("Radio_wt", true);
            cSectionGCD.csMode = cSectionGCD.CsMode.WallThickness;
        }

        private void SlicerEC_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            e.Cancel = true;
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (InspectorR.voxelsLoaded)
            {
                LayerTrackbar.Maximum = cSectionGCD.layerHeights.Count - 1;
                var sloxList = cSectionGCD.layers[cSectionGCD.layerHeights[LayerTrackbar.Value]].Sloxels;
                SloxelNumber.Maximum = sloxList.Count - 1;
                System.Drawing.Graphics g = panel1.CreateGraphics();
                System.Drawing.Pen p = new System.Drawing.Pen(System.Drawing.Color.Red);
                System.Drawing.Pen pen2 = new System.Drawing.Pen(System.Drawing.Color.Aquamarine);
                var center = new Vector2(panel1.Size.Width / 2, panel1.Size.Height / 2);
                var dim = (0.5f / cSectionGCD.sloxelResolution.x) * scale;
                var readout = "";
                if (ShowGCD.Checked)
                {
                    foreach (var gcdLine in cSectionGCD.layers[cSectionGCD.layerHeights[LayerTrackbar.Value]].gcdLines)
                    {
                        if (gcdLine.MovesInX || gcdLine.MovesInZ)
                        {
                            var _p1 = gcdLine.p1 * scale;
                            var _p2 = gcdLine.p2 * scale;
                            var p1 = new Point((int)_p1.x + (int)center.x, (int)_p1.z + (int)center.y);
                            var p2 = new Point((int)_p2.x + (int)center.x, (int)_p2.z + (int)center.y);
                            g.DrawLine(pen2, p1, p2);
                        }
                    }
                }
                if (ShowSloxels.Checked)
                {
                    foreach (var sloxel in sloxList)
                    {
                        var x = center.x + sloxel.Position.x * scale;
                        var y = center.y + sloxel.Position.z * scale;
                        g.DrawRectangle(p, x - dim, y - dim, dim * 2, dim * 2);
                    }
                    var hSlox = sloxList[SloxelNumber.Value];
                    p = new System.Drawing.Pen(System.Drawing.Color.Yellow);
                    SolidBrush sbH = new SolidBrush(System.Drawing.Color.Red);
                    var xH = center.x + hSlox.Position.x * scale;
                    var yH = center.y + hSlox.Position.z * scale;
                    g.DrawRectangle(p, xH - dim, yH - dim, dim * 2, dim * 2);
                    InspectorR.highlight = hSlox.Voxel.Id;
                    if (hSlox.IntersectedByLines.Count == 1)
                        readout += "Intersected By 1 Line. \r\n";
                    else
                        readout = "Intersected By " + hSlox.IntersectedByLines.Count.ToString() + " Lines. \r\n";
                    foreach (var IntLine in hSlox.IntersectedByLines)
                    {
                        readout += "\r\n";
                        readout += "Line " + hSlox.IntersectedByLines.IndexOf(IntLine).ToString() + "\r\n";
                        readout += "p1: " + IntLine.p1.ToString("f4") + "\r\n";
                        readout += "p2: " + IntLine.p2.ToString("f4") + "\r\n";
                    }
                }
                SloxelReadout.Text = readout;
            }
        }

        private void LayerTrackbar_Scroll(object sender, EventArgs e)
        {
            LayerUpDown.Value = LayerTrackbar.Value;
            panel1.Invalidate();
        }

        private void SloxelNumber_Scroll(object sender, EventArgs e)
        {
            SloxelUpDown.Value = SloxelNumber.Value;
            panel1.Invalidate();
        }

        private void ShowSloxels_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        private void ShowGCD_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        private void LayerUpDown_ValueChanged(object sender, EventArgs e)
        {
            LayerTrackbar.Value = (int)LayerUpDown.Value;
            panel1.Invalidate();
        }

        private void SloxelUpDown_ValueChanged(object sender, EventArgs e)
        {
            SloxelNumber.Value = (int)SloxelUpDown.Value;
            panel1.Invalidate();
        }

        private void panel1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            
        }
        private void panel1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            var sloxList = cSectionGCD.layers[cSectionGCD.layerHeights[LayerTrackbar.Value]].Sloxels;
            var mPos = e.Location;
            var dim = (0.5f / cSectionGCD.sloxelResolution.x) * scale;
            var center = new Vector2(panel1.Size.Width / 2, panel1.Size.Height / 2);
            foreach (var sloxel in sloxList)
            {
                var sPosRaw = new Vector2(sloxel.Position.x, sloxel.Position.z);
                var sPos = sPosRaw * scale + center;
                if (mPos.X > sPos.x - dim && mPos.X < sPos.x + dim
                    && mPos.Y > sPos.y - dim && mPos.Y < sPos.y + dim)
                {
                    SloxelNumber.Value = sloxList.IndexOf(sloxel);
                    InspectorR.highlightVector = sloxel.Voxel.Origin;
                    panel1.Invalidate();
                }
                //else
                //{
                //    SloxelReadout.Text = "MPos: " + mPos.ToString();
                //    panel1.Invalidate();
                //}
            }
        }
    }
}
