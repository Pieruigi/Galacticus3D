using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.AI
{
    public class DestayerMover : MonoBehaviour
    {
        [SerializeField]
        float speed;

        [SerializeField]
        float rotationSpeed = 5f;
        float rotSpeedRadians;

        [SerializeField]
        float maxTargetDistance;
        float sqrMaxTargetDistance;
       
        float maxDistance = 0.3f;

        TargetSetter targetSetter;

        // Start is called before the first frame update
        void Start()
        {
            targetSetter = GetComponent<TargetSetter>();
            sqrMaxTargetDistance = maxTargetDistance * maxTargetDistance;
            rotSpeedRadians = rotationSpeed * Mathf.Deg2Rad;
        }

        // Update is called once per frame
        void Update()
        {
            // Move slowly
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * speed * Time.deltaTime, speed);

            
            
            RaycastHit hit;
            // Check whether enemy ship is going against borders or not
            if (Utils.AIUtil.HitObstacle(transform.position, transform.forward, speed * 10f, out hit, /* excludeAvoidance */true))
            {
                // Compute target direction
                Vector3 targetDir = hit.normal;

                // Turn to avoid border collision
                transform.forward = Vector3.RotateTowards(transform.forward, hit.normal,  rotSpeedRadians * Time.deltaTime, 0f);
            }
            else
            {
                Debug.Log("Dist:" + (transform.position - targetSetter.Target.position).magnitude);
                // If player is too far then follow it
                if ((transform.position - targetSetter.Target.position).sqrMagnitude > sqrMaxTargetDistance)
                {
                    Vector3 dir = targetSetter.Target.position - transform.position;
                    dir.y = 0;
                    dir.Normalize();
                    //if (Vector3.Dot(dir, transform.forward) > 0)
                        transform.forward = Vector3.RotateTowards(transform.forward, dir, rotSpeedRadians * Time.deltaTime, 0f);
                    //else
                    //    transform.forward = Vector3.RotateTowards(transform.forward, -dir, rotSpeedRadians * Time.deltaTime, 0f);
                }
                else
                {
                    // Get player position 
                    Vector3 dir = targetSetter.Target.position - transform.position;
                    dir.y = 0;
                    dir.Normalize();
                    if (Vector3.Dot(dir, transform.right) > 0)
                        transform.right = Vector3.RotateTowards(transform.right, dir, rotSpeedRadians * Time.deltaTime, 0f);
                    else
                        transform.right = Vector3.RotateTowards(transform.right, -dir, rotSpeedRadians * Time.deltaTime, 0f);
                }
                
            }
            

        }

    }

}
