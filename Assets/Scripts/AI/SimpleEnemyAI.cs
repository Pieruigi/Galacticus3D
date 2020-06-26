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
    public class SimpleEnemyAI : MonoBehaviour, IActivable, IFreezable
    {
        [SerializeField]
        GameObject idleMover;

        [SerializeField]
        GameObject seeker;

        [SerializeField]
        GameObject figther;

        [SerializeField]
        float fightingDistance;
        
        IEngageTrigger aiTrigger;

        bool engaged = false;

        TargetSetter targetSetter;
        float sqrFightingDistance;

        bool isFighting = false;
        bool isSeeking = false;
        Vector3 startingPoint;

        bool isDead = false;

        private void Awake()
        {
            sqrFightingDistance = fightingDistance * fightingDistance;
        }

        // Start is called before the first frame update
        void Start()
        {
            startingPoint = transform.position;
            targetSetter = GetComponent<TargetSetter>();
            aiTrigger = GetComponent<IEngageTrigger>();
            aiTrigger.OnTargetEngaged += HandleOnTargetEngaged;
            aiTrigger.OnTargetDisengaged += HandleOnTargetDisengaged;

            GetComponentInParent<IDamageable>().OnDie += HandleOnDie;

            if (idleMover)
                idleMover.GetComponent<IActivable>().Activate();


        }

        // Update is called once per frame
        void Update()
        {
        
            if (!engaged || isDead || !enabled)
                return;

            // If target is inside the fighting distance then we stop pathfinding and start the fighting routines ( movement, aim, shooting, ecc ).
            // We keep figthing until the target is out of range or out of sight.
            if ( (!isFighting && (targetSetter.Target.position - transform.position).sqrMagnitude > sqrFightingDistance) ||
                 (isFighting && (targetSetter.Target.position - transform.position).sqrMagnitude > sqrFightingDistance*1.5f) ||
                 !Utils.AIUtil.IsOnSight(transform, targetSetter.Target))
            {


                if (isFighting)
                    StopFighting();

                if (!isSeeking)
                    StartSeeking();
                               
            }
            else
            {
                if (isSeeking)
                    StopSeeking();

                if (!isFighting)
                    StartFighting();

            }
            
        }


        void HandleOnTargetEngaged()
        {
            Debug.Log(string.Format("Enemy {0} is engaging you.", name));
            engaged = true;
            StopIdle();
        }

        void HandleOnTargetDisengaged()
        {
            Debug.Log(string.Format("Enemy {0} disengaged you.", name));
            engaged = false;
            StopSeeking();
            StopFighting();
            StartIdle();
        }

        void StartFighting()
        {
            isFighting = true;

            if(figther)
                figther.GetComponent<IActivable>().Activate();
            
            Debug.Log("Fight");
         
            
        }

        void StopFighting()
        {
            isFighting = false;

            if(figther)
                figther.GetComponent<IActivable>().Deactivate();

            Debug.Log("StopFighting");

        }

        void StartIdle()
        {
            if(idleMover)
                idleMover.GetComponent<IActivable>().Activate();
        }

        void StopSeeking()
        {
            Debug.Log("Stop seeking");
            isSeeking = false;

            if(seeker)
                seeker.GetComponent<IActivable>().Deactivate();
        }

        void StartSeeking()
        {
            Debug.Log("Start seeking");
            isSeeking = true;
            
            if(seeker)
                seeker.GetComponent<IActivable>().Activate();

        }

        void StopIdle()
        {
            if(idleMover)
                idleMover.GetComponent<IActivable>().Deactivate();
        }

        void HandleOnDie(IDamageable damageable)
        {
            isDead = true;

            StopSeeking();
            StopFighting();
            StopIdle();

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
            }
        }

        public void Activate()
        {
            enabled = true;
        }

        public void Deactivate()
        {
            enabled = false;
            StopFighting();
            StopIdle();
            StopSeeking();
        }

        public bool IsActive()
        {
            return enabled;
        }

        public void Freeze(bool value)
        {
            if (!value)
                Activate();
            else
                Deactivate();
        }
    }

}
