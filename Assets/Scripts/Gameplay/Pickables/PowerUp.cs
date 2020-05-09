using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public abstract class PowerUp : MonoBehaviour, IPickable
    {
        public virtual bool TryPickUp(GameObject player)
        {
            throw new System.NotImplementedException();
        }
    }

}
