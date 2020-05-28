using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public class ShieldData : PowerUpData
    {
        public float DamageAmount { get; private set; }

        public ShieldData(float damageAmount, System.Type powerUpType): base(powerUpType)
        {
            DamageAmount = damageAmount;
        }
    }

    public class Shield : PowerUp
    {
        [SerializeField]
        float damage;

        public override PowerUpData GetData()
        {
            return new ShieldData(damage, GetType());
        }

        public override void PickUpAndActivate()
        {
            throw new System.NotImplementedException();
        }

        public override bool TryPickUp(GameObject player)
        {
            return base.TryPickUp(player);
        }
    }

}
