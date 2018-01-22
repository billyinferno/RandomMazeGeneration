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
        public frmMazeBoard()
        {
            InitializeComponent();
        }

        private void frmMazeBoard_Load(object sender, EventArgs e)
        {
            Core.Maze mz = new Core.Maze(10, 10);
            mz.GenerateMaze(0, 0, 9, 9);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (mz.MazeNode[i, j].HasVisited)
                    {
                        Console.Write("#");
                    }
                    else
                    {
                        Console.Write("-");
                    }
                }
                Console.WriteLine("");
            }
        }
    }
}
