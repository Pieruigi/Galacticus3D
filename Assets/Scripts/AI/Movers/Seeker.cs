using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;
using UnityEngine.AI;
using System;

namespace OMTB.AI
{
    /**
     * Simply moves towards the target and then starts fighting.
     * */
    [RequireComponent(typeof(TargetSetter))]
    public class Seeker : MonoBehaviour, IActivable, IRolleable
    {
        [SerializeField]
        float repathTime = 0.5f;

        public float StoppingDistance
        {
            get { return GetStoppingDistance(); }
        }

        NavMeshAgent agent;
        DateTime lastRepath;

        TargetSetter targetSetter;

        bool isActive = false;
        
        // Start is called before the first frame update
        void Awake()
        {
            targetSetter = GetComponent<TargetSetter>();
            agent = GetComponentInParent<NavMeshAgent>();
            Deactivate();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isActive)
                return;

  
            if((DateTime.UtcNow - lastRepath).TotalSeconds > repathTime)
            {
                lastRepath = DateTime.UtcNow;
                agent.SetDestination(targetSetter.Target.position);
            }

        }

        

        public void Activate()
        {
            Debug.Log("Activate seeker");
            //agent.isStopped = false;
            isActive = true;
            agent.isStopped = false;
        }

        public void Deactivate()
        {
            //agent.isStopped = true;
            isActive = false;
            
        }

        float GetStoppingDistance()
        {
            if (!agent)
                agent = GetComponent<NavMeshAgent>();

            return agent.stoppingDistance;
        }

        public float GetMaxAngularSpeed()
        {
            if (agent == null)
                agent = GetComponent<NavMeshAgent>();
            return agent.angularSpeed;
        }

        public float GetMaxSideSpeed()
        {
            return 0;
        }

        public bool IsAiming()
        {
            return false;
        }

        public bool IsActive()
        {
            return !agent.isStopped;
        }
    }

}
