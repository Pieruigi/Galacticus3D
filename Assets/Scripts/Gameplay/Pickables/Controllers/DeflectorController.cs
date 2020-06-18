using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public class DeflectorController : PowerUpController, IDamageable
    {
        Deflector deflector;

        public event Die OnDie;

        Health playerHealth;
        GameObject graphics;

        public void ApplyDamage(float amount)
        {
            // Compute damage
            amount -= amount * deflector.DeflectedDamage;

            playerHealth.ApplyDamage(amount);

        }

        protected override void Awake()
        {
            playerHealth = GetComponent<Health>();
            playerHealth.enabled = false;
        }



        public override void Init(PowerUp powerUp)
        {
            this.deflector = powerUp as Deflector;
            graphics = GameObject.Instantiate(this.deflector.Prefab);
            graphics.transform.parent = transform;
            graphics.transform.localPosition = Vector3.zero;
            graphics.transform.localRotation = Quaternion.identity;
        }

        private void OnDestroy()
        {
            if (!playerHealth.enabled)
                playerHealth.enabled = true;

            Destroy(graphics);
        }

    }

}
