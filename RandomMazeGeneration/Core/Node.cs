using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RandomMazeGeneration.Core
{
    class Node
    {
        public bool HasVisited { get; set; }
        public bool MainNode { get; set; }

        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }

        public Node()
        {
            this.HasVisited = false;
            this.MainNode = false;
            this.Up = false;
            this.Down = false;
            this.Left = false;
            this.Right = false;
        }

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
            //  0 1 0 1 --> which is will be translate into 10, means that we will load ImageIndex 5, which should have gate opened on UP and DOWN

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
