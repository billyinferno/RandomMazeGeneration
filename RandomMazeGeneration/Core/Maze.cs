using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomMazeGeneration.Core
{
    class Maze
    {
        public Node[,] MazeNode { get; private set; }

        public int MaxX { get; private set; }
        public int MaxY { get; set; }

        public int StartX { get; private set; }
        public int StartY { get; private set; }
        public int FinishX { get; private set; }
        public int FinishY { get; private set; }

        private Random RndSeed;

        public Maze(int X, int Y)
        {
            int i, j;

            // create the Maze Node based on the X and Y sent to the constructor
            this.MazeNode = new Node[X,Y];

            // initialize all the Node data
            for (i = 0; i < X; i++)
            {
                for (j = 0; j < Y; j++)
                {
                    this.MazeNode[i, j] = new Node();
                }
            }

            // put the X and Y into MaxX and MaxY variable so we can used it
            // as validation variable later on.
            this.MaxX = X;
            this.MaxY = Y;

            // set the RandomSeed
            this.RndSeed = new Random();
        }

        public void GenerateMaze(int StartX, int StartY, int FinishX, int FinishY)
        {
            bool IsFinished = false;

            this.StartX = StartX;
            this.StartY = StartY;

            this.FinishX = FinishX;
            this.FinishY = FinishY;

            // loop until we reached the FinishX and FinishY
            while (!(IsFinished))
            {
                // traverse thru the maze and generate the path
                IsFinished = this.RandomVisit(StartX, StartY, -1);
            }
        }

        protected bool RandomVisit(int X, int Y, int PrevMove)
        {
            // check if we visited all the node
            bool AllVisited = false;
            int dir;

            // we already visited this node, mark this node as visited
            this.MazeNode[X,Y].HasVisited = true;

            // check the previous move and set the wall as open
            switch (PrevMove)
            {
                // Previous move is UP
                case 1:
                    this.MazeNode[X, Y].Down = true;
                    break;
                // Previous move is RIGHT
                case 2:
                    this.MazeNode[X, Y].Left = true;
                    break;
                // Previous move is DOWN
                case 4:
                    this.MazeNode[X, Y].Up = true;
                    break;
                // Previous move is LEFT
                case 8:
                    this.MazeNode[X, Y].Right = true;
                    break;
                default:
                    // nothing to do, probably this is the first node
                    break;
            }

            // loop if we not visit all 
            while (!(AllVisited))
            {
                // we will only going to be finished if we already
                // reached the Finished X and Y
                if (X == this.FinishX &&
                    Y == this.FinishY)
                {
                    return true;
                }
                else
                {
                    // check if we already visit all direction?
                    if (this.MazeNode[X,Y].Up && this.MazeNode[X, Y].Right && this.MazeNode[X, Y].Down && this.MazeNode[X, Y].Left)
                    {
                        AllVisited = true;
                    }
                    else
                    {
                        // not all direction is visited.
                        // randomize which neighboor we should visit?
                        dir = (1 << (this.RndSeed.Next(1, 16) % 4));

                        // check the direction we will going
                        switch (dir)
                        {
                            // Going UP
                            case 1:
                                if (!this.MazeNode[X, Y].Up)
                                {
                                    // Set current direction as true
                                    this.MazeNode[X, Y].Up = true;

                                    // check previous move and ensure that this move not canceled
                                    // the previous move. If so the just set the move as true, no need to
                                    // perform any move here
                                    if (!(PrevMove == 4))
                                    {
                                        // move is legal, now check whether we can visit this neighboor or not?
                                        if ((Y - 1) >= 0)
                                        {
                                            // we can visit this node, check whether this node is already visited
                                            // or not?
                                            if (!(this.MazeNode[X, Y - 1].HasVisited))
                                            {
                                                // not yet being visited, try to visit the node
                                                if (RandomVisit(X, Y - 1, dir))
                                                {
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            // Going RIGHT
                            case 2:
                                if (!this.MazeNode[X, Y].Right)
                                {
                                    // Set current direction as true
                                    this.MazeNode[X, Y].Right = true;

                                    // check previous move and ensure that this move not canceled
                                    // the previous move. If so the just set the move as true, no need to
                                    // perform any move here
                                    if (!(PrevMove == 8))
                                    {
                                        // move is legal, now check whether we can visit this neighboor or not?
                                        if ((X + 1) < this.MaxX)
                                        {
                                            // we can visit this node, check whether this node is already visited
                                            // or not?
                                            if (!(this.MazeNode[X + 1, Y].HasVisited))
                                            {
                                                // not yet being visited, try to visit the node
                                                if (RandomVisit(X + 1, Y, dir))
                                                {
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            // Going DOWN
                            case 4:
                                if (!this.MazeNode[X, Y].Down)
                                {
                                    // Set current direction as true
                                    this.MazeNode[X, Y].Down = true;

                                    // check previous move and ensure that this move not canceled
                                    // the previous move. If so the just set the move as true, no need to
                                    // perform any move here
                                    if (!(PrevMove == 1))
                                    {
                                        // move is legal, now check whether we can visit this neighboor or not?
                                        if ((Y + 1) < this.MaxY)
                                        {
                                            // we can visit this node, check whether this node is already visited
                                            // or not?
                                            if (!(this.MazeNode[X, Y + 1].HasVisited))
                                            {
                                                // not yet being visited, try to visit the node
                                                if (RandomVisit(X, Y + 1, dir))
                                                {
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                            // Going LEFT
                            case 8:
                                if (!this.MazeNode[X, Y].Left)
                                {
                                    // Set current direction as true
                                    this.MazeNode[X, Y].Left = true;

                                    // check previous move and ensure that this move not canceled
                                    // the previous move. If so the just set the move as true, no need to
                                    // perform any move here
                                    if (!(PrevMove == 1))
                                    {
                                        // move is legal, now check whether we can visit this neighboor or not?
                                        if ((X - 1) >= 0)
                                        {
                                            // we can visit this node, check whether this node is already visited
                                            // or not?
                                            if (!(this.MazeNode[X - 1, Y].HasVisited))
                                            {
                                                // not yet being visited, try to visit the node
                                                if (RandomVisit(X - 1, Y, dir))
                                                {
                                                    return true;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
            }

            // return default to false
            return false;
        }
    }
}
