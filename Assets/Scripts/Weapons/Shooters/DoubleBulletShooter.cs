using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class DoubleBulletShooter : Shooter
    {
        [SerializeField]
        Shooter leftShooter;

        [SerializeField]
        Shooter rightShooter;

        bool nextIsRight = false;


        protected override void Start()
        {
            base.Start();
            leftShooter.Weapon = GetComponent<Weapon>();
            rightShooter.Weapon = GetComponent<Weapon>();
        }

        public override void Shoot()
        {
            if (nextIsRight)
                rightShooter.Shoot();
            else
                leftShooter.Shoot();

            nextIsRight = !nextIsRight;
        }
    }

}
