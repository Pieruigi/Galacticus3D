using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB
{
    public class Rocket : Damager
    {
        float speed = 20;

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

            //if ((transform.position - startPosition).sqrMagnitude > rangeSqr)
            
            
            // Get target by the Owner property
            if((transform.position - GameObject.FindGameObjectWithTag("Player").transform.position).magnitude < 7)
                Explode();
        }

        public override void Init(DamagerConfig config)
        {
            base.Init(config);
            
            BulletConfig bulletConfig = config as BulletConfig;
            range = bulletConfig.Range;
            Debug.Log("Range:" + range);
        }

        void OnCollisionEnter(Collision collision)
        {
            IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Explode();
            }

            
        }

        void Explode()
        {
            Collider[] colls;
            colls = Physics.OverlapSphere(transform.position, MaxRange);
            Debug.Log("colls.length:" + colls.Length);
            if(colls != null)
            {

                for(int i=0; i<colls.Length; i++)
                {
                    Debug.Log("colls["+i+"]:" + colls[i].name);
                    IDamageable damageable = colls[i].GetComponent<IDamageable>();
                    if (damageable == null)
                        continue;

                    float distance = (colls[i].transform.position - transform.position).magnitude;
                    float mul = GetDoubleRangeInterpolationMultiplier(distance, MinRange, MaxRange, true);
                    Debug.Log(colls[i].name + " - mul:" + mul);
                    damageable.ApplyDamage(Amount * mul);

                }
            }

            GameObject.Destroy(gameObject);
        }
    }

}
