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

        // Start is called before the first frame update
        void Start()
        {

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
    }

}
