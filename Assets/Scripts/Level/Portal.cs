using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Level
{
    public class Portal : MonoBehaviour
    {
        Room room;

        //Room nextRoom;

        bool isLocked = false;

        bool inside = false;

        Portal targetPortal;
        

        GameObject player;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            if (!inside)
                return;

            if (isLocked)
                return;

            //if (Input.GetAxis("Action") > 0)
            if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Action"))
            {
                player.transform.position = targetPortal.transform.position;
            }
        }

        public void Init(Room room, Portal targetPortal, bool isLocked)
        {
            this.room = room;
            this.targetPortal = targetPortal;
            this.isLocked = isLocked;
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
