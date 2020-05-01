using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class LevelManager : MonoBehaviour
    {
        int level = 1;
        public int Level
        {
            get { return level; }
        }

        public static LevelManager Instance { get; private set; }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            //CollectionManager.Create();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
