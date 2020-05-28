using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OMTB.Gameplay
{

    public class PowerUpManager : MonoBehaviour
    {
        public static PowerUpManager Instance { get; private set; }

        GameObject player;

        System.Type current;

        [SerializeField]
        PowerUp test;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;

                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            PowerUpData data = test.GetData();
            Debug.Log("PUType:" + data.PowerUpType);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool TryActivate(PowerUp powerUp)
        {
            current = powerUp.GetType();
            powerUp.PickUpAndActivate();
            return true;
        }
    }

}
