using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;
using UnityEngine.Events;

namespace OMTB
{
    /**
     * Catches the player and delivers damage
     * */
    public class Catcher : Shooter
    {
        public UnityAction OnCatch;
        public UnityAction OnUncatch;

        [SerializeField]
        GameObject fighter;

        [SerializeField]
        bool useRigidbody = false;

        [SerializeField]
        float catchSpeed = 100;

        [SerializeField]
        float catchAngularSpeed = 360;

        // Is catching target
        bool isCatching = false;

        // Target caught
        bool caught = false;

        Rigidbody rb;

        TargetSetter targetSetter;

        Collider ownerCollider;

        float catchDistance = 1.25f;

        Vector3 catchPos;
        Vector3 catchRot;

        Uncatcher uncatcher;
        IDamageable targetDamageable;

        protected override void Start()
        {
            base.Start();

            // I may use rigidbody, I may not
            if (useRigidbody)
                rb = Owner.GetComponent<Rigidbody>();

            // Get the owner collider that will come in handy when catching target
            ownerCollider = Owner.GetComponent<Collider>();

            // Get the target setter
            targetSetter = GetComponent<TargetSetter>();

            // Add handle on this damageable
            GetComponentInParent<IDamageable>().OnDie += HandleOnDie;
        }

        private void LateUpdate()
        {
            // If is catching target then compute the position and rotation we want to attach the catcher
            if (isCatching)
            {

                Vector3 fwd = (targetSetter.Target.position - Owner.position).normalized;
                Vector3 pos = targetSetter.Target.position + catchPos;
                Owner.forward = Vector3.RotateTowards(Owner.forward, fwd, catchAngularSpeed * Time.deltaTime, 0.0f);
                Owner.position = Vector3.MoveTowards(Owner.position, pos, catchSpeed * Time.deltaTime);

                if (Owner.position == pos)
                {
                    // Set flags
                    isCatching = false;
                    caught = true;

                    // Set target mesh as parent
                    Owner.parent = targetSetter.Target.GetComponentInChildren<Roller>().transform;
                    
                    // Don't use roller until you detach
                    Owner.GetComponentInChildren<Roller>().enabled = false;

                    // Set the local position and rotation
                    catchPos = Owner.localPosition;
                    catchRot = Owner.localEulerAngles;

                    // Add uncatcher to the target
                    uncatcher = targetSetter.Target.gameObject.AddComponent<Uncatcher>();
                    uncatcher.Catcher = this;

                    // Set damageable target field
                    targetDamageable = targetSetter.Target.GetComponent<IDamageable>();
                    targetDamageable.OnDie += HandleOnDie;

                    OnCatch?.Invoke();
                }
            }
            else
            {
                if (caught)
                {
                    // Set position and rotation
                    Owner.localPosition = catchPos;
                    Owner.localEulerAngles = catchRot;

                    // Try shoot ( weapon code will take care of the shooting cooldown )
                    bool shot = Weapon.Fire();
                    if (shot)
                    {
                        // Apply damage
                        targetDamageable.ApplyDamage(Weapon.DamageAmount);
                    }
                }
            }
           
        }

        public override void Shoot()
        {
            // If target is null do nothing
            if (targetSetter.Target == null)
                return;

            // If target has already been caught do nothing
            if (targetSetter.Target.GetComponent<Uncatcher>() != null)
                return;

            // Start catching if needed
            if (!isCatching && !caught)
            {
                fighter.GetComponent<IActivable>().Deactivate();
                isCatching = true;

                // Catch target
                Catch();

            }
           
            
                
        }

        public void Uncatch()
        {
            // Destroy the uncatcher
            GameObject.Destroy(uncatcher);

            // Remove target handle
            targetDamageable.OnDie -= HandleOnDie;

            // Free damageable target 
            targetDamageable = null;

            // Reset flags
            isCatching = false;
            caught = false;

            // Reset parent and position
            Owner.parent = null;
            Vector3 pos = Owner.transform.position;
            pos.y = 0;
            Owner.transform.position = pos;

            // Enable roller
            Owner.GetComponentInChildren<Roller>().enabled = true;

            // Freeze the AI
            Owner.GetComponent<AI.Freezer>().Freeze();
            
            // Reset physics
            ResetPhysics();

            OnUncatch?.Invoke();
        }

        /**
         * Start catching the target
         * */
        void Catch()
        {
            DisablePhysics();

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

        void DisablePhysics()
        {
            Debug.Log("CatchingTarget");
            // Set rigidbody to kinematic
            if (useRigidbody)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = true;
            }

            // Ignore target and owner collision to avoid physics to go crazy
            Collider targetCollider = targetSetter.Target.GetComponent<Collider>();
            if (ownerCollider && targetCollider)
            {
                Physics.IgnoreCollision(ownerCollider, targetCollider, true);
            }

        }

        void ResetPhysics()
        {
            // Reset rigidbody
            if (useRigidbody)
            {
                rb.velocity = Vector3.zero;
                rb.isKinematic = false;
            }

            // Reset collision
            Collider targetCollider = targetSetter.Target.GetComponent<Collider>();
            if (ownerCollider && targetCollider)
            {
                Physics.IgnoreCollision(ownerCollider, targetCollider, true);
            }
        }

        /**
         * Used both for this and target death
         * */
        void HandleOnDie(IDamageable damageable)
        {
            if (isCatching) isCatching = false;
            else if (caught) Uncatch();
            
        }

        
    }

}
