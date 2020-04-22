using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OMTB
{
    public class Weapon : MonoBehaviour
    {


        [SerializeField]
        Damage damage;

        [SerializeField]
        float range;

        [SerializeField]
        float rate;

        [SerializeField]
        Shooter shooter;

        DateTime lastFire;

        float fireTime;

        private void Awake()
        {
            fireTime = 1f / rate;
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
    }

}
