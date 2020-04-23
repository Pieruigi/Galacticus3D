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
    public class SimpleMover : MonoBehaviour
    {
        [SerializeField]
        float repathTime = 0.5f;

        [SerializeField]
        float minStoppingDistance;

        [SerializeField]
        float maxStoppingDistance;

        IEngageTrigger aiTrigger;

        NavMeshAgent agent;

        bool engaged = false;

        DateTime lastRepath;

        TargetSetter targetSetter;
        Transform target;
        float sqrMaxStoppingDistance;

        private void Awake()
        {
            sqrMaxStoppingDistance = maxStoppingDistance * maxStoppingDistance;
        }

        // Start is called before the first frame update
        void Start()
        {
            targetSetter = GetComponent<TargetSetter>();
            target = targetSetter.Target;
            aiTrigger = GetComponent<IEngageTrigger>();
            aiTrigger.OnTargetEngaged += HandleOnTargetEngaged;
            aiTrigger.OnTargetDisengaged += HandleOnTargetDisengaged;
            agent = GetComponent<NavMeshAgent>();
            UpdateStoppingDistance();
        }

        // Update is called once per frame
        void Update()
        {
            if (!engaged)
                return;

            if((DateTime.UtcNow - lastRepath).TotalSeconds > repathTime)
            {
                lastRepath = DateTime.UtcNow;
                if((target.position - transform.position).sqrMagnitude > sqrMaxStoppingDistance || !Utils.AIUtil.IsOnSight(transform, target))
                {
                    UpdateStoppingDistance(); // Set random between min and max stopping distance
                    agent.SetDestination(target.position);
                }
                    
            }
        }

        void HandleOnTargetEngaged()
        {
            Debug.Log(string.Format("Enemy {0} is engaging you.", name));
            engaged = true;

        }

        void HandleOnTargetDisengaged()
        {
            Debug.Log(string.Format("Enemy {0} disengaged you.", name));
            engaged = false;
        }

        void UpdateStoppingDistance()
        {
            agent.stoppingDistance = UnityEngine.Random.Range(minStoppingDistance, maxStoppingDistance);
        }
    }

}
