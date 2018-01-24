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
    /// <summary>
    /// frmMazeBoard
    /// 
    /// This is the form that will be used to draw the maze board and play the game.
    /// </summary>
    public partial class frmMazeBoard : Form
    {
        private bool FormFinishedPainting;
        private bool MazeGenerated;
        private int X, Y;
        private int StartX, StartY, EndX, EndY;
        private int PlayerX, PlayerY;
        private int TotalMove;
        private Core.Maze MzData;

        /// <summary>
        /// Construct and initialize the maze board size, include calculate the form size
        /// that need to be displayed to the screen.
        /// </summary>
        /// <param name="X">Number of X column(s) of Maze</param>
        /// <param name="Y">Number of Y row(s) of Maze</param>
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

            // set the indicator that we haven't been generated the maze
            this.MazeGenerated = false;
        }

        #region FORM_FUNCTION
        /// <summary>
        /// This function will draw the initial maze after form is ready for Painting.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Paint Event arguments</param>
        private void frmMazeBoard_Paint(object sender, PaintEventArgs e)
        {
            if (!this.FormFinishedPainting)
            {
                this.InitializeMaze();
            }
        }

        /// <summary>
        /// Check the key input by user once the maze is already generated.
        /// Here we will parse and get the movement and check whether the movement is possible
        /// on the game board or not? If yes, then re-draw the graphics for the maze and
        /// reposition the player on the form.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Key event arguments</param>
        private void frmMazeBoard_KeyDown(object sender, KeyEventArgs e)
        {
            // we can only accept the key input once the maze is already generated.
            if (this.MazeGenerated)
            {
                // check which key is pressed
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        // check if we can move up from current player position
                        if (this.MzData.MazeNode[this.PlayerX, this.PlayerY].Up)
                        {
                            // can move up, move the player to the new position
                            // first redraw the current position image
                            this.DrawMaze(this.MzData.MazeNode[this.PlayerX, this.PlayerY].GetImageIndex(), this.PlayerX, this.PlayerY);

                            // move the player to new position
                            this.PlayerY -= 1;

                            // check if we win or not?
                            this.CheckGameFinished();
                        }
                        break;
                    case Keys.Right:
                        // check if we can move right from current player position
                        if (this.MzData.MazeNode[this.PlayerX, this.PlayerY].Right)
                        {
                            // can move up, move the player to the new position
                            // first redraw the current position image
                            this.DrawMaze(this.MzData.MazeNode[this.PlayerX, this.PlayerY].GetImageIndex(), this.PlayerX, this.PlayerY);

                            // move the player to new position
                            this.PlayerX += 1;

                            // check if we win or not?
                            this.CheckGameFinished();
                        }
                        break;
                    case Keys.Down:
                        // check if we can move down from current player position
                        if (this.MzData.MazeNode[this.PlayerX, this.PlayerY].Down)
                        {
                            // can move up, move the player to the new position
                            // first redraw the current position image
                            this.DrawMaze(this.MzData.MazeNode[this.PlayerX, this.PlayerY].GetImageIndex(), this.PlayerX, this.PlayerY);

                            // move the player to new position
                            this.PlayerY += 1;

                            // check if we win or not?
                            this.CheckGameFinished();
                        }
                        break;
                    case Keys.Left:
                        // check if we can move left from current player position
                        if (this.MzData.MazeNode[this.PlayerX, this.PlayerY].Left)
                        {
                            // can move up, move the player to the new position
                            // first redraw the current position image
                            this.DrawMaze(this.MzData.MazeNode[this.PlayerX, this.PlayerY].GetImageIndex(), this.PlayerX, this.PlayerY);

                            // move the player to new position
                            this.PlayerX -= 1;

                            // check if we win or not?
                            this.CheckGameFinished();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Close the form, and set the Maze Data into null.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Form closed event arguments</param>
        private void frmMazeBoard_FormClosed(object sender, FormClosedEventArgs e)
        {
            // remove the Maze Data class
            this.MzData = null;
        }

        /// <summary>
        /// Handle the mouse click on the form to determine where is the location of
        /// the start and end point of the maze, that will be used as the reference
        /// point when generating the maze, and positioning the player for the game.
        /// 
        /// The calculation is performed based on the approximate of the mouse
        /// click location performed by user.
        /// 
        /// Also draw the Start and End position marker when user perform the mouse click.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Mouse event arguments</param>
        private void frmMazeBoard_MouseClick(object sender, MouseEventArgs e)
        {
            // ensure that we can only click if the maze if not yet being generated.
            // otherwise reject the mouse click.
            if (!MazeGenerated)
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
                    this.EndX >= 0 && this.EndY >= 0)
                {
                    // ask user
                    if (MessageBox.Show("Do you want generate a maze?", "Generate Maze", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.GenerateTheMaze();
                    }
                }
            }
        }
        #endregion

        #region GAME_FUNCTION
        /// <summary>
        /// Check whether the game is already finished or not?
        /// If game already finished, ask user whether they want to restart the game or not?
        /// </summary>
        private void CheckGameFinished()
        {
            // draw the player
            this.DrawMaze(18, this.PlayerX, this.PlayerY);

            // add the total move
            this.TotalMove += 1;

            // check if we already finished the game or not?
            if (this.PlayerX == this.EndX && this.PlayerY == this.EndY)
            {
                if (MessageBox.Show("Game finished with " + this.TotalMove.ToString() + " move(s), do you want to restart?", "Game Finished", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // restart the game by redraw the start and end position of the player
                    // draw the player on the start location
                    this.DrawMaze(18, this.StartX, this.StartY);

                    // draw the end location
                    this.DrawMaze(19, this.EndX, this.EndY);

                    // set the player location same as the start location
                    this.PlayerX = this.StartX;
                    this.PlayerY = this.StartY;

                    // reset the total move
                    this.TotalMove = 0;
                }
                else
                {
                    // close this form, since we already finished
                    this.Close();
                }
            }
        }

        /// <summary>
        /// Generate the maze that we already generated using Maze Class, and draw
        /// the maze on the form with the correct image, so we can represent the correct
        /// wall closed and opened for each maze node(s).
        /// </summary>
        private void GenerateTheMaze()
        {
            this.MzData.GenerateMaze(this.StartX, this.StartY, this.EndX, this.EndY);

            // once finished draw the graphics on the screen
            for (int i = 0; i < this.Y; i++)
            {
                for (int j = 0; j < this.X; j++)
                {
                    this.DrawMaze(this.MzData.MazeNode[j, i].GetImageIndex(), j, i);
                }
            }

            // draw the player on the start location
            this.DrawMaze(18, this.StartX, this.StartY);

            // draw the end location
            this.DrawMaze(19, this.EndX, this.EndY);

            // set the player location same as the start location
            this.PlayerX = this.StartX;
            this.PlayerY = this.StartY;

            // reset the total move into 0
            this.TotalMove = 0;

            // once finished generate the maze, set the MazeGenerated indicator
            // into true, so we will reject the next mouse click
            this.MazeGenerated = true;
        }

        /// <summary>
        /// Draw the form with image index based on the Maze Board image list control,
        /// on the position located at X and Y.
        /// </summary>
        /// <param name="MazeIndex">Index of the image (from Maze Board image list)</param>
        /// <param name="X">X location of the image</param>
        /// <param name="Y">Y location of the image</param>
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

        /// <summary>
        /// Initialize and draw the form with the maze image after ready for Painting.
        /// </summary>
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
        #endregion
    }
}