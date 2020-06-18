using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    
    public class Deflector : PowerUp
    {
        [SerializeField]
        [Tooltip("0 means no damage is deflected, 1 means all damage is deflected.")]
        [Range(0f,1f)]
        float deflectedDamage;
        public float DeflectedDamage
        {
            get { return deflectedDamage; }
        }

        [SerializeField]
        GameObject prefab;
        public GameObject Prefab
        {
            get { return prefab; }
        }

               

        public override void Activate()
        {
            CreateController(typeof(DeflectorController));
        }

        public override void Deactivate()
        {
            DestroyController();
        }
    }

}
