using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.AI
{
    public class DestayerAI : MonoBehaviour
    {
        
        List<Weapon> cannons;

        

        // Start is called before the first frame update
        void Start()
        {
            cannons = new List<Weapon>(GetComponentsInChildren<Weapon>());

            IActivable[] activables = GetComponentsInChildren<IActivable>();
            foreach (IActivable activable in activables)
                activable.Activate();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


