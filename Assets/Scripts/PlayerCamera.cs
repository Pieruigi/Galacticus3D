using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class PlayerCamera : MonoBehaviour
    {

        [SerializeField]
        Vector3 distance;

        Transform target;

        float speed = 50f;

        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            //transform.position = Vector3.MoveTowards(transform.position, target.position + distance, speed * Time.deltaTime);

            transform.position = target.position + distance;
        }
    }

}
