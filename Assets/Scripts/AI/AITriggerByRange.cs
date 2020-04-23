using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using OMTB.Interfaces;

namespace OMTB.AI
{
    public class AITriggerByRange : MonoBehaviour, ITriggerable
    {
        
        Transform target;


        // Start is called before the first frame update
        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void IsTriggered()
        {
            throw new System.NotImplementedException();
        }

    }
}

