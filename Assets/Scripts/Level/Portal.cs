using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class PortalConfig
    {
        
        public Room TargetRoom { get; set; }

        public bool IsClosed { get; set; }
    }

    public class Portal
    {
        public static readonly int HorizontalTileSize = 2;
        public static readonly int VerticalTileSize = 2;

        Room targetRoom;
        public Room TargetRoom
        {
            get { return targetRoom; }
        }

        bool isClosed = false;
        public Portal(PortalConfig config)
        {
            targetRoom = config.TargetRoom;
            isClosed = config.IsClosed;
        }

    }

}
