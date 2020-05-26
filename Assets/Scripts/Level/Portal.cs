using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OMTB.Level
{
    public class Portal : MonoBehaviour
    {
        public UnityAction<Portal> OnTeleport;

        Room room;
        public Room Room
        {
            get { return room; }
        }

        //Room nextRoom;

        bool isLocked = false;

        bool inside = false;

        Portal targetPortal;
        public Portal TargetPortal
        {
            get { return targetPortal; }
        }

        bool isTeleporting = false;

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

            if (isTeleporting)
                return;

            //if (Input.GetAxis("Action") > 0)
            if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Action"))
            {
                StartCoroutine(TeleportPlayer());
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

        IEnumerator TeleportPlayer()
        {
            isTeleporting = true;
            yield return new WaitForSeconds(0.25f); // Do some fade ???

            player.transform.position = targetPortal.transform.position;

            // Disable enemies belonging to the current room
            isTeleporting = false;
            OnTeleport?.Invoke(this);

            

        }
    }

}
