using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Level
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField]
        Transform spawnPoint;
        public Transform SpawnPoint
        {
            get { return spawnPoint; }
        }

        // Start is called before the first frame update
        void Start()
        {
            LevelManager.Instance.Player.transform.position = spawnPoint.position;
            LevelManager.Instance.Player.transform.rotation = spawnPoint.rotation;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
