using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.AI
{
    public class Freezer : MonoBehaviour
    {
        [SerializeField]
        float timer = 0;

        float currentTimer = 0;

        List<IFreezable> freezables;

        

        bool isFrozen = false;
       
        // Start is called before the first frame update
        void Start()
        {
            freezables = new List<IFreezable>(GetComponentsInChildren<IFreezable>());
        }

        // Update is called once per frame
        void Update()
        {
            if (isFrozen)
            {
                currentTimer -= Time.deltaTime;
                if (currentTimer < 0)
                {
                    currentTimer = 0;
                    Unfreeze();
                }
                    
            }
        }

        public void Freeze()
        {
            currentTimer += timer;
            if (!isFrozen)
            {
                isFrozen = true;
                foreach (IFreezable freezable in freezables)
                    freezable.Freeze(true);
            }
        }

        void Unfreeze()
        {
            if (isFrozen)
            {
                isFrozen = false;
                foreach (IFreezable freezable in freezables)
                    freezable.Freeze(false);
            }
        }
    }

}
