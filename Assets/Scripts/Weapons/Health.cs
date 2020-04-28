using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;
using UnityEngine.Events;

namespace OMTB
{
    public class Health : MonoBehaviour, IDamageable
    {
        
        [SerializeField]
        float maxHealth;

        [SerializeField]
        float health;

       
        void Awake()
        {
            if (health > maxHealth)
                health = maxHealth;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void ApplyDamage(float amount)
        {
            health -= amount;
            if(health < 0)
            {
                Destroy(gameObject);
            }
        }

    }

}
