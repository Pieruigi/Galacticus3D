using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class Bouncer : MonoBehaviour
    {
        [SerializeField]
        [Range(0,20)]
        float force = 10;
        public float Force
        {
            get { return force; }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

    }

}
