using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class Roller : MonoBehaviour
    {
        
        float maxAngularSpeed = 40;
        //float maxSideSpeed = 0.3f;

        float maxRoll = 60;
        float rollSpeed = 90;

        float angularSpeedWeight = 0.7f;

        //Vector3 lastPos;
        Vector3 lastFwd;

        //float currAngularSpeed = 0;
        //float currSideSpeed = 0;

        

        // Start is called before the first frame update
        void Start()
        {
            //lastPos = transform.position;
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
            //float sideSpeed = 0;
            float angularComp = 0;
            //float sideComp = 0;

            if (transform.forward != lastFwd) // Ship has some angular speed
            {
                angularSpeed = Vector3.SignedAngle(transform.forward, lastFwd, Vector3.up) / Time.deltaTime;
                angularSpeed = Mathf.Clamp(angularSpeed, -maxAngularSpeed, maxAngularSpeed);
            }


            // Compute angle depending on the angular speed
            angularComp = Mathf.LerpAngle(0, Mathf.Sign(angularSpeed) * maxRoll /* * angularSpeedWeight*/, Mathf.Abs(angularSpeed) / maxAngularSpeed);
        

            ////if(angularComp == 0 && transform.localEulerAngles.z == 0)
            ////{
            //    if (lastPos != transform.position)
            //    {
            //        float xDisp = -Vector3.Dot((transform.position - lastPos), transform.right);

            //        sideSpeed = xDisp / Time.deltaTime;
            //    Debug.Log("SideSpeed:" + +sideSpeed);
            //        sideSpeed = Mathf.Clamp(sideSpeed, -maxSideSpeed, maxSideSpeed);

            //    }
                
            ////}
            
           // sideComp = Mathf.LerpAngle(0, Mathf.Sign(sideSpeed) * maxRoll /** (1 - angularSpeedWeight)*/, Mathf.Abs(sideSpeed) / maxSideSpeed);


            angle = angularComp;// + sideComp;
            angle = Mathf.Clamp(angle, -maxRoll, maxRoll);

            Quaternion targetRot = Quaternion.Euler(0, 0, angle);
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRot, rollSpeed * Time.deltaTime);





            lastFwd = transform.forward;
            //lastPos = transform.position;





        }
    }

}
