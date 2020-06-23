using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;
using UnityEngine.AI;

namespace OMTB.AI
{
    public class FlybyMover : MonoBehaviour, IActivable, IRolleable, IFreezable
    {
        [SerializeField]
        float speed = 15f;

        [SerializeField]
        float acceleration = 5;

        float flyingAwaySpeedMul = 2f;
        float flyingAwaySpeed;
        float currSpeed;

        [SerializeField]
        float angularSpeed = 120f;
        float angularSpeedRadians;
                

        [SerializeField]
        bool useRigidbody;



        Rigidbody rb;

        bool isActive = false;

        TargetSetter targetSetter;
        bool flyAway = false;
        float sqrTargetMinDist;
        float sqrTargetMaxDist;

        [SerializeField]
        float targetMinDistance = 20f;

        [SerializeField]
        float targetMaxDistance = 40;
        
        Vector3 targetDir;
        Vector3 currentDir;
        Transform root;

        NavMeshAgent agent;

        void Awake()
        {
            currSpeed = 0;
            flyingAwaySpeed = speed * flyingAwaySpeedMul;
            sqrTargetMinDist = targetMinDistance * targetMinDistance;
            sqrTargetMaxDist = targetMaxDistance * targetMaxDistance;
            angularSpeedRadians = Mathf.Deg2Rad * angularSpeed;
            root = transform.root;
            currentDir = root.forward;
            targetSetter = GetComponent<TargetSetter>();
            rb = root.GetComponent<Rigidbody>();
            agent = root.GetComponent<NavMeshAgent>();
            Deactivate();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!isActive)
                return;

            ReachMinimumSpeed();

            if (!flyAway) 
            {
                Targeting();
               
            }
            else
            {
                FlyingAway();
            }

            // Calculate current direction from target
            targetDir.Normalize();
            float angSpeed = angularSpeedRadians;
            if (flyAway)
                angSpeed *= 1.2f;
            
            currentDir = Vector3.RotateTowards(currentDir, targetDir, angSpeed * Time.deltaTime, 0.0f).normalized;
            
            root.forward = currentDir.normalized;

            if (!useRigidbody)
            {
                root.position += currentDir * currSpeed * Time.deltaTime;
            }
        }

        void FixedUpdate()
        {
            if (!isActive)
                return;

            if (useRigidbody)
            {
                rb.MovePosition(rb.position + currentDir * currSpeed * Time.fixedDeltaTime);
            }
        }

        public void Activate()
        {
            if (agent)
                agent.isStopped = true;
            isActive = true;
        }

        public void Deactivate()
        {
            isActive = false;
            Reset();
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
                currSpeed -= acceleration * Time.deltaTime;
                if (currSpeed < speed)
                    currSpeed = speed;
            }

            // Targeting player
            targetDir = targetSetter.Target.position - root.position;

            // If too close fly away
            if ((root.position - targetSetter.Target.position).sqrMagnitude < sqrTargetMinDist)
            {
                flyAway = true;
                targetDir = -currentDir;
            }
        }

        void FlyingAway()
        {
            // Accelerate 
            if(currSpeed < flyingAwaySpeed)
            {
                currSpeed += acceleration * Time.deltaTime;
                if (currSpeed > flyingAwaySpeed)
                    currSpeed = flyingAwaySpeed;
            }

            // If enemy is far enough restart targeting
            if ((root.position - targetSetter.Target.position).sqrMagnitude > sqrTargetMaxDist)
            {
                flyAway = false;
            }
            
            // Check if there is something in front of the ship
            float dist = speed*1.5f;
            RaycastHit hit;
            if(Utils.AIUtil.HitObstacle(transform.position, targetDir, dist, out hit))
            {
                Debug.Log("hit an obstacle:" + hit.transform.gameObject);
                Debug.Log("hit.normal:" + hit.normal);
                targetDir = hit.normal;
            }
                

            //if (Physics.Raycast(transform.position + Vector3.down*0.5f, targetDir, out hit, dist, LayerMask.GetMask(new string[] { "Obstacle" })))
            //{
            //    Debug.Log("hit an obstacle:"+hit.transform.gameObject );
            //    Debug.Log("hit.normal:" + hit.normal);
            //    //targetDir = Quaternion.Euler(0, Random.Range(-10f, 10f), 0) * hit.normal;
            //    targetDir = hit.normal;
            //}

        }

        public void Freeze(bool value)
        {
            gameObject.SetActive(!value);
            Reset();
        }

        void Reset()
        {
            flyAway = false;
            currSpeed = speed;
        }

        void ReachMinimumSpeed()
        {
            if(currSpeed < speed)
            {
                currSpeed += Time.deltaTime * acceleration;

                if (!flyAway && currSpeed > speed)
                    currSpeed = speed;
            }
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
