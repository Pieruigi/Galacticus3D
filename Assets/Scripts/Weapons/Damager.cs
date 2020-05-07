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

        public virtual void Init(DamagerConfig config)
        {
            amount = config.Amount;
            minRange = config.MinRange;
            maxRange = config.MaxRange;
        }

        

        /**
         * Use this method to compute damage for damageable not directly hit by the damager when min anx max range are different than zero.
         * */
        protected float GetDoubleRangeInterpolationMultiplier(float distance, float internalRange, float externalRange, bool decreasing)
        {

            if (externalRange <= 0 || distance <= internalRange)
                return (!decreasing ? 0 : 1);

            if (distance > externalRange)
                return (!decreasing ? 1 : 0);


            if (externalRange < internalRange)
                externalRange = internalRange;

            float mul = 0;

            if (externalRange > 0)
            {
                if (distance > internalRange && distance < externalRange)
                    mul = (externalRange - distance + internalRange) / (externalRange - internalRange);
            }

            return (!decreasing ? mul : 1 - mul);
        }
    }

}
