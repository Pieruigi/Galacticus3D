using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class ParticleSystemRandomizer : MonoBehaviour
    {
        

        // Start is called before the first frame update
        void Start()
        {
            
            for(int i=0; i<transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                
                bool removed = ((int)Random.Range(0,3) == 0) ? false : true;
                if (removed)
                {
                    Destroy(child.GetComponentInChildren<ParticleSystem>());
                    Destroy(child.GetComponentInChildren<Light>().gameObject);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


