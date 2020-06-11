using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.AI
{
    public class FlybyMover : MonoBehaviour, IActivable, IRolleable, IFreezable
    {
        [SerializeField]
        float speed = 15f;

        float speedAcc = 5;
        float flyingAwaySpeedMul = 2f;
        float flyingAwaySpeed;
        float currSpeed;

        [SerializeField]
        float angularSpeed = 60f;
        float angularSpeedRadians;

        

        [SerializeField]
        bool useRigidbody;



        Rigidbody rb;

        bool isActive = false;

        TargetSetter targetSetter;
        bool flyAway = false;
        float sqrTargetMinDist;
        float sqrTargetMaxDist;

        float targetMinDistance = 20f;
        float targetMaxDistance = 40;
        
        Vector3 targetDir;
        Vector3 currentDir;
        Transform root;

        void Awake()
        {
            currSpeed = speed;
            flyingAwaySpeed = speed * flyingAwaySpeedMul;
            sqrTargetMinDist = targetMinDistance * targetMinDistance;
            sqrTargetMaxDist = targetMaxDistance * targetMaxDistance;
            angularSpeedRadians = Mathf.Deg2Rad * angularSpeed;
            root = transform.root;
            currentDir = root.forward;
            targetSetter = GetComponent<TargetSetter>();
            rb = root.GetComponent<Rigidbody>();
            Deactivate();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!flyAway) 
            {
                Targeting();
                Debug.Log("Targeting...");
            }
            else
            {
                FlyingAway();
            }

            // Calculate current direction from target
            targetDir.Normalize();
            if(!flyAway)
                currentDir = Vector3.RotateTowards(currentDir, targetDir, angularSpeedRadians * Time.deltaTime, 0.0f).normalized;
            else
                currentDir = Vector3.RotateTowards(currentDir, -targetDir, angularSpeedRadians * Time.deltaTime * 1.2f, 0.0f).normalized;

            root.forward = currentDir.normalized;

            if (!useRigidbody)
            {
                root.position += currentDir * currSpeed * Time.deltaTime;
            }
        }

        void FixedUpdate()
        {
            if (useRigidbody)
            {
                rb.MovePosition(rb.position + currentDir * currSpeed * Time.fixedDeltaTime);
            }
        }

        public void Activate()
        {
            isActive = true;
        }

        public void Deactivate()
        {
            isActive = false;
        }

        public bool IsActive()
        {
            return isActive;
        }

        public float GetMaxAngularSpeed()
        {
            return angularSpeed;
        }

        public float GetMaxSideSpeed()
        {
            return speed;
        }

        public bool IsAiming()
        {
            return false;
        }

        void Targeting()
        {
            // Decelerate
            if (currSpeed > speed)
            {
                currSpeed -= speedAcc * Time.deltaTime;
                if (currSpeed < speed)
                    currSpeed = speed;
            }

            // Targeting player
            targetDir = targetSetter.Target.position - root.position;

            // If too close fly away
            if ((root.position - targetSetter.Target.position).sqrMagnitude < sqrTargetMinDist)
            {
                flyAway = true;
            }
        }

        void FlyingAway()
        {
            // Accelerate 
            if(currSpeed < flyingAwaySpeed)
            {
                currSpeed += speedAcc * Time.deltaTime;
                if (currSpeed > flyingAwaySpeed)
                    currSpeed = flyingAwaySpeed;
            }

            // If enemy is far enough restart targeting
            if ((root.position - targetSetter.Target.position).sqrMagnitude > sqrTargetMaxDist)
            {
                flyAway = false;
            }

            // Check if there is something in front of the ship
            float dist = speed * 3f;

            
        }

        public void Freeze(bool value)
        {
            gameObject.SetActive(!value);
        }

        //void FlyingAway()
        //{
        //     Get direction towards player
        //    Vector3 d = targetSetter.Target.position - root.position;
        //    d.y = 0;
        //    d.Normalize();

        //     Enemy is following a safe direction far from player
        //    if (Vector3.Angle(targetDir, d) > flyAwayCollisionDist)
        //        return;

        //     Check right side
        //    Vector3 dir = Quaternion.Euler(0, flyAwayAngle, 0) * d;

        //    bool hasNewDir = false;
        //    if(!Physics.Raycast(root.position, dir, flyAwayCollisionDist))
        //    {
        //         Lean right
        //        hasNewDir = true;
        //    }

        //    if(!hasNewDir)
        //    {
        //        dir = Quaternion.Euler(0, -flyAwayAngle, 0) * d;
        //        if (!Physics.Raycast(root.position, dir, flyAwayCollisionDist))
        //        {
        //             Lean left
        //            hasNewDir = true;
        //        }
        //    }

        //    if(hasNewDir)
        //        targetDir = dir; 
        //}
    }

}
