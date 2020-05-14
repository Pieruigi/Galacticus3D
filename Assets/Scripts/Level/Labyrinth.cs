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
        int tilesBetweenWalls;

        public Labyrinth(LabyrinthConfig config):base(config)
        {
            Debug.Log("Child const...");
            tilesBetweenWalls = config.TilesBetweenWalls;

            Init();
        }

 
        void Init()
        {
            int len = tilesBetweenWalls + 1;
            //if (Width % len > 0 || Height % len > 0)
            //    throw new System.Exception("Width and Height must be multiple of " + len);

            // Compute the number of walls in their original position ( not yet rotated )
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
                    // Set the base wall tile
                    int idx = (tilesBetweenWalls + j*c) + (tilesBetweenWalls + i*c)*Width;
                    SetTileValue(idx, (int)TileValue.Wall);
                    
                    // Give the labyrinth some shape
                    SetWallDirection(idx);
                }
            }
        }

        void SetWallDirection(int tileIndex)
        {
            int r = Random.Range(0, 4);
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
        }

        void DebugRoom()
        {
            string s = "";
            for(int i=0; i<Height; i++)
            {
                for(int j=0; j<Width; j++)
                {
                    s += " " + GetTileValue(i, j);
                }

                s += "\n";
            }

            Debug.Log(s);
        }
    }
}

