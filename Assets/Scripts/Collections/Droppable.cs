using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Collections
{

    /**
     * Common is for repair kit and coins.
     * Rare is for level 1 power up
     * Epic is for level 2 power up
     * Legendary is for level 3 power up
     * Unique is for level 4 power up
     * */
    public enum DroppableRarity { Common = 1000, Rare = 100, Epic = 50, Legendary = 25, Unique = 10 }

    public class Droppable : ScriptableObject
    {
        public const string ResourceFolder = "Droppables";

        public enum SpecialEnemyGroupType { None, Millenium }

        [SerializeField]
        GameObject prefab;
        public GameObject Prefab
        {
            get { return prefab; }
        }

        [SerializeField]
        DroppableRarity rarity = DroppableRarity.Common;
        public DroppableRarity Rarity
        {
            get { return rarity; }
        }
        
        //public static DroppableRarity GetRarityByLevel(int level)
        //{
        //    // Level 1 only commons powerups
        //    if (level < 1)
        //        return DroppableRarity.Common;


        //    if (level < 2)
        //        return DroppableRarity.Epic;

        //    if (level < 3)
        //        return DroppableRarity.Legendary;

        //    return DroppableRarity.Unique;
        //}
    }

}
