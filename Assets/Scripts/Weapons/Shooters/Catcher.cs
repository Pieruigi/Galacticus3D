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

        [SerializeField]
        float catchSpeed = 20;

        [SerializeField]
        float catchAngularSpeed = 60;

        bool isCatching = false;
        bool catched = false;

        Damager damager;

        Rigidbody rb;

        TargetSetter targetSetter;

        Collider coll;

        float catchDistance = 1.25f;

        Vector3 catchPos;
        Vector3 catchRot;

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
                Vector3 pos = targetSetter.Target.position + catchPos;
                Owner.forward = Vector3.RotateTowards(Owner.forward, fwd, catchAngularSpeed * Time.deltaTime, 0.0f);
                Owner.position = Vector3.MoveTowards(Owner.position, pos, catchSpeed * Time.deltaTime);

                if (Owner.position == pos)
                {
                    isCatching = false;
                    catched = true;
                    Owner.parent = targetSetter.Target.GetComponentInChildren<Roller>().transform;
                    Owner.GetComponentInChildren<Roller>().enabled = false;
                    catchPos = Owner.localPosition;
                    catchRot = Owner.localEulerAngles;
                }
            }

            if (catched)
            {
                Owner.localPosition = catchPos;
                Owner.localEulerAngles = catchRot;
            }
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

            // Raycast to get position and rotation
            RaycastHit[] hits;
            Vector3 dir = targetSetter.Target.position - Owner.position;

            hits = Physics.RaycastAll(Owner.position, dir.normalized, dir.magnitude);
            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {
                    if ("Player".Equals(hit.transform.gameObject.tag))
                    {
                        Vector3 disp = (hit.point - hit.transform.position).normalized * catchDistance;
                        catchPos = hit.point + disp - hit.transform.position;
                        catchPos.y = 0;
                    }
                }
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
