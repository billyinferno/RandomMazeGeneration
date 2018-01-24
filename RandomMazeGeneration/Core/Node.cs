using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RandomMazeGeneration.Core
{
    /// <summary>
    /// Node
    /// 
    /// This will be used to store the Node Data of the Maze.
    /// The node will stored which direction is allowed for player navigation,
    /// and indicator whether the node is already visited or node so we knew
    /// the direction we need to go during generation of the maze.
    /// </summary>
    class Node
    {
        public bool HasVisited { get; set; }
        public bool MainNode { get; set; }

        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }

        /// <summary>
        /// Constructor of the Node Class.
        /// </summary>
        public Node()
        {
            this.HasVisited = false;
            this.MainNode = false;
            this.Up = false;
            this.Down = false;
            this.Left = false;
            this.Right = false;
        }

        /// <summary>
        /// This function will reset all the node value with the initial value.
        /// </summary>
        public void ResetNode()
        {
            // reset the node value to the initial value
            this.HasVisited = false;
            this.MainNode = false;
            this.Up = false;
            this.Down = false;
            this.Left = false;
            this.Right = false;
        }

        /// <summary>
        /// Get the image index of the node based on the direction set on the Node.
        /// </summary>
        /// <returns>Image index for the Node</returns>
        public int GetImageIndex()
        {
            // we can get the image index by set the bit of the Up, Down, Left, and Right
            //      1
            //    +---+
            //  4 |   | 2
            //    +---+
            //      3
            // so for example if UP and DOWN is set as true then the bit should be
            //  4 3 2 1
            // +-+-+-+-+
            //  0 1 0 1 --> which is will be translate into (0101), means that we will load ImageIndex (5), which should have gate opened on UP and DOWN

            int Result = 0;

            // Check for UP
            if (this.Up)
            {
                Result += 1;
            }

            // Check for RIGHT
            if (this.Right)
            {
                Result += 2;
            }

            // Check for DOWN
            if (this.Down)
            {
                Result += 4;
            }

            // Check for LEFT
            if (this.Left)
            {
                Result += 8;
            }

            // return the image index result
            return Result;
        }
    }
}
