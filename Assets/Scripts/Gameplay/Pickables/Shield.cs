using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public class Shield : PowerUp
    {
        public override bool TryPickUp(GameObject player)
        {
            return base.TryPickUp(player);
        }
    }

}
