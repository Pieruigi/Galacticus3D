using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.AI
{
    public class DestayerMover : MonoBehaviour
    {
        [SerializeField]
        float speed;

       
        float maxAngle = 0.1f;

        TargetSetter targetSetter;

        // Start is called before the first frame update
        void Start()
        {
            targetSetter = GetComponent<TargetSetter>();
            
        }

        // Update is called once per frame
        void Update()
        {
            // Move slowly
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * speed * Time.deltaTime, speed);

            // Get player position 
            Vector3 dir = targetSetter.Target.position - transform.position;
            dir.y = 0;
            dir.Normalize();
            if(Vector3.Dot(dir, transform.right) > 0)
                transform.right = Vector3.MoveTowards(transform.right, dir * Time.deltaTime, maxAngle);
            else
                transform.right = Vector3.MoveTowards(transform.right, -dir * Time.deltaTime, maxAngle);

        }

    }

}
