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
        [Tooltip("The center point the range is computed from. Leave it null if you want this object to be the center point.")]
        Transform engageTriggerPoint; 

        [SerializeField]
        [Tooltip("Zero is infinite so leave zero if you want the enemy to try to engage you whatever the distance is.")]
        float engageRange = 0;

        [SerializeField]
        bool engageOnSightOnly = false;

        [SerializeField]
        [Tooltip("Zero is infinite, just leave zero if you want the enemy to stay in the engaging state until you destroy it... or you die.")]
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
            if((escapeRange > 0 && escapeRange < engageRange) || (engageRange == 0 && escapeRange > 0))
            {
                Debug.LogWarning(string.Format("Misconfiguration error: engageRange:{0}, escapeRange:{1}", engageRange, escapeRange));
                escapeRange = engageRange; 
            }
         
            sqrRange = engageRange * engageRange;
        }

        // Start is called before the first frame update
        void Start()
        {
            if (engageTriggerPoint == null)
                engageTriggerPoint = transform;

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
                if((target.position - engageTriggerPoint.position).sqrMagnitude < sqrRange || sqrRange == 0)
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
                if(sqrRange > 0 && (target.position - engageTriggerPoint.position).sqrMagnitude > sqrRange)
                {
                    engaged = false;
                    sqrRange = engageRange * engageRange;
                    OnTargetDisengaged?.Invoke();
                }
            }
        }

       

    }
}

