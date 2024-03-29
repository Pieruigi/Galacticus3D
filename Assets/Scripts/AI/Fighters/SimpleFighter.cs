﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.AI
{
    [RequireComponent(typeof(TargetSetter))]
    public class SimpleFighter : MonoBehaviour, IActivable, IFreezable
    {
        //[SerializeField]
        //float aimSpeed = 5;

        [SerializeField]
        float aimError = 3;

        [SerializeField]
        bool alwaysShoot = false;

        [SerializeField]
        Weapon weapon;

        [SerializeField]
        bool useNoFriendlyFireLayer = false;

        [SerializeField]
        GameObject combatMover;



        bool isActive = false;

        TargetSetter targetSetter;

        float sqrWeaponRange;

        bool isDead = false;

        bool isFrozen = false;
       
        
        // Start is called before the first frame update
        void Awake()
        {
            targetSetter = GetComponent<TargetSetter>();
            sqrWeaponRange = weapon.FireRange * weapon.FireRange;
            Deactivate();
        }

        void Start()
        {
            GetComponentInParent<IDamageable>().OnDie += HandleOnDie;
        }

        // Update is called once per frame
        void Update()
        {
            if (isDead || isFrozen)
                return;

            if (!isActive)
            {
                if (alwaysShoot) // Shoot anyway
                {
                    if ((targetSetter.Target.position - transform.position).sqrMagnitude < sqrWeaponRange)
                    {
                        TryShoot();
                    }
                }

                return;
            }
            else
            {
                if ((targetSetter.Target.position - transform.position).sqrMagnitude < sqrWeaponRange) 
                { 
                    // Shoot
                    TryShoot();
                }
            }    

            
            
        }

        public void Activate()
        {
            isActive = true;

            if(combatMover)
                combatMover.GetComponent<IActivable>().Activate();
        }

        public void Deactivate()
        {
            isActive = false;

            if(combatMover)
                combatMover.GetComponent<IActivable>().Deactivate();
        }

        void TryShoot()
        {
            Vector3 dir = (targetSetter.Target.position - weapon.transform.position).normalized;


            // Check collision
            RaycastHit hit;
            int mask;
            if(!useNoFriendlyFireLayer)
                mask = LayerMask.GetMask(new string[] { "Obstacle", "AIAvoidance" });
            else
                mask = LayerMask.GetMask(new string[] { "Obstacle", "AIAvoidance", "NoFriendlyFire" });

            

            if (Physics.SphereCast(weapon.transform.position, 1f, dir, out hit, weapon.FireRange * 1.5f, mask))
                return;


            if (Vector3.Angle(weapon.transform.forward, dir) < aimError)
                weapon.Fire();
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void Freeze(bool value)
        {
            isFrozen = value;
        }

        void HandleOnDie(IDamageable damageable)
        {
            isDead = true; 
        }
    }

}
