using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class Roller : MonoBehaviour
    {
        
        float maxAngularSpeed = 90;
        float maxSideSpeed = 3;

        float maxRoll = 30;
        float angularDecSpeed = 180;

        Vector3 lastPos;
        Vector3 lastFwd;

        float currAngularSpeed = 0;
        float currSideSpeed = 0;

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
            //transform.localEulerAngles = Vector3.zero;

            if (transform.forward != lastFwd)
            {
                currAngularSpeed = Vector3.SignedAngle(transform.forward, lastFwd, Vector3.up) / Time.deltaTime;
                Debug.Log("**********************************************************transform.fwd:" + transform.forward);
                currAngularSpeed = Mathf.Clamp(currAngularSpeed, -maxAngularSpeed, maxAngularSpeed);
            }
            else
            {
                if (currAngularSpeed > 0)
                {
                    currAngularSpeed -= angularDecSpeed * Time.deltaTime;
                    if (currAngularSpeed < 0)
                        currAngularSpeed = 0;
                }
                else if (currAngularSpeed < 0)
                {
                    currAngularSpeed += angularDecSpeed * Time.deltaTime;
                    if (currAngularSpeed > 0)
                        currAngularSpeed = 0;
                }


            }

            float angle = Mathf.LerpAngle(0, Mathf.Sign(currAngularSpeed) * maxRoll, Mathf.Abs(currAngularSpeed) / maxAngularSpeed);

            //if(angle == 0)
            //{
            //    if (lastPos != transform.position)
            //    {
            //        float xDisp = -Vector3.Dot((transform.position - lastPos), transform.right);
            //        currSideSpeed = xDisp / Time.deltaTime;
            //        currSideSpeed = Mathf.Clamp(currSideSpeed, -maxSideSpeed, maxSideSpeed);
            //        resetAngle = false;
            //    }

            //    angle = Mathf.Lerp(0, Mathf.Sign(currSideSpeed) * maxRoll, Mathf.Abs(currSideSpeed) / maxSideSpeed);
            //}

          

            transform.localEulerAngles = new Vector3(0, 0, angle);
            //transform.Rotate(transform.forward, angle, Space.World);

            Debug.Log("AngularSpeed:" + currAngularSpeed);
            lastFwd = transform.forward;
            lastPos = transform.position;
            Debug.Log("LastForward:" + lastFwd);
        }
    }

}
