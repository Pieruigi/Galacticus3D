using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Collections
{
    public class Droppable : ScriptableObject
    {
        public enum Type { RepairKit, Coin }



        [SerializeField]
        GameObject prefab;

        [SerializeField]
        [Range(0f,1f)]
        float dropChance;
        public float DropChance
        {
            get { return dropChance; }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static Droppable Create()
        {
            return ScriptableObject.CreateInstance<Droppable>();
        }
    }

}
