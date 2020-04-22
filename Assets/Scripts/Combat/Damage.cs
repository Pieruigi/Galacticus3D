using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    [System.Serializable]
    public class Damage
    {
        [SerializeField]
        private float amount;

        [SerializeField]
        private float rangeMin = 0; // Zero to deliver damage by contact ( ex. bullet ); use to deliver the whole damage to the enemies in range ( for explosion )

        [SerializeField]
        private float rangeMax;  // Part of the damage will be applied to all the enemies in range
    }

}
