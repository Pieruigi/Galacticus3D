using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public class RepairKit : MonoBehaviour, IPickable
    {
        [SerializeField]
        float amount;

        public bool TryPickUp(GameObject player)
        {
            //return true;
            Health health = player.GetComponent<Health>();

            if (health.CurrentHealth == health.MaxHealth)
                return false;


            health.Recover(amount);
            return true;
            
        }


    }

}
