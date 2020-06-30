using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class CircleRandomMultiShooter : Shooter
    {
        [SerializeField]
        Shooter shooterTemplate;

        [SerializeField]
        int minShooters;

        [SerializeField]
        int maxShooters;

        List<Shooter> shooters = new List<Shooter>();

        protected override void Start()
        {
            base.Start();

            // Get weapon
            Weapon weapon = GetComponent<Weapon>();

            // Create random shooters
            int count = Random.Range(minShooters, maxShooters + 1);
            for(int i=0; i<count; i++)
            {
                Shooter shooter = GameObject.Instantiate(shooterTemplate);
                shooter.transform.parent = transform;
                shooter.transform.localPosition = Vector3.zero;
                shooter.transform.localRotation = Quaternion.identity;
                shooter.transform.localEulerAngles = Vector3.up * Random.Range(0f, 360f);
                shooter.Weapon = weapon;
                shooters.Add(shooter);
            }

            // Destroy template
            Destroy(shooterTemplate);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void Shoot()
        {
            // Shoot all shooters
            foreach (Shooter s in shooters)
                s.Shoot();
            

        }
    }

}
