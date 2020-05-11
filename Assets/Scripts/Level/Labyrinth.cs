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
            tilesBetweenWalls = config.TilesBetweenWalls;
        }
    }
}

