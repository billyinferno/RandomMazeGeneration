﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandomMazeGeneration
{
    public partial class frmMazeInput : Form
    {
        public frmMazeInput()
        {
            InitializeComponent();
        }

        private void cmdGenerate_Click(object sender, EventArgs e)
        {
            // check whether both X and Y already filled by user
            if (txtX.Text.Trim().Length > 0 && txtY.Text.Trim().Length > 0)
            {
                int X, Y;
                // ensure that we can parse both number, and since the form will be generated too big
                // we will limit the form size into 1280 x 720, which means that:
                // X -> 42 -> 42 * 30 = 1260
                // Y -> 24 -> 24 * 30 = 720
                //
                // first get both X and Y
                try
                {
                    X = int.Parse(this.txtX.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error when parsing X value.\n" + ex.Message, "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    Y = int.Parse(this.txtY.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error when parsing Y value.\n" + ex.Message, "Parsing Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ensure that the value is between limit
                if (!((X > 0) && (X <= 42)))
                {
                    MessageBox.Show("Invalid value for X.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!((Y > 0) && (Y <= 24)))
                {
                    MessageBox.Show("Invalid value for Y.", "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // all value is correct, now create the new form, and close this form
                frmMazeBoard frm = new frmMazeBoard(X, Y);
                frm.ShowDialog();

                // set the form into null
                frm.Dispose();
                frm = null;
            }
            else
            {
                // fields is missing
                MessageBox.Show("Please fill all fields needed for generating the maze.", "Fields Blank", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
