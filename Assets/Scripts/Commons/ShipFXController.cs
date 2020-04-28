using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class ShipFXController : MonoBehaviour
    {
        [SerializeField]
        ParticleSystem hitPs;
        
        [SerializeField]
        ParticleSystem diePS;

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
                ParticleSystem ps = GameObject.Instantiate(hitPs);
                ps.transform.position = transform.position;
                ps.transform.rotation = Quaternion.identity;
                ps.Play();
            }
        }
    }

}
