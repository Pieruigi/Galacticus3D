using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB
{
    public class Roller : MonoBehaviour
    {
        [SerializeField]
        float maxRoll = 60;

        [SerializeField]
        float rollSpeed = 90;

        [SerializeField]
        GameObject target;

        [SerializeField]
        bool useRigidbody;

        float maxAngularSpeed;
        float maxSideSpeed;

        Vector3 lastPos;
        Vector3 lastFwd;

        IRolleable rolleable;
        Rigidbody rb;

        System.DateTime lastChange;


        private void Awake()
        {
            rolleable = target.GetComponent<IRolleable>();
            maxAngularSpeed = rolleable.GetMaxAngularSpeed();
            maxSideSpeed = rolleable.GetMaxSideSpeed();
            Debug.Log("maxSideSpeed:" + maxSideSpeed);

            if (useRigidbody)
                rb = target.GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        void Start()
        {
            lastPos = transform.position;
            lastFwd = transform.forward;

                
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            if (!useRigidbody)
                Roll();
        }

        private void FixedUpdate()
        {
            if (useRigidbody)
                Roll();
        }

        private void Roll()
        {
            //Quaternion targetRot2 = Quaternion.Euler(0,0,30);
            //transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRot2, 20 * Time.deltaTime);
            
            
            float angle = 0;
            float angularSpeed = 0;
            float sideSpeed = 0;
            
            if (!rolleable.IsAiming())
            {
                if (transform.forward != lastFwd) // Ship has some angular speed
                {
                    angularSpeed = Vector3.SignedAngle(transform.forward, lastFwd, Vector3.up) / Time.deltaTime;
                    angularSpeed = Mathf.Clamp(angularSpeed, -maxAngularSpeed, maxAngularSpeed);
                    lastChange = System.DateTime.UtcNow;
                }


                // Compute angle depending on the angular speed
                angle = Mathf.LerpAngle(0, Mathf.Sign(angularSpeed) * maxRoll, Mathf.Abs(angularSpeed) / maxAngularSpeed);

                
            }
            else
            {
                Vector3 pos = useRigidbody ? rb.position : transform.position;
                if (lastPos != pos)
                {
                    lastChange = System.DateTime.UtcNow;
                    float xDisp = -Vector3.Dot((pos - lastPos), transform.right);

                    sideSpeed = xDisp / Time.deltaTime;
                  
                    sideSpeed = Mathf.Clamp(sideSpeed, -maxSideSpeed, maxSideSpeed);

                }

                // Compute angle depending on the angular speed
                angle = Mathf.LerpAngle(0, Mathf.Sign(sideSpeed) * maxRoll , Mathf.Abs(sideSpeed) / maxSideSpeed);
                
            }


            if ((System.DateTime.UtcNow - lastChange).TotalSeconds < 1f)
                angle = Mathf.Clamp(angle, -maxRoll, maxRoll);
            else
                angle = 0;

            Quaternion targetRot = Quaternion.Euler(0, 0, angle);
            
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRot, rollSpeed * Time.deltaTime);
            


            lastFwd = transform.forward;
            lastPos = useRigidbody ? rb.position : transform.position;

        }


    }

}
