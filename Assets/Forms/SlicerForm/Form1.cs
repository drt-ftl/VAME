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
    public partial class Form1 : Form
    {

        public Form1()
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
            ButtonPressed("Slice", true);
        }
    }
}
