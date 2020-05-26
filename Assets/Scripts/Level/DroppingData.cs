using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collections;

namespace OMTB.Level
{
    [System.Serializable]
    public class DroppingData
    {
        [SerializeField]
        GameObject droppable;
        public GameObject Droppable
        {
            get { return droppable; }
        }

        [SerializeField]
        int weight;
        public int Weight
        {
            get { return weight; }
        }
    }

}
