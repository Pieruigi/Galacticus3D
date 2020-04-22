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

        float maxAngularSpeed;
        float maxSideSpeed;

        Vector3 lastPos;
        Vector3 lastFwd;

        IRolleable rolleable;

        private void Awake()
        {
            rolleable = target.GetComponent<IRolleable>();
            maxAngularSpeed = rolleable.GetMaxAngularSpeed();
            maxSideSpeed = rolleable.GetMaxSideSpeed();
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
                }


                // Compute angle depending on the angular speed
                angle = Mathf.LerpAngle(0, Mathf.Sign(angularSpeed) * maxRoll /* * angularSpeedWeight*/, Mathf.Abs(angularSpeed) / maxAngularSpeed);

                
            }
            else
            {
                if (lastPos != transform.position)
                {
                    float xDisp = -Vector3.Dot((transform.position - lastPos), transform.right);

                    sideSpeed = xDisp / Time.deltaTime;
                    Debug.Log("SideSpeed:" + +sideSpeed);
                    sideSpeed = Mathf.Clamp(sideSpeed, -maxSideSpeed, maxSideSpeed);

                }

                // Compute angle depending on the angular speed
                angle = Mathf.LerpAngle(0, Mathf.Sign(sideSpeed) * maxRoll , Mathf.Abs(sideSpeed) / maxSideSpeed);
                
            }


            angle = Mathf.Clamp(angle, -maxRoll, maxRoll);

            Quaternion targetRot = Quaternion.Euler(0, 0, angle);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRot, rollSpeed * Time.deltaTime);



            lastFwd = transform.forward;
            lastPos = transform.position;





        }
    }

}
