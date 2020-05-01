using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class RandomRotator : MonoBehaviour
    {
        Vector3 axis;

        float maxSpeed = 10;
        
        // Start is called before the first frame update
        void Start()
        {
                axis = new Vector3(Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed));
                
        }

        // Update is called once per frame
        void Update()
        {
            transform.Rotate(axis*Time.deltaTime);

        }
    }

}
