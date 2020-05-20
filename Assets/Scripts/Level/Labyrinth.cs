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
    }

}
