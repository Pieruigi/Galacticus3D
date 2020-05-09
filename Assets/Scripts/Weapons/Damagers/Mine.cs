using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB
{
    public class Mine : Damager
    {

        [SerializeField]
        float damage;

        [SerializeField]
        float minRange;

        [SerializeField]
        float maxRange;

        Transform target;

        // Start is called before the first frame update
        void Start()
        {
            Init(new DamagerConfig() { Amount = damage, MaxRange = maxRange, MinRange = minRange, Owner = transform });
        }

        // Update is called once per frame
        void Update()
        {
        }

        public override void Init(DamagerConfig config)
        {
            base.Init(config);
            
            BulletConfig bulletConfig = config as BulletConfig;
            
            // Setting rocket target
            //TargetSetter targetSetter = Owner.GetComponent<TargetSetter>();
            //target = targetSetter.Target;

        }

        void OnCollisionEnter(Collision collision)
        {
            
            Explode();
            
        }

        void Explode()
        {
            Collider[] colls;
            colls = Physics.OverlapSphere(transform.position, MaxRange);

            if(colls != null)
            {

                for(int i=0; i<colls.Length; i++)
                {
                 
                    IDamageable damageable = colls[i].GetComponent<IDamageable>();
                    if (damageable == null)
                        continue;

                    float distance = (colls[i].transform.position - transform.position).magnitude;
                    float damage = GetDamageAmountByRange(distance, MinRange, MaxRange);
                    damageable.ApplyDamage(damage);

                }
            }

            GameObject.Destroy(gameObject);
        }
    }

}
