using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{


    public class DeflectorPowerUp : PowerUp
    {
        [SerializeField]
        [Tooltip("Percentage (%) of deflected damage.")]
        float deflectedDamage;
        public float DeflectedDamage
        {
            get { return deflectedDamage; }
        }

        [SerializeField]
        GameObject prefab;

        Deflector shield;

        

        public override void Activate()
        {

            // Get the player health
            //playerHealth = GameManager.Instance.Player.GetComponent<Health>();
            //playerHealth.enabled = false;

            if (Level.LevelManager.Instance == null)
                shield = GameObject.FindGameObjectWithTag("Player").AddComponent<Deflector>();
            else
                shield = Level.LevelManager.Instance.Player.AddComponent<Deflector>();

            
            shield.Init(deflectedDamage, prefab);

            
        }

        public override void Deactivate()
        {

            shield.Deactivate();
        }
    }

}
