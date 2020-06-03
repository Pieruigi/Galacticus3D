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

        Transform target;

        float angularSpeed = 60;

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
                Explode();


            // Get target by the Owner property
            Quaternion targetRot = Quaternion.LookRotation((target.position - transform.position).normalized);
            Quaternion rot = Quaternion.RotateTowards(transform.rotation, targetRot, angularSpeed * Time.deltaTime);
            transform.rotation = rot;

        }

        public override void Init(DamagerConfig config)
        {
            base.Init(config);
            
            BulletConfig bulletConfig = config as BulletConfig;
            range = bulletConfig.Range;
           
            // Setting rocket target
            TargetSetter targetSetter = Owner.GetComponent<TargetSetter>();
            target = targetSetter.Target;

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

                    IDamageable damageable = null;
                    List<Component> components = new List<Component>(colls[i].GetComponents(typeof(IDamageable)));
                    foreach (Component c in components)
                    {
                        Debug.Log("Dam-Comp:" + c);
                        if ((c as MonoBehaviour).enabled)
                        {
                            damageable = c as IDamageable;
                            break;
                        }
                    }

                    Debug.Log("Dam:" + damageable);
                    //IDamageable damageable = colls[i].GetComponent<IDamageable>();
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
