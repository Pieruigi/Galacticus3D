using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

       

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
           
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
