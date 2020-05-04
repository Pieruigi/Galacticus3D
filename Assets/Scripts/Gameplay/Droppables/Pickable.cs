using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Gameplay
{
    public abstract class Pickable : MonoBehaviour
    {
        public abstract void PickUp();

        protected virtual void Awake()
        {
            
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }
    }
}

