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
                Console.WriteLine("");
                for (int i = 0; i < this.MaxY; i++)
                {
                    for (int j = 0; j < this.MaxX; j++)
                    {
                        // check if this is not yet visited, if yes then start
                        // fill this point onwards.
                        if (this.MazeNode[j, i].HasVisited == false)
                        {
                            this.FillUnvisited(j, i, -1, false);
                        }
                    }
                }
            }
        }

        private bool FillUnvisited(int X, int Y, int PrevMove, bool Connected)
        {
            int Direction;

            bool UpCheck = false,
                 DownCheck = false,
                 LeftCheck = false,
                 RightCheck = false;

            int TriesAnotherDirection = 0;

            // set the node to already visited
            this.MazeNode[X, Y].HasVisited = true;

            // if not connected look for nearest node, if nearest node is already visited
            // connect this node with that node
            while (!(Connected))
            {
                // random check whether nearest node is available or not?
                Direction = (1 << (this.RndSeed.Next(1, 16) % 4));

                while (TriesAnotherDirection < 3 && Direction == PrevMove)
                {
                    Direction = (1 << (this.RndSeed.Next(1, 16) % 4));
                    TriesAnotherDirection += 1;
                }

                // check whether current direction canceled previous direction or not?
                if (PrevMove == 1 && Direction == 4) DownCheck = true;
                if (PrevMove == 2 && Direction == 8) LeftCheck = true;
                if (PrevMove == 4 && Direction == 1) UpCheck = true;
                if (PrevMove == 8 && Direction == 2) RightCheck = true;

                // check the move
                switch (Direction)
                {
                    case 1:
                        if (!UpCheck)
                        {
                            UpCheck = true;
                            // check if we got nearest neighboor on UP position
                            if ((Y - 1) >= 0)
                            {
                                // break the wall between these two
                                this.MazeNode[X, (Y - 1)].Down = true;
                                this.MazeNode[X, Y].Up = true;

                                // check whether it already been visited before or not?
                                if (this.MazeNode[X, (Y - 1)].HasVisited || this.MazeNode[X, (Y - 1)].MainNode)
                                {
                                    Connected = true;
                                }
                                else
                                {
                                    // the node is not yet being visited.
                                    Connected = this.FillUnvisited(X, (Y - 1), Direction, Connected);
                                }
                            }
                        }
                        break;
                    case 2:
                        if (!RightCheck)
                        {
                            RightCheck = true;
                            // check if we got nearest neighboor on RIGHT position
                            if ((X + 1) < this.MaxX)
                            {
                                // break the wall between these two
                                this.MazeNode[(X + 1), Y].Left = true;
                                this.MazeNode[X, Y].Right = true;

                                // check whether it already been visited before or not?
                                if (this.MazeNode[(X + 1), Y].HasVisited || this.MazeNode[(X + 1), Y].MainNode)
                                {
                                    Connected = true;
                                }
                                else
                                {
                                    // the node is not yet being visited.
                                    Connected = this.FillUnvisited((X + 1), Y, Direction, Connected);
                                }
                            }
                        }
                        break;
                    case 4:
                        if (!DownCheck)
                        {
                            DownCheck = true;
                            // check if we got nearest neighboor on DOWN position
                            if ((Y + 1) < this.MaxY)
                            {
                                // break the wall between these two
                                this.MazeNode[X, (Y + 1)].Up = true;
                                this.MazeNode[X, Y].Down = true;

                                // check whether it already been visited before or not?
                                if (this.MazeNode[X, (Y + 1)].HasVisited || this.MazeNode[X, (Y + 1)].MainNode)
                                {
                                    Connected = true;
                                }
                                else
                                {
                                    // the node is not yet being visited.
                                    Connected = this.FillUnvisited(X, (Y + 1), Direction, Connected);
                                }
                            }
                        }
                        break;
                    case 8:
                        if (!LeftCheck)
                        {
                            LeftCheck = true;
                            // check if we got nearest neighboor on DOWN position
                            if ((X - 1) >= 0)
                            {
                                // break the wall between these two
                                this.MazeNode[(X - 1), Y].Right = true;
                                this.MazeNode[X, Y].Left = true;

                                // check whether it already been visited before or not?
                                if (this.MazeNode[(X - 1), Y].HasVisited || this.MazeNode[(X - 1), Y].MainNode)
                                {
                                    Connected = true;
                                }
                                else
                                {
                                    Connected = this.FillUnvisited((X - 1), Y, Direction, Connected);
                                }
                            }
                        }
                        break;
                }
            }

            // default to return false
            return Connected;
        }

        private bool RandomVisit(int X, int Y, int PrevMove)
        {
            Console.Write("(" + X.ToString() + "," + Y.ToString() + ")");
            // check if we visited all the node
            bool AllVisited = false;
            int TriesSameAsPrev = 0;
            bool CheckUp = false, CheckRight = false, CheckDown = false, CheckLeft = false;
            int dir;

            // we already visited this node, mark this node as visited
            this.MazeNode[X, Y].HasVisited = true;
            this.MazeNode[X, Y].MainNode = true;

            // check the previous move and set the wall as open
            switch (PrevMove)
            {
                // Previous move is UP
                case 1:
                    this.MazeNode[X, Y].Down = true;
                    CheckDown = true;
                    break;
                // Previous move is RIGHT
                case 2:
                    this.MazeNode[X, Y].Left = true;
                    CheckLeft = true;
                    break;
                // Previous move is DOWN
                case 4:
                    this.MazeNode[X, Y].Up = true;
                    CheckUp = true;
                    break;
                // Previous move is LEFT
                case 8:
                    this.MazeNode[X, Y].Right = true;
                    CheckRight = true;
                    break;
                default:
                    // nothing to do, probably this is the first node
                    break;
            }

            // bound the check to knew which direction that we actually can visit
            if ((X - 1) < 0)
            {
                // cannot move to left
                CheckLeft = true;
            }
            if ((X + 1) >= this.MaxX)
            {
                // cannot move to right
                CheckRight = true;
            }
            if ((Y - 1) < 0)
            {
                // cannot move to up
                CheckUp = true;
            }
            if ((Y + 1) >= this.MaxY)
            {
                // cannot move to down
                CheckDown = true;
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
                    if (CheckUp && CheckRight && CheckDown && CheckLeft)
                    {
                        AllVisited = true;
                    }
                    else
                    {
                        // not all direction is visited.
                        // randomize which neighboor we should visit?
                        dir = (1 << (this.RndSeed.Next(1, 128) % 4));

                        // check whether current direction same as previous direction?
                        // if so, then check the SameAsPrev variable, whether it's already set as false
                        // or not?
                        // if it's still set as true then skip this direction
                        if (dir == PrevMove)
                        {
                            TriesSameAsPrev = 0;
                            while (TriesSameAsPrev < 3 && dir == PrevMove)
                            {
                                dir = (1 << (this.RndSeed.Next(1, 128) % 4));
                                TriesSameAsPrev += 1;
                            }
                            
                            if (dir == PrevMove) Console.Write("#");
                        }

                        // check the direction we will going
                        switch (dir)
                        {
                            // Going UP
                            case 1:
                                if (!CheckUp)
                                {
                                    CheckUp = true;

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
                                                Console.Write("^");
                                                // Set current direction as true
                                                this.MazeNode[X, Y].Up = true;

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
                                if (!CheckRight)
                                {
                                    CheckRight = true;

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
                                                Console.Write(">");
                                                // Set current direction as true
                                                this.MazeNode[X, Y].Right = true;

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
                                if (!CheckDown)
                                {
                                    CheckDown = true;

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
                                                Console.Write("V");
                                                // Set current direction as true
                                                this.MazeNode[X, Y].Down = true;

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
                                if (!CheckLeft)
                                {
                                    CheckLeft = true;

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
                                                Console.Write("<");
                                                // Set current direction as true
                                                this.MazeNode[X, Y].Left = true;

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
