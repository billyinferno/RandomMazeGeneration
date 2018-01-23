using System;
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
    public partial class frmMazeBoard : Form
    {
        private bool FormFinishedPainting;
        private int X, Y;
        private int StartX, StartY, EndX, EndY;
        private Core.Maze MzData;

        public frmMazeBoard(int X, int Y)
        {
            InitializeComponent();

            // resize the form based on the number of X and Y passed to the form
            this.Size = new Size((X * 30) + 16, (Y * 30) + 39);
            this.X = X;
            this.Y = Y;

            // set the FormFinishedPainting into false
            this.FormFinishedPainting = false;

            // set the start location as -1, since we will ask user to click
            // to generate the Start and End point
            this.StartX = -1;
            this.StartY = -1;
            this.EndX = -1;
            this.EndY = -1;

            // initialize the maze data
            this.MzData = new Core.Maze(X, Y);
        }

        private void frmMazeBoard_Load(object sender, EventArgs e)
        {

        }

        private void frmMazeBoard_Paint(object sender, PaintEventArgs e)
        {
            if (!this.FormFinishedPainting)
            {
                this.InitializeMaze();
            }
        }

        private void frmMazeBoard_MouseClick(object sender, MouseEventArgs e)
        {
            // get current location and determine the position on the X, and Y axis
            int MouseX = e.Location.X;
            int MouseY = e.Location.Y;

            int PosX = MouseX / 30;
            if ((MouseX % 30) > 0)
            {
                PosX += 1;
            }

            int PosY = MouseY / 30;
            if ((MouseY % 30) > 0)
            {
                PosY += 1;
            }

            // check the mouse click to determine whether this is start position
            // or end position.
            // left click will indicate Start Position
            // right click will indicate End Position
            if (e.Button == MouseButtons.Left)
            {
                // check if the location is the same as the end?
                // if so reject this location.
                if ((PosX - 1) == this.EndX && (PosY - 1) == this.EndY)
                {
                    MessageBox.Show("Start point cannot be the same as end point.", "Wrong Start Location", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // check if previously the start location already given or not?
                // if yes, then reset the picture of the box first before we draw the start box
                if (this.StartX >= 0 && this.StartY >= 0)
                {
                    this.DrawMaze(0, this.StartX, this.StartY);
                }
                // set the start position, remember that the position will be always calculated
                // from 0, so we need to substract 1 from the position
                this.StartX = (PosX - 1);
                this.StartY = (PosY - 1);

                // draw the maze start position on the form
                this.DrawMaze(16, this.StartX, this.StartY);
            }

            if (e.Button == MouseButtons.Right)
            {
                // check if the location is the same as the start?
                // if so reject this location.
                if ((PosX - 1) == this.StartX && (PosY - 1) == this.StartY)
                {
                    MessageBox.Show("End point cannot be the same as start point.", "Wrong End Location", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // check if previously the end location already given or not?
                // if yes, then reset the picture of the box first before we draw the end box
                if (this.EndX >= 0 && this.EndY >= 0)
                {
                    this.DrawMaze(0, this.EndX, this.EndY);
                }
                // set the end position, remember that the position will be always calculated
                // from 0, so we need to substract 1 from the position
                this.EndX = (PosX - 1);
                this.EndY = (PosY - 1);

                // draw the maze end position on the form
                this.DrawMaze(17, this.EndX, this.EndY);
            }

            // check if both Start and End position is filled, and check whether user want to generate the
            // maze or not?
            if (this.StartX >= 0 && this.StartY >= 0 &&
                this.EndX   >= 0 && this.EndY   >= 0)
            {
                // ask user
                if (MessageBox.Show("Do you want generate a maze?", "Generate Maze", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.GenerateTheMaze();
                }
            }
        }

        private void GenerateTheMaze()
        {
            this.MzData.GenerateMaze(this.StartX, this.StartY, this.EndX, this.EndY);

            // once finished draw the graphics on the screen
            for (int i = 0; i < this.Y; i++)
            {
                for (int j = 0; j < this.X; j++)
                {
                    Console.WriteLine("(" + j.ToString() + "," + i.ToString() + ") -> ("+ this.MzData.MazeNode[j, i].GetImageIndex().ToString() + ")" +
                        this.MzData.MazeNode[j, i].Up.ToString() + "," + this.MzData.MazeNode[j, i].Right.ToString() + "," +
                        this.MzData.MazeNode[j, i].Down.ToString() + "," + this.MzData.MazeNode[j, i].Left.ToString());
                    this.DrawMaze(this.MzData.MazeNode[j, i].GetImageIndex(), j, i);
                }
            }
        }

        private void DrawMaze(int MazeIndex, int X, int Y)
        {
            if (MazeIndex >= 0 && MazeIndex < this.imgMazeBoard.Images.Count)
            {
                this.CreateGraphics().DrawImage(this.imgMazeBoard.Images[MazeIndex], new Point(X * 30, Y * 30));
            }
            else
            {
                throw new Exception("Image index is outside of the Image List bound");
            }
        }

        private void InitializeMaze()
        {
            int PosX = 0, PosY = 0;

            // loop thru all X and Y and paint on the form
            for (int i = 0; i < Y; i++)
            {
                // initialize the position into 0
                PosX = 0;
                // move to the left
                for (int j = 0; j < X; j++)
                {
                    this.CreateGraphics().DrawImage(this.imgMazeBoard.Images[0], new Point(PosX, PosY));
                    // move to the next position
                    PosX += 30;
                }
                // add 30 to go to the next row for PosY
                PosY += 30;
            }

            // set the FormFinishedPainting into true, so we will not going
            // to re-paint the form again
            this.FormFinishedPainting = true;
        }
    }
}