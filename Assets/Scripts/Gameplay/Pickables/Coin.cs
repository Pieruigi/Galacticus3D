using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public class Coin : MonoBehaviour, IPickable
    {
        [SerializeField]
        int amount;

        public bool TryPickUp()
        {
            Debug.Log("You got " + amount + " coin(s).");
            CoinManager.Instance.PickUpCoins(amount);
            return true;
        }


    }

}
