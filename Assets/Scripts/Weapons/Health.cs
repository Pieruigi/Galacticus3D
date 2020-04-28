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
