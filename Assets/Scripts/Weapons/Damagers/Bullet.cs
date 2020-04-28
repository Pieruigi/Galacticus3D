using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB
{
    public class Bullet : Damager
    {
        float speed = 50;

        float range;

     
        Vector3 startPosition;
        float rangeSqr;

        // Start is called before the first frame update
        void Start()
        {
            rangeSqr = range * range;
            startPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position += transform.forward * speed * Time.deltaTime;

            if ((transform.position - startPosition).sqrMagnitude > rangeSqr)
                GameObject.Destroy(gameObject);
        }

        public override void Init(DamagerConfig config)
        {
            base.Init(config);
            
            BulletConfig bulletConfig = config as BulletConfig;
            range = bulletConfig.Range;

        }

        void OnCollisionEnter(Collision collision)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.ApplyDamage(Amount);
            }

            GameObject.Destroy(gameObject);
        }
    }

}
