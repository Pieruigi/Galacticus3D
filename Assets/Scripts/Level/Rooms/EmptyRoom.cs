using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Level
{
    public class EmptyRoom : Room
    {
        [SerializeField]
        List<Transform> portalSpawns;

        protected override void Start()
        {
            base.Start();

            Clear();
        }

        protected override void CreateWalls()
        {

        }

        public override Vector3 GetPortalPosition(int widthInTiles, int heightInTiles)
        {
            // No available position, throw an exception
            if(portalSpawns.Count == 0)
                throw new System.Exception("No portal spawn found.");

            // Get the new portal position
            Transform ret = portalSpawns[Random.Range(0, portalSpawns.Count)];

            // Remove object from available list
            portalSpawns.Remove(ret);

            // Get position
            Vector3 retPos = ret.position;

            // Destroy empty object
            //Destroy(ret);

            // Return position
            return retPos;
        }

        void Clear()
        {
            // Remove unused portal spawns
            int count = portalSpawns.Count;
            for(int i=0; i<count; i++)
            {
                Destroy(portalSpawns[0].gameObject);
            }
            portalSpawns.Clear();
        }
    }

}
