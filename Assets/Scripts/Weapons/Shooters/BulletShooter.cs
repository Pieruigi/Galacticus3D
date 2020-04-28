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

        Transform owner;
        Weapon weapon;

        // Start is called before the first frame update
        void Start()
        {
            owner = transform.root;
            weapon = GetComponent<Weapon>();
        }

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
            bullet.GetComponent<Bullet>().Init(new BulletConfig()
            {
                Range = weapon.FireRange,
                Amount = weapon.DamageAmount,
                MaxRange = weapon.DamageMaxRange,
                MinRange = weapon.DamageMinRange

            });

            // No collision with the owner
            if (owner)
                Physics.IgnoreCollision(owner.GetComponent<Collider>(), bullet.GetComponent<Collider>());


        }
    }

}
