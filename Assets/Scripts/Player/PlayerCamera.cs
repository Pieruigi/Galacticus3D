using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class PlayerCamera : MonoBehaviour
    {

        [SerializeField]
        Vector3 distance;

        float lookAtMaxDistance = 5;

        Transform target;
        PlayerController playerController;

        float speed = 50f;
        Vector3 eulerDefault;

        float lastSpeed = 0;
        

        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            playerController = target.GetComponent<PlayerController>();
            eulerDefault = transform.eulerAngles;
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            
            transform.position = target.position + distance;

            

            float dist = Mathf.Lerp(0, lookAtMaxDistance, playerController.CurrentSpeed.magnitude / playerController.MaxSpeed);
            Quaternion def = Quaternion.LookRotation((target.position - transform.position).normalized, Vector3.forward);
            Quaternion tgt = Quaternion.LookRotation((target.position + target.forward*lookAtMaxDistance - transform.position).normalized, Vector3.forward);
            Quaternion curr = Quaternion.Slerp(def, tgt, playerController.CurrentSpeed.sqrMagnitude / playerController.SqrMaxSpeed);

            Camera.main.transform.rotation = curr;



        }
    }

}
