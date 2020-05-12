using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    
    public class RoomConfig
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public float TileSize { get; set; }
    }

    public abstract class Room 
    {
        int width, height;
        float tileSize = 8;

        List<Portal> portals = new List<Portal>();
                

        public Room(RoomConfig config)
        {
            width = config.Width;
            height = config.Height;
            tileSize = config.TileSize;

        }

        public void AddPortal(Portal portal)
        {
            portals.Add(portal);
        }
    }

}
