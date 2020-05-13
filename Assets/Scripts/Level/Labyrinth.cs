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
            if (Width % len > 0 || Height % len > 0)
                throw new System.Exception("Width and Height must be multiple of " + len);


        }
    }
}

