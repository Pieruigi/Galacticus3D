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



        public override void Init(RoomConfig config)
        {
            base.Init(config);
            corridorWidth = (config as LabyrinthConfig).CorridorWidth;
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
            int[] rots = new int[Width * Height];
            for (int i = 0; i < rots.Length; i++)
            {
                rots[i] = -1; // Not used yet
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
                       

                        // Rotate wall
                        RotateWall(idx, g, ref rots);
                    }

                }
            }

            string s = "";
            for(int i=0; i<Height; i++)
            {
                for(int j=0; j<Width; j++)
                {
                    int idx = i * Width + j ;
                    s += rots[idx] + " ";
                }
                s += "\n";
            }
            print(s);
        }


        void RotateWall(int tileIndex, GameObject wallObject, ref int[] rots)
        {
            int tot = Width * Height;
            List<int> dirs = new List<int>();

            // Avoid overlapping
            // Check north
            if (rots[tileIndex - Width] == -1)
                dirs.Add(0);
            // Check East
            if (rots[tileIndex + 1] == -1)
                dirs.Add(1);
            // Check south
            if (rots[tileIndex + Width] == -1)
                dirs.Add(2);
            // Check west
            if (rots[tileIndex - 1] == -1)
                dirs.Add(3);

            if (!allowUnreachable)
            {
                // Avoid unreacheable tiles
                //int left = tileIndex - corridorWidth - 1;
                //int upper = tileIndex - (Width * (corridorWidth + 1));
                //int upperLeft = upper - corridorWidth - 1;
                int left = tileIndex - corridorWidth - 1;
                int upper = tileIndex - (Width * (corridorWidth + 1));
                int upperLeft = upper - corridorWidth - 1;
                print(string.Format("l,u,ul:{0},{1},{2}", left, upper, upperLeft));
                if (left >= 0 && upper >= 0 && upperLeft >= 0 && rots[left] == 0 && rots[upperLeft] == 1 && rots[upper] == 2)
                    dirs.Remove(3);
                if (left >= 0 && upper >= 0 && upperLeft >= 0 && rots[left] == 1 && rots[upperLeft] == 2 && rots[upper] == 3)
                    dirs.Remove(0);
            }

            // Rotate
            int r = dirs[Random.Range(0, dirs.Count)];
            rots[tileIndex] = r;
            SetTile(tileIndex); // Tile is no longer free

            for (int i = 0; i < corridorWidth; i++)
            {
                int idx = -1;

                switch (r)
                {
                    case 0: // North
                        idx = tileIndex - Width * (i + 1);
                        break;
                    case 1: // East
                        idx = tileIndex + i + 1;
                        break;
                    case 2: // South
                        idx = tileIndex + Width * (i + 1);
                        break;
                    case 3: // West
                        idx = tileIndex - i - 1;
                        break;
                }

                rots[idx] = r;
                SetTile(idx); // Tile is no longer free
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
