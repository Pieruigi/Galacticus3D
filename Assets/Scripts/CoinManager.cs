using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;
using UnityEngine.Events;

namespace OMTB
{
    public class CoinManager : MonoBehaviour
    {
        public UnityAction<int, int> OnChange;

        // Player bag balance ( not yet entrusted in bank )
        int bagBalance;

        // Bank account balance
        int bankBalance;

        public static CoinManager Instance { get; private set; }

        GameObject player;

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
            GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<IDamageable>().OnDie += HandleOnDie;
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

            OnChange?.Invoke(bagBalance, bankBalance);
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

            OnChange?.Invoke(bagBalance, bankBalance);

            return true;
        }

        /**
         * Increases the player bag balance by a given amount of coins
         * */
        public void PickUpCoins(int amount)
        {
            bagBalance += amount;

            OnChange?.Invoke(bagBalance, bankBalance);
        }

        void HandleOnDie(IDamageable damageable)
        {
            bagBalance = 0;

            OnChange?.Invoke(bagBalance, bankBalance);
        }
    }

}
