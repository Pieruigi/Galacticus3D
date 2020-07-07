using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OMTB
{
    public class CoinManager : MonoBehaviour
    {
        // Player bag balance ( not yet entrusted in bank )
        int bagBalance;

        // Bank account balance
        int bankBalance;

        public static CoinManager Instance { get; private set; }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        /**
         * Move coins from bag to bank
         * */
        public void DepositAll()
        {
            bankBalance += bagBalance;
            bagBalance = 0;
        }

        /**
         * Tries to withdraw some coins from the bank account and returns true if the operation succeed, otherwise returns false and
         * no coins are withdrawn.
         * */
        public bool Withdraw(int amount)
        {
            if (amount > bankBalance)
                return false;

            bankBalance -= amount;
            return true;
        }

        /**
         * Increases the player bag balance by a given amount of coins
         * */
        public void PickUpCoins(int amount)
        {
            bagBalance += amount;
        }
    }

}
