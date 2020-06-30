using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class GameManager : MonoBehaviour
    {


        public static GameManager Instance { get; private set; }

        bool inGame = false;

        PlayerController playerController;

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
            inGame = true;
            if (inGame)
            {
                GameObject.FindObjectOfType<CameraFadeController>().FadeIn();
                playerController = Level.LevelManager.Instance.Player.GetComponent<PlayerController>();
            }
                
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (playerController.IsGamepadConnected)
                {
                    playerController.IsGamepadConnected = false;
                    playerController.MouseHasGamepadBehavior = true;
                }
                else
                {
                    if (playerController.MouseHasGamepadBehavior)
                        playerController.MouseHasGamepadBehavior = false;
                    else
                        playerController.IsGamepadConnected = true;
                }
            }
        }
    }

}
