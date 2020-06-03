using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OMTB
{
    public class Weapon : MonoBehaviour
    {

        [Header("Weapon Stats")]
        [SerializeField]
        float fireRange;
        public float FireRange
        {
            get { return fireRange; }
        }

        [SerializeField]
        float fireRate;

        [Header("Damage")]
        [SerializeField]
        float damageAmount;
        public float DamageAmount
        {
            get { return damageAmount; }
        }

        [SerializeField]
        float damageMinRange;
        public float DamageMinRange
        {
            get { return damageMinRange; }
        }

        [SerializeField]
        float damageMaxRange;
        public float DamageMaxRange
        {
            get { return damageMaxRange; }
        }

        [Header("Components")]
        [SerializeField]
        Shooter shooter;

        DateTime lastFire;

        float fireTime;

        private void Awake()
        {
            fireTime = 1f / fireRate;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool Fire()
        {
            if ((DateTime.UtcNow - lastFire).TotalSeconds < fireTime)
                return false;

            lastFire = DateTime.UtcNow;
            shooter.Shoot();


            return true;
        }

        public void SetDelay(float delay)
        {
            lastFire = DateTime.UtcNow.AddSeconds(-delay);
        }
    }

}
