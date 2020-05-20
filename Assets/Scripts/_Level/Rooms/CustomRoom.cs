using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class CustomRoomConfig: RoomConfig
    {

    }

    public class CustomRoom : Room
    {
        public CustomRoom(CustomRoomConfig config) : base(config)
        {

        }


    }

}
