using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{


    public class ShieldPowerUp : PowerUp
    {
        [SerializeField]
        float damage;
        public float Damage
        {
            get { return damage; }
        }

        [SerializeField]
        GameObject prefab;


        Health playerHealth;
        Shield shield;

        

        public override void Activate()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Get the player health
            playerHealth = player.GetComponent<Health>();
            playerHealth.enabled = false;

            shield = player.AddComponent<Shield>();
            shield.Init(damage, prefab);

            
        }

        
    }

}
