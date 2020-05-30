using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public class Shield : MonoBehaviour, IDamageable
    {
        [SerializeField]
        float health;



        public event Die OnDie;

        Health playerHealth;
        GameObject graphics;

        public void ApplyDamage(float amount)
        {
            health -= amount;
            if (health <= 0)
            {
                playerHealth.enabled = true;
                if (health < 0)
                    playerHealth.ApplyDamage(-health);

                Destroy(this);
            }
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

        private void OnDestroy()
        {
            if (!playerHealth.enabled)
                playerHealth.enabled = true;

            Destroy(graphics);
        }

        public void Init(float health, GameObject prefab)
        {
            this.health = health;

            // Create graphic shield
            graphics = GameObject.Instantiate(prefab);
            graphics.transform.parent = transform;
            graphics.transform.localPosition = Vector3.zero;
            graphics.transform.localRotation = Quaternion.identity;
        }
    }

}
