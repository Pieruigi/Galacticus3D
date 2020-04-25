using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.AI
{
    public class ZigZager : MonoBehaviour, IActivable
    {
        [SerializeField]
        float maxSpeed = 3;

        [SerializeField]
        float acceleration = 5;

        [SerializeField]
        float deceleration = 3;

        [SerializeField]
        float changeDirectionRate = 0.3f;
        
        bool isActive = false;

        float rComp = 0, fComp = 0;
        float currentSpeed = 0;
       
        float changeTime;
        System.DateTime lastChange;

        float checkTime = 0.5f; // Check for collisions

        System.DateTime lastCheck;

        Transform root;

        bool isChangingDirection = false;
        bool isDecelerating = false;

        // Start is called before the first frame update
        void Start()
        {
            root = transform.root;
            SetChangeTime();
            Debug.Log("ChangeTime:" + changeTime);


        }

        // Update is called once per frame
        void Update()
        {
            Debug.Log(Vector3.MoveTowards(new Vector3(12, 0, 0.2f), new Vector3(-1, 0, -.13f), 0.3f));

            if (!isActive)
                return;

            // Check if enough time has passed from the last time the ship changes its direction
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


            Debug.Log("Root.right:"+root.right*rComp);
            Debug.Log("Root.forward:" + root.forward * fComp);
            root.position += (root.right * rComp + root.forward * fComp).normalized * currentSpeed * Time.deltaTime;
            
           // root.position = targetPos;

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
            SetChangeTime();
        }

        void SetChangeTime()
        {
            changeTime = 1f / Random.Range(changeDirectionRate * 0.8f, changeDirectionRate * 1.2f);
        }
    }

}
