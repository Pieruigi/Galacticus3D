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
    public class Seeker : MonoBehaviour, IActivable
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
        Transform target;

        // Start is called before the first frame update
        void Awake()
        {
            targetSetter = GetComponent<TargetSetter>();
            target = targetSetter.Target;
            agent = GetComponentInParent<NavMeshAgent>();
            Deactivate();
        }

        // Update is called once per frame
        void Update()
        {
            if (agent.isStopped)
                return;

  
            if((DateTime.UtcNow - lastRepath).TotalSeconds > repathTime)
            {
                lastRepath = DateTime.UtcNow;
                agent.SetDestination(target.position);
            }

        }

        

        public void Activate()
        {
            Debug.Log("Activate seeker");   
            agent.isStopped = false;
        }

        public void Deactivate()
        {
            agent.isStopped = true;
            
        }

        float GetStoppingDistance()
        {
            if (!agent)
                agent = GetComponent<NavMeshAgent>();

            return agent.stoppingDistance;
        }
    }

}
