using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class ShipFXController : MonoBehaviour
    {
        [SerializeField]
        List<ParticleSystem> hitPsList;
        
        [SerializeField]
        ParticleSystem diePS;

        [SerializeField]
        Transform point;

        [SerializeField]
        List<ParticleSystem> stopOnDieList;

        // Start is called before the first frame update
        void Start()
        {
            GetComponent<OMTB.Interfaces.IDamageable>().OnDie += HandleOnDie;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            if("Bullet".Equals(collision.gameObject.tag))
            {
                foreach(ParticleSystem hitPs in hitPsList)
                {
                    ParticleSystem ps = GameObject.Instantiate(hitPs);
                    ps.transform.position = point.position;
                    //ps.transform.rotation = Quaternion.identity;
                    ps.Play();
                }
                
            }
        }

        void HandleOnDie(OMTB.Interfaces.IDamageable damageable)
        {
            foreach (ParticleSystem p in stopOnDieList)
                p.Stop();

            ParticleSystem ps = GameObject.Instantiate(diePS);
            ps.transform.position = point.position;
            //ps.transform.rotation = Quaternion.identity;
            ps.Play();
        }
    }

}
