using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Gameplay
{
    public class Stargate : MonoBehaviour
    {
        Portal portal;
        public Portal Portal
        {
            get { return portal; }
            set { portal = value; }
        }

        Room room;
        public Room Room
        {
            get { return room; }
            set { room = value; }
        }

        bool inside = false;
      
        private static List<Stargate> stargates;

        // Start is called before the first frame update
        void Start()
        {
            if(stargates == null)
                stargates = new List<Stargate>(GameObject.FindObjectsOfType<Stargate>());
    
        }

        // Update is called once per frame
        void Update()
        {
            if (!inside)
                return;

            if (portal.IsClosed)
                return;

            //if (Input.GetAxis("Action") > 0)
            if(Input.GetKeyDown(KeyCode.E))
            {
                Portal targetPortal = null;
                
                Room next = portal.TargetRoom;
                Debug.Log("CurrentRoom:" + room.RoomName);
                Debug.Log("NextRoom:" + next.RoomName);
                foreach(Portal p in next.Portals)
                {
                    Debug.Log("Iterate Portal - targetRoom:" + p.TargetRoom);
                    if (p.TargetRoom == room)
                    {
                        targetPortal = p;
                        break;
                    }
                }

                if(targetPortal == null)
                {
                    Debug.LogError("No target portal found.");
                    return;
                }

                Stargate nextS = stargates.Find(s => s.portal == targetPortal);
                GameObject.FindGameObjectWithTag("Player").transform.position = nextS.transform.position;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ("Player".Equals(other.tag))
            {
                inside = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if ("Player".Equals(other.tag))
            {
                inside = false;
            }
        }


        
    }

}
