using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public class Invincibility : PowerUp
    {
        public override PowerUpData GetData()
        {
            throw new System.NotImplementedException();
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
