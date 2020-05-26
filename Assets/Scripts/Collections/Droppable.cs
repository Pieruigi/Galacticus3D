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

      
 

    }

}
