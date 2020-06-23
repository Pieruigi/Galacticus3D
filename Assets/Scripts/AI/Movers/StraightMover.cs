using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;
using UnityEngine.AI;

namespace OMTB.AI
{
    public class StraightMover : MonoBehaviour, IActivable
    {
        [SerializeField]
        float speed = 3f;

        [SerializeField]
        float angularSpeed = 60f;
        float angularSpeedRad;

        [SerializeField]
        bool useRigidbody;

        bool isActive = false;

        Rigidbody rb;

        Transform root;

        TargetSetter targetSetter;

        NavMeshAgent agent;

        void Awake()
        {
            angularSpeedRad = Mathf.Deg2Rad * angularSpeed;
            root = transform.root;
            targetSetter = GetComponent<TargetSetter>();

            if (useRigidbody)
                rb = root.GetComponent<Rigidbody>();

            agent = root.GetComponent<NavMeshAgent>();

            Deactivate();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isActive) return;

            // Get direction towards target
            Vector3 dir = targetSetter.Target.position - transform.position;
            dir.y = 0;

            // Rotate towards target
            transform.forward = Vector3.RotateTowards(transform.forward, dir.normalized, angularSpeedRad * Time.deltaTime, 0.0f);

            // Move with no rigidbody
            if (!useRigidbody)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }
        }

        void FixedUpdate()
        {
            if (!isActive) return;

            // Move using rigidbody
            if (useRigidbody)
            {
                rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
            }
        }

        public void Activate()
        {
            if (agent)
                agent.isStopped = true;

            isActive = true;

        }

        public void Deactivate()
        {
            isActive = false;
        }

        public bool IsActive()
        {
            return isActive;
        }
    }

}
