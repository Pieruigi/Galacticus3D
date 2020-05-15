using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class LabyrinthAllocator : RoomAllocator
    {

        protected override void Awake()
        {
            base.Awake();


        }


        public override void Allocate()
        {
            
            //// Create wall root and add the optimizer script
            //GameObject wallRoot = new GameObject("Walls");
            //wallRoot.transform.parent = transform;
            //wallRoot.transform.localPosition = Vector3.zero;
            //wallRoot.transform.localRotation = Quaternion.identity;
            //wallRoot.AddComponent<Optimizer>();

            // Get wall resources
            List<GameObject> wallRes = LoadWallResources();
            if (wallRes.Count == 0)
                return;

            Debug.Log("FindWall:" + wallRes[0]);
            // Put walls in scene
            List<Labyrinth.Wall> walls = new List<Labyrinth.Wall>((Room as Labyrinth).Walls);
            foreach(Labyrinth.Wall wall in walls)
            {
                // Coordinates
                int index = wall.RootIndex;
                float x = index % Room.Width + 0.5f;
                x *= Room.TileSize;
                float z = index / Room.Width + 0.5f;
                z *= -Room.TileSize;

                // Create wall object
                GameObject g = GameObject.Instantiate(wallRes[Random.Range(0, wallRes.Count)]);
                g.transform.parent = WallRoot.transform;
                g.transform.localPosition = new Vector3(x, 0, z);
                g.transform.localEulerAngles = Vector3.up * 90f * (float)wall.Direction;
            }
        }

        List<GameObject> LoadWallResources()
        {
            string wallRes = "Walls/";

            List<GameObject> ret = new List<GameObject>();

            if (Room.GetType() == typeof(Labyrinth))
            {
                string folder = string.Format("{0}x{1}", Room.TileSize, Room.TileSize * ((Room as Labyrinth).CorridorWidthInTiles+1));
                ret = new List<GameObject>(Resources.LoadAll<GameObject>(wallRes + folder));
            }

            return ret;
        }
    }

}
