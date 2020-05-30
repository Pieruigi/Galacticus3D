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

        GameObject player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        public bool TryPickUp()
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
