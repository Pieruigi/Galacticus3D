using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField]
        Vector3 axis;

        [SerializeField]
        bool world;

        [SerializeField]
        float speed;

        Vector3 rotAxis;

        void Start()
        {
            if (world)
                rotAxis = axis.normalized;
            else
                rotAxis = (transform.right * axis.x + transform.up * axis.y + transform.forward * axis.z).normalized;
        }

        // Update is called once per frame
        void Update()
        {
            float angle = speed * Time.deltaTime;
            transform.Rotate(rotAxis, angle, world ? Space.World : Space.Self);
        }
    }

}
