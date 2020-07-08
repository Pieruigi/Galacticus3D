using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Gameplay
{
    public class Bank : MonoBehaviour
    {
        GameObject player;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag(Tags.Player);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if(other == player)
            {
                // Deposit coins
                CoinManager.Instance.DepositAll();
            }
        }
    }

}
