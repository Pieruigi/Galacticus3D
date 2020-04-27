using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.AI
{
    [RequireComponent(typeof(TargetSetter))]
    public class ZigZager : MonoBehaviour, IActivable
    {
        [SerializeField]
        float maxSpeed = 3;
        float maxSpeedDefault;

        [SerializeField]
        float acceleration = 5;
        float accelerationDefault;

        [SerializeField]
        float deceleration = 3;
        float decelerationDefault;

        [SerializeField]
        float angularSpeed;
        float angularSpeedDefault;

        [SerializeField]
        float changeDirectionRate = 0.3f;
        float changeDirectionRateDefault;

        [SerializeField]
        bool useRigidbody = false;
        
        bool isActive = false;

        float rComp = 0, fComp = 0;
        float currentSpeed = 0;
        Vector3 targetPos;
        TargetSetter targetSetter;
       
        float changeTime;
        System.DateTime lastChange;

        float checkTime = 0.5f; // Check for collisions

        System.DateTime lastCheck;

        Transform root;

        bool isChangingDirection = false;
        bool isDecelerating = false;

        Rigidbody rb;

        private void Awake()
        {
            maxSpeedDefault = maxSpeed;
            accelerationDefault = acceleration;
            decelerationDefault = deceleration;
            angularSpeedDefault = angularSpeed;
            changeDirectionRateDefault = changeDirectionRate;
        }

        // Start is called before the first frame update
        void Start()
        {
            root = transform.root;

            if(useRigidbody)
                rb = root.GetComponent<Rigidbody>();

            targetSetter = GetComponent<TargetSetter>();

            RandomizeValues();

            Debug.Log("ChangeTime:" + changeTime);


        }

        // Update is called once per frame
        void Update()
        {

            if (!isActive)
                return;

            // Check whether enough time has passed from the last time the ship's changed its direction
            if(!isChangingDirection && (System.DateTime.UtcNow - lastChange).TotalSeconds > changeTime)
            {
                isChangingDirection = true;
                isDecelerating = true;
            }

            if(!isDecelerating && currentSpeed < maxSpeed)
            {
                currentSpeed += acceleration * Time.deltaTime;
                if (currentSpeed > maxSpeed)
                    currentSpeed = maxSpeed;
            }

            if (isDecelerating)
            {
                // Decrease speed
                currentSpeed -= deceleration * Time.deltaTime;
                if (currentSpeed < 0)
                {
                    currentSpeed = 0;
                    isDecelerating = false;
                }
                    
                // Check if is changing direction
                if(isChangingDirection && currentSpeed == 0)
                {
                    ChangeDirection();
                    lastChange = System.DateTime.UtcNow;
                    isChangingDirection = false;
                  
                }
            }

            
            // Apply movement to root.transform ( not using rigidbody )
            if(!useRigidbody)
                root.position += (root.right * rComp + root.forward * fComp).normalized * currentSpeed * Time.deltaTime;

            // Aim player
            Quaternion targetRot = Quaternion.LookRotation((targetSetter.Target.position - root.transform.position).normalized);
            Quaternion rot = Quaternion.RotateTowards(root.transform.rotation, targetRot, angularSpeed * Time.deltaTime);
            root.rotation = rot;


        }

        private void FixedUpdate()
        {
            // Apply movement to rigidbody
            if (useRigidbody)
                rb.MovePosition(rb.position + (root.right * rComp + root.forward * fComp).normalized * currentSpeed * Time.fixedDeltaTime);
        }

        public void Activate()
        {
            isActive = true;
            Reset();
        }

        public void Deactivate()
        {
            isActive = false;
            Reset();
        }

        void Reset()
        {
            rComp = 0;
            fComp = 0;
            currentSpeed = 0;
            isChangingDirection = false;
            isDecelerating = false;

        }

        void ChangeDirection()
        {
            // Change direction
            if (rComp == 0)
                rComp = (Random.Range(0, 2) == 0) ? -1f : 1f;
            else
                rComp = -rComp;

            fComp = Random.Range(-0.2f, 0.2f);

            // Update che change time with a new random value            
            RandomizeValues();
        }

        void RandomizeValues()
        {
            maxSpeed = Random.Range(maxSpeedDefault * 0.8f, maxSpeedDefault * 1.2f);
            acceleration = Random.Range(accelerationDefault * 0.8f, accelerationDefault * 1.2f);
            deceleration = Random.Range(decelerationDefault * 0.8f, decelerationDefault * 1.2f);
            angularSpeed = Random.Range(angularSpeedDefault * 0.8f, angularSpeedDefault * 1.2f);
            changeDirectionRate = Random.Range(changeDirectionRateDefault * 0.5f, changeDirectionRateDefault * 2f);
            changeTime = 1f / changeDirectionRate;
        }

    }

}
