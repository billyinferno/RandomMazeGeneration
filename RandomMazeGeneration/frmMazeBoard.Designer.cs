namespace RandomMazeGeneration
{
    partial class frmMazeBoard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMazeBoard));
            this.imgMazeBoard = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imgMazeBoard
            // 
            this.imgMazeBoard.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgMazeBoard.ImageStream")));
            this.imgMazeBoard.TransparentColor = System.Drawing.Color.Transparent;
            this.imgMazeBoard.Images.SetKeyName(0, "maze_00");
            this.imgMazeBoard.Images.SetKeyName(1, "maze_01");
            this.imgMazeBoard.Images.SetKeyName(2, "maze_02");
            this.imgMazeBoard.Images.SetKeyName(3, "maze_03");
            this.imgMazeBoard.Images.SetKeyName(4, "maze_04");
            this.imgMazeBoard.Images.SetKeyName(5, "maze_05");
            this.imgMazeBoard.Images.SetKeyName(6, "maze_06");
            this.imgMazeBoard.Images.SetKeyName(7, "maze_07");
            this.imgMazeBoard.Images.SetKeyName(8, "maze_08");
            this.imgMazeBoard.Images.SetKeyName(9, "maze_09");
            this.imgMazeBoard.Images.SetKeyName(10, "maze_10");
            this.imgMazeBoard.Images.SetKeyName(11, "maze_11");
            this.imgMazeBoard.Images.SetKeyName(12, "maze_12");
            this.imgMazeBoard.Images.SetKeyName(13, "maze_13");
            this.imgMazeBoard.Images.SetKeyName(14, "maze_14");
            this.imgMazeBoard.Images.SetKeyName(15, "maze_15");
            this.imgMazeBoard.Images.SetKeyName(16, "maze_start");
            this.imgMazeBoard.Images.SetKeyName(17, "maze_end");
            this.imgMazeBoard.Images.SetKeyName(18, "maze_player");
            this.imgMazeBoard.Images.SetKeyName(19, "maze_finished");
            // 
            // frmMazeBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(120, 30);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMazeBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Maze Board";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMazeBoard_FormClosed);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMazeBoard_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmMazeBoard_KeyDown);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.frmMazeBoard_MouseClick);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imgMazeBoard;
    }
}