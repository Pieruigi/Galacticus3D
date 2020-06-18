using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public class ShieldController : PowerUpController, IDamageable
    {
        Shield powerUp;
        
        public event Die OnDie;

        Health playerHealth;
        GameObject graphics;

        protected override void Awake()
        {
            base.Awake();
            playerHealth = GetComponent<Health>();
            playerHealth.enabled = false;
        }


        public void ApplyDamage(float amount)
        {
            powerUp.Damage -= amount;
            if (powerUp.Damage <= 0)
            {
                playerHealth.enabled = true;
                if (powerUp.Damage < 0)
                {
                    playerHealth.ApplyDamage(-powerUp.Damage);
                    powerUp.Damage = 0;
                }

                Destroy(this);
            }
        }

  
        private void OnDestroy()
        {
            if (!playerHealth.enabled)
                playerHealth.enabled = true;

            // Update power up reference in the powerup manager
            Destroy(graphics);
        }

       
        public override void Init(PowerUp powerUp)
        {

            this.powerUp = powerUp as Shield;
            graphics = GameObject.Instantiate(this.powerUp.Prefab);
            graphics.transform.parent = transform;
            graphics.transform.localPosition = Vector3.zero;
            graphics.transform.localRotation = Quaternion.identity;
        }
    }

}
