using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        float speed;

        [SerializeField]
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

        private void OnCollisionEnter(Collision collision)
        {
            GameObject.Destroy(gameObject);
        }
    }

}
