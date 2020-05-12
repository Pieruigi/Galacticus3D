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
        Room targetRoom;
        bool isClosed = false;
        public Portal(PortalConfig config)
        {
            targetRoom = config.TargetRoom;
            isClosed = config.IsClosed;
        }

    }

}
