using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class PlayerCamera : MonoBehaviour
    {

        [SerializeField]
        Vector3 distance;

        float distMax = 10;

        Vector3 currDisp;

        Transform target;
        PlayerController playerController;


        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            playerController = target.GetComponent<PlayerController>();
            transform.position = target.position + distance;
            transform.rotation = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.forward);
        }

        // Update is called once per frame
        void Update()
        {

        }



        private void LateUpdate()
        {

            transform.position = target.position + distance;
            
            float dist = Mathf.SmoothStep(0, 1, playerController.CurrentVelocity.magnitude / playerController.MaxSpeed);

            //transform.position += Vector3.up * dist * distMax;

            currDisp = Vector3.MoveTowards(currDisp, Vector3.up * dist * distMax, 4 * Time.deltaTime);
            transform.position += currDisp;
        }


    }

}
