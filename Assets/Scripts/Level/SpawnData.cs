using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Level
{
    public enum Rarity { Common = 1000, NotCommon = 500, Rare = 100, Epic = 50, Legendary = 25, Unique = 10 }

    [System.Serializable]
    public class SpawnData
    {
       
        [SerializeField]
        [Range(1, LevelManager.NumberOfLevels)]
        int minLevel = 1;
        public int MinLevel
        {
            get { return minLevel; }
        }

        [SerializeField]
        [Range(1, LevelManager.NumberOfLevels)]
        int maxLevel = 1;
        public int MaxLevel
        {
            get { return (minLevel>maxLevel ? minLevel : maxLevel); }
        }

        [SerializeField]
        Rarity rarity = Rarity.Common;
        public Rarity Rarity
        {
            get { return rarity; }
        }


        
    }

}
