using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public class Deflector : MonoBehaviour, IDamageable
    {
        float deflectedDamage;

        public event Die OnDie;

        Health playerHealth;
        GameObject graphics;

        public void ApplyDamage(float amount)
        {
            // Compute damage
            amount -= amount * deflectedDamage;

            playerHealth.ApplyDamage(amount);

        }

        void Awake()
        {
            playerHealth = GetComponent<Health>();
            playerHealth.enabled = false;

            
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        

        public void Init(float deflectedDamage, GameObject prefab)
        {
            this.deflectedDamage = deflectedDamage;

            // Create graphic shield
            graphics = GameObject.Instantiate(prefab);
            graphics.transform.parent = transform;
            graphics.transform.localPosition = Vector3.zero;
            graphics.transform.localRotation = Quaternion.identity;
        }

        public void Deactivate()
        {
            if (!playerHealth.enabled)
                playerHealth.enabled = true;

            Destroy(graphics);

            Destroy(this);
        }
    }

}
