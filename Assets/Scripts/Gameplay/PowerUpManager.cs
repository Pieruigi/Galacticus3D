using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collections;


namespace OMTB.Gameplay
{

    public class PowerUpManager : MonoBehaviour
    {
        public static PowerUpManager Instance { get; private set; }

        GameObject player;

        PowerUp current;

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

            Debug.Log("PP:" + player.GetComponent<Health>());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool TryActivate(PowerUp powerUp)
        {
            
            if(current != null)
                ReleaseCurrentPowerUp();

            current = powerUp;

            // Avoid the power up to get destroyed with the picker
            current.transform.parent = transform;

            powerUp.Activate();

            return true;
        }

        void ReleaseCurrentPowerUp()
        {
            Debug.Log("TO-DO: releasing power up");


        }
    }

}
