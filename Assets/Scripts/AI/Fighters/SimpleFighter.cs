﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.AI
{
    [RequireComponent(typeof(TargetSetter))]
    public class SimpleFighter : MonoBehaviour, IActivable
    {
        [SerializeField]
        float aimSpeed = 5;

        [SerializeField]
        float aimError = 3;

        [SerializeField]
        bool alwaysShoot = false;

        [SerializeField]
        Weapon weapon;

        [SerializeField]
        GameObject combatMover;

        bool isActive = false;

        TargetSetter targetSetter;

        float sqrWeaponRange;
        
        // Start is called before the first frame update
        void Start()
        {
            targetSetter = GetComponent<TargetSetter>();
            sqrWeaponRange = weapon.Range * weapon.Range;
            Deactivate();
        }

        // Update is called once per frame
        void Update()
        {
         
            if (!isActive)
            {
                if (alwaysShoot) // Shoot anyway
                {
                    if((targetSetter.Target.position - transform.position).sqrMagnitude < sqrWeaponRange)
                    {
                        TryShoot();
                    }
                }

                return;
            }
                

            // Shoot
            TryShoot();
            
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
            if (Vector3.Angle(weapon.transform.forward, dir) < aimError)
                weapon.Fire();
        }

        public bool IsActive()
        {
            return isActive;
        }
    }

}
