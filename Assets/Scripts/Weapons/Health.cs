using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;
using UnityEngine.Events;

namespace OMTB
{
    public class Health : MonoBehaviour, IDamageable
    {
        public UnityAction OnHealthChanged;
        //public UnityAction OnDestroyed;

        [SerializeField]
        float maxHealth;
        public float MaxHealth
        {
            get { return maxHealth; }
        }

        [SerializeField]
        float health;

        public event Die OnDie;

        public float CurrentHealth
        {
            get { return health; }
        }

       
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

            if (health < 0)
                health = 0;

            OnHealthChanged?.Invoke();

            if(health == 0)
            {
                //OnDestroyed?.Invoke();
                OnDie?.Invoke(this);
                
                //Destroy(gameObject, 1);
            }
        }


        public void Recover(float amount)
        {
            health += amount;
            if (health > maxHealth)
                health = maxHealth;

            OnHealthChanged?.Invoke();
        }

        public bool IsDead()
        {
            return health == 0;
        }
    }

}
