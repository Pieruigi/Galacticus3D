using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using OMTB.Interfaces;
using UnityEngine.Events;
using System;
using OMTB.Utils;

namespace OMTB.AI
{
    [RequireComponent(typeof(TargetSetter))]
    public class AITriggerByRange : MonoBehaviour, IEngageTrigger
    {
        public event TargetEngaged OnTargetEngaged;
        public event TargetDisengaged OnTargetDisengaged;

        [SerializeField]
        float reactionTime = 0;

        [SerializeField]
        float engageRange = 0;

        [SerializeField]
        bool engageOnSightOnly = false;

        [SerializeField]
        float escapeRange = 0;

       
        TargetSetter targetSetter;


        Transform target;
        public Transform Target
        {
            get { return target; }
        }

        bool engaged = false;
        float sqrRange;
        DateTime lastCheck;



        private void Awake()
        {
            sqrRange = engageRange * engageRange;
        }

        // Start is called before the first frame update
        void Start()
        {
            targetSetter = GetComponent<TargetSetter>();
            target = targetSetter.Target;
            targetSetter.OnTargetChanged += delegate (Transform t) { target = t; };
        }

        // Update is called once per frame
        void Update()
        {
            if ((DateTime.UtcNow - lastCheck).TotalSeconds < reactionTime)
                return;

            lastCheck = DateTime.UtcNow;

            if (!engaged)
            {
                
                // Check if the target is inside the engage range
                if((target.position - transform.position).sqrMagnitude < sqrRange)
                {
                    // Check if target needs to be on sight
                    if(!engageOnSightOnly || AIUtil.IsOnSight(transform, target))
                    {
                        engaged = true;
                        sqrRange = escapeRange * escapeRange;
                        OnTargetEngaged?.Invoke();
                    }
                    
                }
            }
            else
            {
                // If target is outside the escape range then disengage
                if((target.position - transform.position).sqrMagnitude > sqrRange)
                {
                    engaged = false;
                    sqrRange = engageRange * engageRange;
                    OnTargetDisengaged?.Invoke();
                }
            }
        }

       
    }
}

