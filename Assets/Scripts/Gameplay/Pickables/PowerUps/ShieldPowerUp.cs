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


    
        Shield shield;

        

        public override void Activate()
        {


            // Get the player health
            //playerHealth = player.GetComponent<Health>();
            //playerHealth.enabled = false;

            if (Level.LevelManager.Instance == null)
                shield = GameObject.FindGameObjectWithTag("Player").AddComponent<Shield>();
            else
                shield = Level.LevelManager.Instance.Player.AddComponent<Shield>();

            shield.Init(damage, prefab);

            
        }

        public override void Deactivate()
        {
            shield.Deactivate();
        }
    }

}
