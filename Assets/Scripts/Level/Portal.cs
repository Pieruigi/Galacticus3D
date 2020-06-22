using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OMTB.Level
{
    public class Portal : MonoBehaviour
    {
        public UnityAction<Portal> OnTeleport;

        // The room this portal belongs to
        Room room;
        public Room Room
        {
            get { return room; }
        }

        //Room nextRoom;

        bool isLocked = false;

        bool inside = false;

        bool alreadyUsed = false;
        public bool AlreadyUsed
        {
            get { return alreadyUsed; }
            set { alreadyUsed = value; }
        }

        // The target portal 
        Portal targetPortal;
        public Portal TargetPortal
        {
            get { return targetPortal; }
        }

        bool isTeleporting = false;

        GameObject player;

        CameraFadeController cameraFade;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            cameraFade = GameObject.FindObjectOfType<CameraFadeController>();
        }

        void Update()
        {
            if (!inside)
                return;

            if (isLocked)
                return;

            if (isTeleporting)
                return;

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

            //yield return new WaitForSeconds(0.25f); // Do some fade ???
            yield return cameraFade.FadeOutCoroutine();
            
            player.transform.position = targetPortal.transform.position;

            // Set this portal as already used
            alreadyUsed = true;

            // Set the target portal as already used too
            targetPortal.AlreadyUsed = true;

            yield return cameraFade.FadeInCoroutine();

            // Teleport completed
            isTeleporting = false;
            
            OnTeleport?.Invoke(this);

            

        }

    }

}
