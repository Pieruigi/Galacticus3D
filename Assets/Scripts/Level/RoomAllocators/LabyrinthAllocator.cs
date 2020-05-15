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
            
            // Create wall root and add the optimizer script
            GameObject wallRoot = new GameObject("Walls");
            wallRoot.transform.parent = transform;
            wallRoot.transform.localPosition = Vector3.zero;
            wallRoot.transform.localRotation = Quaternion.identity;
            wallRoot.AddComponent<Optimizer>();

            // Get wall resources
            List<GameObject> wallRes = LoadWallResources();
            if (wallRes.Count == 0)
                return;

            Debug.Log("FindWall:" + wallRes[0]);
        }

        List<GameObject> LoadWallResources()
        {
            string wallRes = "Walls/";

            List<GameObject> ret = new List<GameObject>();

            if (Room.GetType() == typeof(Labyrinth))
            {
                string folder = string.Format("{0}x{1}", Room.TileSize, Room.TileSize * (Room as Labyrinth).CorridorWidthInTiles);
                ret = new List<GameObject>(Resources.LoadAll<GameObject>(wallRes + folder));
            }

            return ret;
        }
    }

}
