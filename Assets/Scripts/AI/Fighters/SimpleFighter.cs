using System.Collections;
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
        
        // Start is called before the first frame update
        void Awake()
        {
            targetSetter = GetComponent<TargetSetter>();
            sqrWeaponRange = weapon.FireRange * weapon.FireRange;
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
            else
            {
                // Shoot
                TryShoot();
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

            Debug.Log("TryShoot");
            // Check collision
            RaycastHit hit;
            int mask;
            if(!useNoFriendlyFireLayer)
                mask = LayerMask.GetMask(new string[] { "Obstacle", "AIAvoidance" });
            else
                mask = LayerMask.GetMask(new string[] { "Obstacle", "AIAvoidance", "NoFriendlyFire" });

            

            if (Physics.SphereCast(weapon.transform.position, 1f, dir, out hit, weapon.FireRange * 1.5f, mask))
                return;

            Debug.Log("TryShoot 1");

            if (Vector3.Angle(weapon.transform.forward, dir) < aimError)
                weapon.Fire();
        }

        public bool IsActive()
        {
            return isActive;
        }

        public void Freeze(bool value)
        {
            gameObject.SetActive(!value);
        }
    }

}
