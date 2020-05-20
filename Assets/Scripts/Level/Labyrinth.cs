using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Level
{
    public class LabyrinthConfig : RoomConfig
    {
        public int CorridorWidth { get; set; }
    }

    public class Labyrinth : Room
    {
        int corridorWidth;

        float fillRate = 1;
        bool allowUnreachable = false;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }


        public override void Init(RoomConfig config)
        {
            corridorWidth = (config as LabyrinthConfig).CorridorWidth;
            base.Init(config);
        }

        /**
         * Creates internal walls giving the labyrinth some shape.
         * */
        protected override void CreateWalls()
        {
            List<GameObject> wallRes = LoadWallResources();

            // Instanziate walls
            int c = corridorWidth + 1;
            int h = Width / c;
            int v = Height / c;

            // Create a tiles array to keep trace of the walls
            int[] tiles = new int[Width * Height];
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = -1; // Not used yet
            }

            // Check every wall and rotate it
            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    if (Random.Range(0f, 1f) <= fillRate)
                    {
                        // Set the base wall tile
                        int idx = (corridorWidth + j * c) + (corridorWidth + i * c) * Width;
                        
                        // Compute coordinates
                        float x = idx % Width + 0.5f;
                        x *= TileSize;
                        float z = idx / Width + 0.5f;
                        z *= - TileSize;

                        // Create wall object
                        GameObject g = GameObject.Instantiate(wallRes[Random.Range(0, wallRes.Count)]);
                        g.transform.parent = WallRoot.transform;
                        g.transform.localPosition = new Vector3(x, 0, z);
                        //g.transform.localEulerAngles = Vector3.up * 90f * (float)wall.Direction;

                        // Rotate wall
                        RotateWall(idx, g, ref tiles);
                    }

                }
            }

        }


        void RotateWall(int tileIndex, GameObject wallObject, ref int[] tiles)
        {
            int tot = Width * Height;
            List<int> dirs = new List<int>();

            // Avoid overlapping
            // Check north
            if (tiles[tileIndex - Width] == -1)
                dirs.Add(0);
            // Check East
            if (tiles[tileIndex + 1] == -1)
                dirs.Add(1);
            // Check south
            if (tiles[tileIndex + Width] == -1)
                dirs.Add(2);
            // Check west
            if (tiles[tileIndex - 1] == -1)
                dirs.Add(3);

            if (!allowUnreachable)
            {
                // Avoid unreacheable tiles
                int left = tileIndex - corridorWidth - 1;
                int upper = tileIndex - (Width * (corridorWidth + 1));
                int upperLeft = upper - corridorWidth - 1;
                if (left == 0 && upperLeft == 1 && upper == 2)
                    dirs.Remove(3);
                if (left == 1 && upperLeft == 2 && upper == 3)
                    dirs.Remove(0);
            }

            // Rotate
            int r = dirs[Random.Range(0, dirs.Count)];

            switch (r)
            {
                case 0: // North
                    for (int i = 0; i < corridorWidth; i++)
                    {
                        int idx = tileIndex - Width * (i + 1);
                        tiles[idx] = r;// SetTileValue(idx, (int)TileValue.Wall);
                    }
                    break;

                case 1: // East
                    for (int i = 0; i < corridorWidth; i++)
                    {
                        int idx = tileIndex + i + 1;
                        tiles[idx] = r; //SetTileValue(idx, (int)TileValue.Wall);
                    }
                    break;
                case 2: // South
                    for (int i = 0; i < corridorWidth; i++)
                    {
                        int idx = tileIndex + Width * (i + 1);
                        tiles[idx] = r; //SetTileValue(idx, (int)TileValue.Wall);
                    }
                    break;
                case 3: // West
                    for (int i = 0; i < corridorWidth; i++)
                    {
                        int idx = tileIndex - i - 1;
                        tiles[idx] = r; //SetTileValue(idx, (int)TileValue.Wall);
                    }
                    break;
            }


            wallObject.transform.localEulerAngles = Vector3.up * 90f * r;

        }

        List<GameObject> LoadWallResources()
        {
            string wallRes = "Walls/";

            List<GameObject> ret = new List<GameObject>();

            
            string folder = string.Format("{0}x{1}", TileSize, TileSize * (corridorWidth + 1));
            ret = new List<GameObject>(Resources.LoadAll<GameObject>(wallRes + folder));
            
            return ret;
        }

    }

}
