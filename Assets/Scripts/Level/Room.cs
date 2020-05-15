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

    public enum TileValue { Free = 0, Wall, Enemy, Droppable }

    public abstract class Room 
    {
        int width, height;
        public int Width
        {
            get { return width; }

        }

        public int Height
        {
            get { return height; }
        }

        float tileSize = 8;

        List<Portal> portals = new List<Portal>();

        /**
         * 0 - free
         * 1 - wall
         * */
        int[] tiles;
        public int[] Tiles
        {
            get { return tiles; }
        }
        
        public Room(RoomConfig config)
        {
            Debug.Log("Parent const...");
            width = config.Width;
            height = config.Height;
            tileSize = config.TileSize;
            tiles = new int[width * height];

        }

        public void AddPortal(Portal portal)
        {
            portals.Add(portal);
        }

        public void SetTileValue(int row, int col, int value)
        {
            int i = row * width + col % width;
            tiles[i] = (int)value;
        }

        public void SetTileValue(int index, int value)
        {
            tiles[index] = value;
        }

        public int GetTileValue(int row, int col)
        {
            int i = row * width + col % width;
            return tiles[i];
        }

        public int GetTileValue(int index)
        {
            return tiles[index];
        }

#if UNITY_EDITOR
        public override string ToString()
        {

            string pstr = "";
            foreach(Portal p in portals)
            {
                
                pstr += p.TargetRoom.RoomName + " ";
            }
            return string.Format("[{0} - Portals:{1}]", RoomName, pstr);
        }


        public string RoomName { get; set; }

#endif
    }

}
