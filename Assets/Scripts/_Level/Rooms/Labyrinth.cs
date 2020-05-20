using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class LabyrinthConfig: RoomConfig
    {
        public int TilesBetweenWalls { get; set; }
    }

    public class Labyrinth : Room
    {
        public class Wall
        {
            int rootIndex;
            public int RootIndex
            {
                get { return rootIndex; }
            }
            /**
             *  0: north
             *  1: east
             *  2: south
             *  3: west
             *  */
            int direction; 
            public int Direction
            {
                get { return direction; }
            }

            public Wall(int rootIndex, int direction)
            {
                this.rootIndex = rootIndex;
                this.direction = direction;
            }
        }

        int tilesBetweenWalls;
        public int CorridorWidthInTiles
        {
            get { return tilesBetweenWalls; }
        }

        float fillRate = 1;

        bool allowUnreacheableTiles = false;

        List<Wall> walls = new List<Wall>();
        public IList<Wall> Walls
        {
            get { return walls.AsReadOnly(); }
        }

        public Labyrinth(LabyrinthConfig config):base(config)
        {
            Debug.Log("Child const...");
            tilesBetweenWalls = config.TilesBetweenWalls;

            Init();
        }


        void Init()
        {
            int len = tilesBetweenWalls + 1;
        
            // Create walls and give the labyrinth some shape
            Debug.Log("Setting walls for room - width:" + Width + ", height:" + Height + ", dist:" + tilesBetweenWalls);
            InitWalls();

            DebugRoom();
        }

        void InitWalls()
        {
            int c = tilesBetweenWalls + 1;
            int h = Width / c;
            int v = Height / c;

            

            // Check every wall and rotate it
            for(int i=0; i<v; i++)
            {
                for(int j=0; j<h; j++)
                {
                    if(Random.Range(0f,1f) <= fillRate)
                    {
                        // Set the base wall tile
                        int idx = (tilesBetweenWalls + j * c) + (tilesBetweenWalls + i * c) * Width;
                        SetTileValue(idx, (int)TileValue.Wall);

                        // Give the labyrinth some shape
                        SetWallDirection(idx);
                    }
                    
                }
            }


            // Set unreacheable tiles
            if (allowUnreacheableTiles)
            {
                List<int> freeList = new List<int>();
                List<int> checkList = new List<int>();
                float size = Width * Height;
                for (int i = 0; i < size; i++)
                    if (!CheckTileIsRecheable(i, ref freeList, ref checkList))
                        SetTileValue(i, (int)TileValue.Unreacheable);
            }
            

            
        }

        void SetWallDirection(int tileIndex)
        {
            int tot = Width * Height;
            List<int> dirs = new List<int>();

            // Avoid overlapping
            // Check north
            if (GetTileValue(tileIndex - Width) == (int)TileValue.Free)
                dirs.Add(0);
            // Check East
            if (GetTileValue(tileIndex + 1) == (int)TileValue.Free)
                dirs.Add(1);
            // Check south
            if (GetTileValue(tileIndex + Width) == (int)TileValue.Free)
                dirs.Add(2);
            // Check west
            if (GetTileValue(tileIndex - 1) == (int)TileValue.Free)
                dirs.Add(3);

            if (!allowUnreacheableTiles)
            {
                // Avoid unreacheable tiles
                int left = tileIndex - tilesBetweenWalls - 1;
                int upper = tileIndex - (Width * (tilesBetweenWalls + 1));
                int upperLeft = upper - tilesBetweenWalls - 1;
                if (left == 0 && upperLeft == 1 && upper == 2)
                    dirs.Remove(3);
                if (left == 1 && upperLeft == 2 && upper == 3)
                    dirs.Remove(0);
            }
            

            int r = dirs[Random.Range(0, dirs.Count)];

            switch (r)
            {
                case 0: // North
                    for(int i=0; i<tilesBetweenWalls; i++)
                    {
                        int idx = tileIndex - Width * (i + 1);
                        SetTileValue(idx, (int)TileValue.Wall);
                    }
                    break;

                case 1: // East
                    for (int i = 0; i < tilesBetweenWalls; i++)
                    {
                        int idx = tileIndex + i + 1;
                        SetTileValue(idx, (int)TileValue.Wall);
                    }
                    break;
                case 2: // South
                    for (int i = 0; i < tilesBetweenWalls; i++)
                    {
                        int idx = tileIndex + Width * (i + 1);
                        SetTileValue(idx, (int)TileValue.Wall);
                    }
                    break;
                case 3: // West
                    for (int i = 0; i < tilesBetweenWalls; i++)
                    {
                        int idx = tileIndex - i - 1;
                        SetTileValue(idx, (int)TileValue.Wall);
                    }
                    break;
            }

            walls.Add(new Wall(tileIndex, r));
        }


        bool CheckTileIsRecheable(int tileId, ref List<int> freeList, ref List<int> checkList)
        {
            // It's a wall tile, so it's not reacheable 
            if (GetTileValue(tileId) == (int)TileValue.Wall)
                return false;

            // It's a reacheable tile
            if (freeList.Contains(tileId))
                return true;

            // All the tiles along the border are reacheable for sure
            if (tileId / Width == 0 || tileId / Width == Height - 1 || tileId % Width == 0 || tileId % Width == Width - 1)
            {
                freeList.Add(tileId);
                return true;
            }

            // We don't know if it's reacheable yet
            checkList.Add(tileId);

            bool ret = false;

            int i = 0;

            while (i < 4 && !ret)
            {
                int adjacentId = -1;
                if (i == 0) // Left tile
                    adjacentId = tileId - 1;
                else if (i == 1) // Up
                    adjacentId = tileId - Width;
                else if (i == 2) // Right
                    adjacentId = tileId + 1;
                else if (i == 3)
                    adjacentId = tileId + Width;

                if (!checkList.Contains(adjacentId))
                {
                    // If some adjacent tile is reacheable then the current one is recheable too
                    ret = CheckTileIsRecheable(adjacentId, ref freeList, ref checkList);

                    if (ret)
                        freeList.Add(tileId);
                    
                }



                i++;
            }

            checkList.Remove(tileId);

            return ret;

        }


    

        void DebugRoom()
        {
            string s = "";
            for(int i=0; i<Height; i++)
            {
                for(int j=0; j<Width; j++)
                {
                    s += " " + (GetTileValue(i, j) == (int)TileValue.Free ? "0" : "3");
                }

                s += "\n";
            }

            Debug.Log(s);
        }
    }
}

