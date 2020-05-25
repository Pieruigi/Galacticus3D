using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Level
{
    public class RoomEnemyDataConfig
    {
        public int MinEnemyCount { get; set; }
        public int MaxEnemyCount { get; set; }
    }

    public class RoomEnemyData : MonoBehaviour
    {
        int minEnemyCount;
        public int MinEnemyCount
        {
            get { return minEnemyCount; }
        }
        int maxEnemyCount;
        public int MaxEnemyCount
        {
            get { return maxEnemyCount; }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(RoomEnemyDataConfig config)
        {
            minEnemyCount = config.MinEnemyCount;
            maxEnemyCount = config.MaxEnemyCount;
        }
    }

}
