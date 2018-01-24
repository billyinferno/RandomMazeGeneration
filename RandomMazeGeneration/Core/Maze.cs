using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomMazeGeneration.Core
{
    /// <summary>
    /// Maze Class
    /// 
    /// This class will be generate the Maze Board and perform generation of the
    /// maze using Backtracking Stack Algorithm and Heat Map Algorithm to
    /// ensure that all the node is accessible (no closed node).
    /// </summary>
    class Maze
    {
        public Node[,] MazeNode { get; private set; }
        private int[,] HeatMap;

        public int MaxX { get; private set; }
        public int MaxY { get; private set; }

        public int StartX { get; private set; }
        public int StartY { get; private set; }
        public int FinishX { get; private set; }
        public int FinishY { get; private set; }

        private Random RndSeed;

        /// <summary>
        /// Constructor of the Maze Class.
        /// </summary>
        /// <param name="X">X size of the Maze Board</param>
        /// <param name="Y">Y size of the Maze Board</param>
        public Maze(int X, int Y)
        {
            int i, j;

            // create the Maze Node based on the X and Y sent to the constructor
            this.MazeNode = new Node[X,Y];

            // create the heat map for this maze
            this.HeatMap = new int[X, Y];

            // initialize all the Node data
            for (i = 0; i < X; i++)
            {
                for (j = 0; j < Y; j++)
                {
                    // create new node
                    this.MazeNode[i, j] = new Node();

                    // assuming that this is a blocked path
                    this.HeatMap[i, j] = 0;
                }
            }

            // put the X and Y into MaxX and MaxY variable so we can used it
            // as validation variable later on.
            this.MaxX = X;
            this.MaxY = Y;

            // set the RandomSeed
            this.RndSeed = new Random();
        }

        /// <summary>
        /// Override the maximum value of the MaximumX and MaximumY value.
        /// The value of this should be between the length of the MazeNode size.
        /// </summary>
        /// <param name="X">New maximum X size</param>
        /// <param name="Y">New maximum Y size</param>
        public void SetMaximum(int X, int Y)
        {
            if (((X - 1) <= this.MazeNode.GetLength(0)) && ((Y - 1) <= this.MazeNode.GetLength(1)))
            {
                this.MaxX = X;
                this.MaxY = Y;
            }
            else
            {
                throw new Exception("Cannot override the maximumn length, maximum length out of Maze Node Array boundaries.");
            }
        }

        /// <summary>
        /// Generate the Maze for game play.
        /// </summary>
        /// <param name="StartX">X start location</param>
        /// <param name="StartY">Y start location</param>
        /// <param name="FinishX">X end location</param>
        /// <param name="FinishY">Y end location</param>
        public void GenerateMaze(int StartX, int StartY, int FinishX, int FinishY)
        {
            this.StartX = StartX;
            this.StartY = StartY;

            this.FinishX = FinishX;
            this.FinishY = FinishY;

            // generate the main maze
            this.GenerateMainMaze(this.StartX, this.StartY, -1);
#if DEBUG
            Console.WriteLine("");
#endif

            // visit all the unvisited node
            this.GenerateUnvisitedNode();
#if DEBUG
            Console.WriteLine("");
#endif

            // connect all the node, so all node will be accessible
            this.RemoveBlock();
        }

        /// <summary>
        /// This remove the non-accessible block from the generated map, so all the
        /// node is accessible (no closed node).
        /// </summary>
        private void RemoveBlock()
        {
            int Direction;

            // here we will check and ensure that all the path that already
            // generated is accessible.

            // first what we need to do is generate the heat map by traversing through
            // all the maze, and update 1 for all the path that we visited
            this.GenerateHeatMap(this.StartX, this.StartY, -1);

#if DEBUG
            Console.WriteLine("MAZE HEAT MAP");
#endif
            for (int i = 0; i < this.MaxY; i++)
            {
                for (int j = 0; j < this.MaxX; j++)
                {
#if DEBUG
                    Console.Write(this.HeatMap[j, i].ToString());
#endif
                    // check whether this is un-connected node?
                    if (this.HeatMap[j, i] == 0)
                    {
                        // try to connect this node to nearest connected node
                        // we can do this by checking in random direction
                        // to knew which direction is already visited
                        while (this.HeatMap[j, i] == 0)
                        {

                            Direction = (1 << (this.RndSeed.Next() % 4));
                            switch (Direction)
                            {
                                case 1:
                                    // check if we can go up?
                                    if ((i - 1) >= 0)
                                    {
                                        // check if it's connected node?
                                        if (this.HeatMap[j, (i - 1)] == 1)
                                        {
                                            // break the wall between these two
                                            this.MazeNode[j, i].Up = true;
                                            this.MazeNode[j, (i - 1)].Down = true;

                                            // re-trace the heat map from this location
                                            this.GenerateHeatMap(j, i, -1);
                                        }
                                    }
                                    break;
                                case 2:
                                    // check if we can go right?
                                    if ((j + 1) < this.MaxX)
                                    {
                                        // check if it's connected node?
                                        if (this.HeatMap[(j + 1), i] == 1)
                                        {
                                            // break the wall between these two
                                            this.MazeNode[j, i].Right = true;
                                            this.MazeNode[(j + 1), i].Left = true;

                                            // re-trace the heat map from this location
                                            this.GenerateHeatMap(j, i, -1);
                                        }
                                    }
                                    break;
                                case 4:
                                    // check if we can go down?
                                    if ((i + 1) < this.MaxY)
                                    {
                                        // check if it's connected node?
                                        if (this.HeatMap[j, (i + 1)] == 1)
                                        {
                                            // break the wall between these two
                                            this.MazeNode[j, i].Down = true;
                                            this.MazeNode[j, (i + 1)].Up = true;

                                            // re-trace the heat map from this location
                                            this.GenerateHeatMap(j, i, -1);
                                        }
                                    }
                                    break;
                                case 8:
                                    // check if we can go left?
                                    if ((j - 1) >= 0)
                                    {
                                        // check if it's connected node?
                                        if (this.HeatMap[(j - 1), i] == 1)
                                        {
                                            // break the wall between these two
                                            this.MazeNode[j, i].Left = true;
                                            this.MazeNode[(j - 1), i].Right = true;

                                            // re-trace the heat map from this location
                                            this.GenerateHeatMap(j, i, -1);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
#if DEBUG
                Console.WriteLine("");
#endif
            }
        }

        /// <summary>
        /// Traverse the Node data and generate the Heat Mape of the maze to
        /// determine whether there are closed block on the maze or not?
        /// Closed node will be indicated by value "0" on the heat map array.
        /// </summary>
        /// <param name="X">X location</param>
        /// <param name="Y">Y location</param>
        /// <param name="PrevMove">Previous move performed on the GenerateHeatMap function</param>
        /// <returns></returns>
        private bool GenerateHeatMap(int X, int Y, int PrevMove)
        {
            bool CheckUp = false,
                 CheckRight = false,
                 CheckDown = false,
                 CheckLeft = false;

            // check if we already visited this map or not?
            if (this.HeatMap[X, Y] == 1)
            {
                // we already visit this node, no need to perform traverse again
                return false;
            }
            else
            {
                // set the Check indicator based on the previous move, so we will not going
                // to waste the iteration by re-visiting the previous location.
                switch (PrevMove)
                {
                    case 1:
                        // move up, so we don't need to move down
                        CheckDown = true;
                        break;
                    case 2:
                        // move right, so we don't need to move left
                        CheckLeft = true;
                        break;
                    case 4:
                        // move down, so we don't need to move up
                        CheckUp = true;
                        break;
                    case 8:
                        // move left, so we don't need to move right
                        CheckRight = true;
                        break;
                }

                // perform traverse from this node onwards.
                // before doing that set this node heat map as 1
                this.HeatMap[X, Y] = 1;

                // check what direction is available for this node
                // Up?
                if (this.MazeNode[X, Y].Up && !CheckUp)
                {
                    // try to go up
                    this.GenerateHeatMap(X, (Y - 1), 1);
                }

                // Right?
                if (this.MazeNode[X, Y].Right && !CheckRight)
                {
                    // try to go right
                    this.GenerateHeatMap((X + 1), Y, 2);
                }

                // Down?
                if (this.MazeNode[X, Y].Down && !CheckDown)
                {
                    // try to go down
                    this.GenerateHeatMap(X, (Y + 1), 4);
                }

                // Left?
                if (this.MazeNode[X, Y].Left && !CheckLeft)
                {
                    // try to go left
                    this.GenerateHeatMap((X - 1), Y, 8);
                }
            }

            // defaulted to return false
            return false;
        }

        /// <summary>
        /// This function will generated all the node that not yet being visited,
        /// so all the node can be connected later on.
        /// </summary>
        private void GenerateUnvisitedNode()
        {
            // loop for all the node, and traverse all the unvisited node
            for (int i = 0; i < this.MaxY; i++)
            {
                for (int j = 0; j < this.MaxX; j++)
                {
                    // check if this node is unvisited?
                    if (!this.MazeNode[j, i].HasVisited)
                    {
                        // traverse from this node, until we connect to the visited node
                        this.FillUnvisitedNode(j, i, -1, false);
                    }
                }
            }
        }

        /// <summary>
        /// Traverse to generate the Main Maze path.
        /// </summary>
        /// <param name="X">X location</param>
        /// <param name="Y">Y location</param>
        /// <param name="PrevMove">Previous move perform when generate the Main Maze path</param>
        /// <returns></returns>
        private bool GenerateMainMaze(int X, int Y, int PrevMove)
        {
#if DEBUG
            Console.Write("(" + X.ToString() + "," + Y.ToString() + ")");
#endif
            // Check indicator to knew whether we already check all the
            // possible path of the maze or not?
            bool CheckUp    = false,
                 CheckRight = false,
                 CheckDown  = false,
                 CheckLeft  = false;

            int Direction;

            // set that we already visited this node, and this is a main node
            this.MazeNode[X, Y].HasVisited = true;
            this.MazeNode[X, Y].MainNode = true;

            // check if this is the finished location or not?
            if (X == this.FinishX && Y == this.FinishY)
            {
                // return true to the previous stack
                return true;
            }

            // set the Check indicator based on the previous move, so we will not going
            // to waste the iteration by re-visiting the previous location.
            switch (PrevMove)
            {
                case 1:
                    // move up, so we don't need to move down
                    CheckDown = true;
                    break;
                case 2:
                    // move right, so we don't need to move left
                    CheckLeft = true;
                    break;
                case 4:
                    // move down, so we don't need to move up
                    CheckUp = true;
                    break;
                case 8:
                    // move left, so we don't need to move right
                    CheckRight = true;
                    break;
            }

            // after that, we can check the location of the X, Y to knew whether
            // it's possible to go to Up, Right, Down, or Left direction or not?
            if ((Y - 1) < 0)
            {
                // cannot move up
                CheckUp = true;
            }
            if ((X + 1) >= this.MaxX)
            {
                // cannot move right
                CheckRight = true;
            }
            if ((Y + 1) >= this.MaxY)
            {
                // cannot move down
                CheckDown = true;
            }
            if ((X - 1) < 0)
            {
                // cannot move left
                CheckLeft = true;
            }

            // once we finished check all the possibilities then we can loop to the remaining
            // direction that we can go to
            while ((!CheckUp) || (!CheckRight) || (!CheckDown) || (!CheckLeft))
            {
                // randomize the direction, by perform bitwise function
                Direction = (1 << (this.RndSeed.Next() % 4));
                switch (Direction)
                {
                    case 1:
                        // move UP, check whether we can perform UP direction or not?
                        if (!CheckUp)
                        {
                            // set CheckUp into true, since we already try the up direction
                            CheckUp = true;

                            // now check if we already visited the node or not?
                            // if we not yet visit the node, then we can try to visit that node
                            if(!this.MazeNode[X, (Y-1)].HasVisited)
                            {
#if DEBUG
                                Console.Write("^");
#endif
                                this.MazeNode[X, Y].Up = true;
                                this.MazeNode[X, (Y - 1)].Down = true;
                                // node not yet being visited, try to visit the node
                                if (this.GenerateMainMaze(X, (Y - 1), Direction))
                                {
                                    return true;
                                }
                            }
                        }
                        break;
                    case 2:
                        // move RIGHT, check whether we can perform RIGHT direction or not?
                        if (!CheckRight)
                        {
                            // set CheckRight into true, since we already try the up direction
                            CheckRight = true;

                            // now check if we already visited the node or not?
                            // if we not yet visit the node, then we can try to visit that node
                            if (!this.MazeNode[(X + 1), Y].HasVisited)
                            {
#if DEBUG
                                Console.Write(">");
#endif
                                this.MazeNode[X, Y].Right = true;
                                this.MazeNode[(X + 1), Y].Left = true;
                                // node not yet being visited, try to visit the node
                                if (this.GenerateMainMaze((X + 1), Y, Direction))
                                {
                                    return true;
                                }
                            }
                        }
                        break;
                    case 4:
                        // move DOWN, check whether we can perform DOWN direction or not?
                        if (!CheckDown)
                        {
                            // set CheckDown into true, since we already try the up direction
                            CheckDown = true;

                            // now check if we already visited the node or not?
                            // if we not yet visit the node, then we can try to visit that node
                            if (!this.MazeNode[X, (Y + 1)].HasVisited)
                            {
#if DEBUG
                                Console.Write("V");
#endif
                                this.MazeNode[X, Y].Down = true;
                                this.MazeNode[X, (Y + 1)].Up = true;
                                // node not yet being visited, try to visit the node
                                if (this.GenerateMainMaze(X, (Y + 1), Direction))
                                {
                                    return true;
                                }
                            }
                        }
                        break;
                    case 8:
                        // move LEFT, check whether we can perform LEFT direction or not?
                        if (!CheckLeft)
                        {
                            // set CheckLeft into true, since we already try the up direction
                            CheckLeft = true;

                            // now check if we already visited the node or not?
                            // if we not yet visit the node, then we can try to visit that node
                            if (!this.MazeNode[(X - 1), Y].HasVisited)
                            {
#if DEBUG
                                Console.Write("<");
#endif
                                this.MazeNode[X, Y].Left = true;
                                this.MazeNode[(X - 1), Y].Right = true;
                                // node not yet being visited, try to visit the node
                                if (this.GenerateMainMaze((X - 1), Y, Direction))
                                {
                                    return true;
                                }
                            }
                        }
                        break;
                }
            }

            // defaulted to return false
            return false;
        }

        /// <summary>
        /// Traverse and fill all the unvisited node, so all the node can be connected
        /// later on.
        /// </summary>
        /// <param name="X">X location</param>
        /// <param name="Y">Y location</param>
        /// <param name="PrevMove">Previous move performed when filling the unvisited node</param>
        /// <param name="Connected">Indicator to check whether node already connected to another visited node or not?</param>
        /// <returns></returns>
        private bool FillUnvisitedNode(int X, int Y, int PrevMove, bool Connected)
        {
            // Check indicator to knew whether we already check all the
            // possible path of the maze or not?
            bool CheckUp = false,
                 CheckRight = false,
                 CheckDown = false,
                 CheckLeft = false;

            int Direction;

            // set that we already visited this node, and this is a main node
            this.MazeNode[X, Y].HasVisited = true;
            this.MazeNode[X, Y].MainNode = true;

            // set the Check indicator based on the previous move, so we will not going
            // to waste the iteration by re-visiting the previous location.
            switch (PrevMove)
            {
                case 1:
                    // move up, so we don't need to move down
                    CheckDown = true;
                    break;
                case 2:
                    // move right, so we don't need to move left
                    CheckLeft = true;
                    break;
                case 4:
                    // move down, so we don't need to move up
                    CheckUp = true;
                    break;
                case 8:
                    // move left, so we don't need to move right
                    CheckRight = true;
                    break;
            }

            // after that, we can check the location of the X, Y to knew whether
            // it's possible to go to Up, Right, Down, or Left direction or not?
            if ((Y - 1) <= 0)
            {
                // cannot move up
                CheckUp = true;
            }
            if ((X + 1) >= this.MaxX)
            {
                // cannot move right
                CheckRight = true;
            }
            if ((Y + 1) >= this.MaxY)
            {
                // cannot move down
                CheckDown = true;
            }
            if ((X - 1) <= 0)
            {
                // cannot move left
                CheckLeft = true;
            }

            // loop until this one is connected to another node that we already visited
            while (!Connected)
            {
                // randomize the direction, by perform bitwise function
                Direction = (1 << (this.RndSeed.Next() % 4));
                switch (Direction)
                {
                    case 1:
                        // move UP, check whether we can perform UP direction or not?
                        if (!CheckUp)
                        {
                            // set CheckUp into true, since we already try the up direction
                            CheckUp = true;
                            this.MazeNode[X, Y].Up = true;
                            this.MazeNode[X, (Y - 1)].Down = true;

                            // check if the node is already visited or not?
                            if (this.MazeNode[X, (Y - 1)].HasVisited)
                            {
                                // already visited, connect to this node
                                Connected = true;
                            }
                            else
                            {
                                // node is never been visited, let's visit this node first
                                Connected = this.FillUnvisitedNode(X, (Y-1), Direction, Connected);
                            }
                        }
                        break;
                    case 2:
                        // move RIGHT, check whether we can perform RIGHT direction or not?
                        if (!CheckRight)
                        {
                            // set CheckRight into true, since we already try the up direction
                            CheckRight = true;
                            this.MazeNode[X, Y].Right = true;
                            this.MazeNode[(X + 1), Y].Left = true;

                            // check if the node is already visited or not?
                            if (this.MazeNode[(X + 1), Y].HasVisited)
                            {
                                // already visited, connect to this node
                                Connected = true;
                            }
                            else
                            {
                                // node is never been visited, let's visit this node first
                                Connected = this.FillUnvisitedNode((X + 1), Y, Direction, Connected);
                            }
                        }
                        break;
                    case 4:
                        // move DOWN, check whether we can perform DOWN direction or not?
                        if (!CheckDown)
                        {
                            // set CheckDown into true, since we already try the up direction
                            CheckDown = true;
                            this.MazeNode[X, Y].Down = true;
                            this.MazeNode[X, (Y + 1)].Up = true;

                            // check if the node is already visited or not?
                            if (this.MazeNode[X, (Y + 1)].HasVisited)
                            {
                                // already visited, connect to this node
                                Connected = true;
                            }
                            else
                            {
                                // node is never been visited, let's visit this node first
                                Connected = this.FillUnvisitedNode(X, (Y + 1), Direction, Connected);
                            }
                        }
                        break;
                    case 8:
                        // move LEFT, check whether we can perform LEFT direction or not?
                        if (!CheckLeft)
                        {
                            // set CheckLeft into true, since we already try the up direction
                            CheckLeft = true;
                            this.MazeNode[X, Y].Left = true;
                            this.MazeNode[(X - 1), Y].Right = true;

                            // check if the node is already visited or not?
                            if (this.MazeNode[(X - 1), Y].HasVisited)
                            {
                                // already visited, connect to this node
                                Connected = true;
                            }
                            else
                            {
                                // node is never been visited, let's visit this node first
                                Connected = this.FillUnvisitedNode((X - 1), Y, Direction, Connected);
                            }
                        }
                        break;
                }
            }

            // return true to the stack, since we will only going to be exit from
            // the loop if we already connected to the visited node
            return true;
        }
    }
}
