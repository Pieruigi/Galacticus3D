using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Collections
{
    public class Droppable : ScriptableObject
    {
        public const string ResourceFolder = "Droppables";

        public enum SpecialEnemyGroupType { None, Millenium }

        [Header("Drop Section")]
        [SerializeField]
        GameObject pickerPrefab;
        public GameObject PickerPrefab
        {
            get { return pickerPrefab; }
        }

        [SerializeField]
        [Range(0f,1f)]
        float dropChance;
        public float DropChance
        {
            get { return dropChance; }
        }

        [SerializeField]
        int minLevel;
        public int MinLevel
        {
            get { return minLevel; }
        }

        [SerializeField]
        int maxLevel = -1;
        public int MaxLevel
        {
            get { return maxLevel; }
        }

        [SerializeField]
        bool byScene;
        public bool ByScene
        {
            get { return byScene; }
        }

        [SerializeField]
        bool bySpaceStation;
        public bool BySpaceStation
        {
            get { return bySpaceStation; }
        }

        //[SerializeField]
        //bool byCommonEnemies;
        //public bool ByCommonEnemies
        //{
        //    get { return byCommonEnemies; }
        //}

        [SerializeField]
        SpecialEnemyGroupType specialEnemyGroup;
        public SpecialEnemyGroupType SpecialEnemyGroup
        {
            get { return specialEnemyGroup; }
        }

         

    }

}
