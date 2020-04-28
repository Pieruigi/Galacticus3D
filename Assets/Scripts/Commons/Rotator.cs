using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class Rotator : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(new Vector3(0.05f, 0.03f, 0.01f));

        }
    }

}
