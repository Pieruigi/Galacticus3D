using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB
{
    public class Destroyer : MonoBehaviour
    {

        [SerializeField]
        float delay = 0.1f;

        

        // Start is called before the first frame update
        void Start()
        {
            IDamageable idam = GetComponent<IDamageable>();
            if (idam != null)
                idam.OnDie += HandleOnDie;
        }

        // Update is called once per frame
        void Update()
        {
           
        }

        void HandleOnDie(IDamageable damageable)
        {
           

            // Disable collider
            Collider coll = GetComponent<Collider>();
            if (coll)
                coll.enabled = false;

            // Disable behaviours
            //IActivable[] activables = GetComponentsInChildren<IActivable>();
            //foreach (IActivable activable in activables)
            //    activable.Deactivate();

           
            Destroy(gameObject, delay);
        }
    }

}
