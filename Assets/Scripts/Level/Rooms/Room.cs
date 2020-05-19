using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    
    public class RoomConfig
    {
        public string RoomName { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public float TileSize { get; set; }
    }

    public enum TileValue { Free = 0, Wall, Enemy, Droppable, Unreacheable, Portal }

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
        public float TileSize
        {
            get { return tileSize; }
        }

        string roomName;
        public string RoomName
        {
            get { return roomName; }
            set { roomName = value; }
        }


        List<Portal> portals = new List<Portal>();
        public IList<Portal> Portals
        {
            get { return portals.AsReadOnly(); }
        }

        List<int> portalRootList = new List<int>();


        /**
         * 0 - free
         * 1 - wall
         * */
        int[] tiles;
        public int[] Tiles
        {
            get { return tiles; }
        }


        //protected abstract List<int> GetPortalTileGroup(Portal portal);

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

            int rootTile = GetPortalRootTile(portal);
            portalRootList.Add(rootTile);
            for(int i=0; i<Portal.VerticalTileSize; i++)
            {
                for(int j=0; j<Portal.HorizontalTileSize; j++)
                {
                    int index = rootTile + j + (i * width);
                    SetTileValue(index, (int)TileValue.Portal);
                    Debug.Log("PortalTile:" + index);
                }
            }
            
        }

        public Vector3 GetPortalPosition(Portal portal) 
        {
            int index = portals.IndexOf(portal);
            float h = (portalRootList[index]%width + (Portal.HorizontalTileSize - 1) / 2f ) * TileSize + TileSize/2f;
            float v = - (portalRootList[index]/width + (Portal.VerticalTileSize - 1) / 2f ) * TileSize - TileSize / 2f;
            return new Vector3(h, 0, v);
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

        protected virtual int GetPortalRootTile(Portal portal)
        {
            List<int> all = GetFreeTileGroups(Portal.HorizontalTileSize, Portal.VerticalTileSize);
            Debug.Log("Room:" + RoomName + " - All.Count:" + all.Count);
            int rootTile = all[Random.Range(0, all.Count)];
            return rootTile;

        }
     
        /**
         * Some droppables may occupie more than one tile
         * */
        public List<int> GetFreeTileGroups(int rows, int cols)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < tiles.Length; i++)
                indices.Add(i);
            List<int> all = indices.FindAll(delegate(int t)
            {
               // Debug.Log("Delegate t:" + t);
                if (t + cols >= tiles.Length)
                    return false;
                if (t + rows * width >= tiles.Length)
                    return false;
                

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        int index = t + j + (i * width);
                
                        if (tiles[index] != (int)TileValue.Free)
                            return false;
                    }
                }

                return true;
            });

            return all;
        }

        public override string ToString()
        {

            string pstr = "";
            foreach(Portal p in portals)
            {
                
                pstr += p.TargetRoom.RoomName + " ";
            }
            return string.Format("[{0} - Portals:{1}]", RoomName, pstr);
        }

    }

}
