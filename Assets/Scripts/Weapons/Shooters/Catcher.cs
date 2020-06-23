using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB
{
    /**
     * Catches the player and delivers damage
     * */
    public class Catcher : Shooter
    {
        [SerializeField]
        GameObject fighter;

        [SerializeField]
        bool useRigidbody = false;

        bool isCatching = false;
        bool catched = false;

        Damager damager;

        Rigidbody rb;

        TargetSetter targetSetter;

        Collider coll;

        float catchDistance = 3.5f;

        

        protected override void Start()
        {
            base.Start();

            if (useRigidbody)
                rb = Owner.GetComponent<Rigidbody>();

            // Get collider
            coll = Owner.GetComponent<Collider>();

            // Add the damager
            damager = gameObject.AddComponent<Melee>();
            damager.Init(new DamagerConfig() { Amount = Weapon.DamageAmount, MaxRange = Weapon.DamageMaxRange, MinRange = Weapon.DamageMinRange, Owner = this.Owner });

            targetSetter = GetComponent<TargetSetter>();
        }

        private void LateUpdate()
        {
            
            if (isCatching)
            {
                Vector3 fwd = (targetSetter.Target.position - Owner.position).normalized;
                Vector3 pos = targetSetter.Target.position - Owner.forward * catchDistance;
                Owner.forward = Vector3.RotateTowards(Owner.forward, fwd, 720f * Time.deltaTime, 0.0f);
                Owner.position = Vector3.MoveTowards(Owner.position, pos, 150 * Time.deltaTime);

                if (Owner.position == pos)
                {
                    isCatching = false;
                    catched = true;
                    Owner.parent = targetSetter.Target;
                }
            }

            //if (catched)
            //{
            //    Vector3 fwd = (targetSetter.Target.position - Owner.position).normalized;
            //    Vector3 pos = targetSetter.Target.position - Owner.forward * catchDistance;
            //    Owner.forward = fwd;
            //    Owner.position = pos;
            //}
        }

        public override void Shoot()
        {

            // If is already catching the target just deactivate the fighter and return 
            if(!isCatching && !catched)
            {
                fighter.GetComponent<IActivable>().Deactivate();
                isCatching = true;

                // Catch target
                DisablePhysics();
            }

            

            Weapon.Fire(); // The weapon itself takes care about shooting cooldown
        }

        void DisablePhysics()
        {
            Debug.Log("CatchingTarget");
            // Set rigidbody to kinematic
            if (useRigidbody)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
                coll.enabled = false;
                
            }

            
        }

        void ResetPhysics()
        {
            // Reset rigidbody
            if (useRigidbody)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = false;
                coll.enabled = true;
            }
        }
    }

}
