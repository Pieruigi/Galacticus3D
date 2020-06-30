using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB
{
    public class ProximityMine : Damager
    {

        [SerializeField]
        float damage;

        [SerializeField]
        float minRange;

        [SerializeField]
        float maxRange;


        [SerializeField]
        Weapon weapon;

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
            Fragment();

            
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Trigger enter");
            if (Tags.Player.Equals(other.tag))
            {
                //Collider[] colls = GetComponents<Collider>();
                //foreach (Collider coll in colls)
                //    coll.enabled = false;
                Explode();
                Fragment();

                
            }
                
        }

        void Explode()
        {
            Collider[] colls;
            colls = Physics.OverlapSphere(transform.position, MaxRange);

            if(colls != null)
            {

                for(int i=0; i<colls.Length; i++)
                {

                    //IDamageable damageable = colls[i].GetComponent(typeof(IDamageable))  as IDamageable;
                    IDamageable damageable = null;
                    List<Component> components = new List<Component>(colls[i].GetComponents(typeof(IDamageable)));
                    foreach(Component c in components)
                    {
                        if((c as MonoBehaviour).enabled)
                        {
                            damageable = c as IDamageable;
                            break;
                        }
                    }


                    if (damageable == null)
                        continue;

                    float distance = (colls[i].transform.position - transform.position).magnitude;
                    float damage = GetDamageAmountByRange(distance, MinRange, MaxRange);
                    damageable.ApplyDamage(damage);

                }
            }

            //GameObject.Destroy(gameObject, 1f);
        }

        void Fragment()
        {
            weapon.Fire();
        }

       
    }

}
