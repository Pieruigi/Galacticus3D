using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public abstract class PowerUpData
    {
        public System.Type PowerUpType { get; private set; }

        public PowerUpData(System.Type powerUpType)
        {
            PowerUpType = powerUpType;
        }
    }

    public abstract class PowerUp : MonoBehaviour, IPickable
    {
        public virtual bool TryPickUp(GameObject player)
        {
            return PowerUpManager.Instance.TryActivate(this);
        }

        public abstract void PickUpAndActivate();

        public abstract PowerUpData GetData();
    }

}
