using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public class ShipLauncher : MonoBehaviour
    {
        Vector3 targetPosition;
        public Vector3 TargetPosition
        {
            get { return targetPosition; }
            set { targetPosition = value; }
        }

        float timer = 1f;
        float speed = 20;
        float upSpeed = 30;
        float speedDecelleration = 3;

        
        // Start is called before the first frame update
        void Start()
        {
         
            DeactivateAll();
          
            
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.y == 0)
            {
                ActivateAll();
                //transform.position = targetPosition;
                Destroy(this);
                return;
            }

            timer -= Time.deltaTime;

            if(speed > 0)
            {
                transform.position = transform.position + transform.forward * speed * Time.deltaTime;

                speed -= speedDecelleration * Time.deltaTime;
                if (speed < 0)
                    speed = 0;
            }
            

            if (timer < 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), upSpeed * Time.deltaTime);
            }


        }

        void ActivateAll()
        {

            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent)
                agent.enabled = true;

            IActivable[] activables = GetComponentsInChildren<IActivable>();
            foreach (IActivable activable in activables)
                activable.Activate();

            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach(Collider c in colliders)
            {
                c.enabled = true;
            }

            Roller r = GetComponentInChildren<Roller>();
            if (r)
                r.enabled = true;


            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb)
                rb.isKinematic = false;

        }

        void DeactivateAll()
        {
          

            IActivable[] activables = GetComponentsInChildren<IActivable>();
            foreach (IActivable activable in activables)
                activable.Deactivate();

            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (Collider c in colliders)
            {
                c.enabled = false;
            }

            Roller r = GetComponentInChildren<Roller>();
            if (r)
                r.enabled = false;

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb)
                rb.isKinematic = true;

            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent)
                agent.enabled = false;
        }
    }

}
