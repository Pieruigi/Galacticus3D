using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{


    public class SpeedPowerUp : PowerUp
    {
        [SerializeField]
        float speedMultiplier;
        public float SpeedMultiplier
        {
            get { return speedMultiplier; }
        }

        [SerializeField]
        GameObject prefab;

        GameObject player;

        public override void Activate()
        {

            
            if (Level.LevelManager.Instance == null)
                player = GameObject.FindGameObjectWithTag("Player");
            else
                player = Level.LevelManager.Instance.Player;

            float maxSpeed = player.GetComponent<PlayerController>().MaxSpeed;
            maxSpeed *= speedMultiplier;
            player.GetComponent<PlayerController>().MaxSpeed = maxSpeed;
        }

        public override void Deactivate()
        {
            player.GetComponent<PlayerController>().ResetMaxSpeed();
        }
    }

}
