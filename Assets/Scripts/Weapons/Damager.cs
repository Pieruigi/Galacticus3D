using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB
{
    public abstract class Damager: MonoBehaviour
    {
        private float amount;
        public float Amount
        {
            get { return amount; }
        }

        private float minRange = 0; // Zero to deliver damage by contact ( ex. bullet ); use to deliver the whole damage to the enemies in range ( for explosion )
        public float MinRange
        {
            get { return minRange; }
        }

        private float maxRange;  // Part of the damage will be applied to all the enemies in range
        public float MaxRange
        {
            get { return maxRange; }
        }

        private Transform owner;
        public Transform Owner
        {
            get { return owner; }
        }

        public virtual void Init(DamagerConfig config)
        {
            amount = config.Amount;
            minRange = config.MinRange;
            maxRange = config.MaxRange;
            owner = config.Owner;
        }

        

        /**
         * Use this method to compute damage for damageable not directly hit by the damager when min anx max range are different than zero.
         * */
        protected float GetDamageAmountByRange(float distance, float internalRange, float externalRange)
        {

            if (distance <= internalRange)
                return amount;

            if (externalRange > 0 && distance > externalRange)
                return 0;

            float mul = 0;


            if (distance > internalRange && distance < externalRange)
                mul = ((externalRange - distance) / (externalRange - internalRange));


            return mul*amount;
        }
    }

}
