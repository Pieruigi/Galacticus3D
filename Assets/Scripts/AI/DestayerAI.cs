using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.AI
{
    public class DestayerAI : MonoBehaviour
    {
        
        List<Weapon> cannons;

        int cannonCount;
           

        // Start is called before the first frame update
        void Start()
        {
            cannons = new List<Weapon>(GetComponentsInChildren<Weapon>());
            cannonCount = cannons.Count;

            IActivable[] activables = GetComponentsInChildren<IActivable>();
           
            foreach (IActivable activable in activables)
                activable.Activate();

            IDamageable[] damageables = GetComponentsInChildren<IDamageable>();
            foreach(IDamageable damageable in damageables)
            {
                damageable.OnDie += HandleOnDie;
            }
        }


        void ShipDestroy()
        {
            Destroy(gameObject);
        }

        void HandleOnDie(IDamageable damageable)
        {
            cannonCount--;
            if (cannonCount == 0)
                ShipDestroy(); 
            
            
            
        }
    }
}


