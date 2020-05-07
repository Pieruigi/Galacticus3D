using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class BulletShooter : Shooter
    {
        [SerializeField]
        GameObject bulletPrefab;

        [SerializeField]
        Transform firePoint;

             


        // Update is called once per frame
        void Update()
        {

        }

        public override void Shoot()
        {
            // Create bullet
            GameObject bullet = GameObject.Instantiate(bulletPrefab);
            bullet.transform.position = firePoint.position;
            bullet.transform.eulerAngles = firePoint.eulerAngles;
            bullet.GetComponent<Damager>().Init(new BulletConfig()
            {
                Range = Weapon.FireRange,
                Amount = Weapon.DamageAmount,
                MaxRange = Weapon.DamageMaxRange,
                MinRange = Weapon.DamageMinRange

            });

            // No collision with the owner
            if (Owner)
                Physics.IgnoreCollision(Owner.GetComponent<Collider>(), bullet.GetComponent<Collider>());


        }
    }

}
