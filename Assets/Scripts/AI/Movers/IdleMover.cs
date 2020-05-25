using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;
using UnityEngine.AI;

namespace OMTB.AI
{
    public class IdleMover : MonoBehaviour, IActivable, IFreezable
    {
        Vector3 startingPosition;

        NavMeshAgent agent;

        bool isActive = false;

        // Start is called before the first frame update
        void Start()
        {
            startingPosition = transform.position;
            agent = GetComponentInParent<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isActive)
                return;
            
            if ((transform.position - startingPosition).sqrMagnitude > 0.2f)
                agent.SetDestination(startingPosition);
            
        }

        public void Activate()
        {
            //agent.isStopped = false;
            isActive = true;
            if(!agent)
                agent = GetComponentInParent<NavMeshAgent>();
            agent.isStopped = false;
        }

        public void Deactivate()
        {
            //agent.isStopped = true;

            isActive = false;
        }

        public bool IsActive()
        {
            if (!agent)
                agent = GetComponentInParent<NavMeshAgent>();
            return !agent.isStopped;
        }

        public void Freeze(bool value)
        {
            gameObject.SetActive(!value);
        }
    }

}
