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
    public class SimpleEnemyAI : MonoBehaviour, IRolleable
    {
        [SerializeField]
        GameObject seeker;

        [SerializeField]
        GameObject figther;

        [SerializeField]
        float fightingDistance;
        
        IEngageTrigger aiTrigger;

        bool engaged = false;

        TargetSetter targetSetter;
        Transform target;
        float sqrFightingDistance;

        bool isFighting = false;
        bool isSeeking = false;
        Vector3 startingPoint;

        private void Awake()
        {
            sqrFightingDistance = fightingDistance * fightingDistance;
        }

        // Start is called before the first frame update
        void Start()
        {
            startingPoint = transform.position;
            targetSetter = GetComponent<TargetSetter>();
            target = targetSetter.Target;
            aiTrigger = GetComponent<IEngageTrigger>();
            aiTrigger.OnTargetEngaged += HandleOnTargetEngaged;
            aiTrigger.OnTargetDisengaged += HandleOnTargetDisengaged;
            
        }

        // Update is called once per frame
        void Update()
        {
            

            if (!engaged)
                return;

            // If target is inside the fighting distance then we stop pathfinding and start the fighting routines ( movement, aim, shooting, ecc ).
            // We keep figthing until the target is inside the fighting range plus some additin value.
            if ( (!isFighting && (target.position - transform.position).sqrMagnitude > sqrFightingDistance) ||
                 (isFighting && (target.position - transform.position).sqrMagnitude > sqrFightingDistance*1.5f) ||
                 !Utils.AIUtil.IsOnSight(transform, target))
            {


                if (isFighting)
                    StopFighting();

                if (!isSeeking)
                    StartSeeking();
               
            }
            else
            {
                if (!isFighting)
                    StartFighting();

                if (isSeeking)
                    StopSeeking();
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
            StopSeeking();
            StopFighting();
        }

        //void SetStoppingDistance()
        //{
        //    stoppingDistance = seeker.GetComponent<Seeker>().StoppingDistance;
        //    stoppingDistance *= 1.4f;
        //    sqrStoppingDistance = stoppingDistance * stoppingDistance;
        //}

        void StartFighting()
        {
            isFighting = true;
            figther.GetComponent<IActivable>().Activate();
            
            Debug.Log("Fight");
         
            
        }

        void StopFighting()
        {
            isFighting = false;
            figther.GetComponent<IActivable>().Deactivate();

            Debug.Log("StopFighting");

        }

        void StopSeeking()
        {
            Debug.Log("Stop seeking");
            isSeeking = false;
            seeker.GetComponent<IActivable>().Deactivate();
        }

        void StartSeeking()
        {
            Debug.Log("Start seeking");
            isSeeking = true;
            //SetStoppingDistance();
            seeker.GetComponent<IActivable>().Activate();

        }

        public float GetMaxAngularSpeed()
        {
            return 30;
        }

        public float GetMaxSideSpeed()
        {
            return 10;
        }

        public bool IsAiming()
        {
            return false;
        }
    }

}
