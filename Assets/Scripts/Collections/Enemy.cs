using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Collections
{
    public class Enemy : ScriptableObject
    {
        public const string ResourceFolder = "Enemies";

        [SerializeField]
        GameObject prefabObject;
        public GameObject PrefabObject
        {
            get { return prefabObject; }
        }

        [SerializeField]
        string displayName;

        [SerializeField]
        [Range(1u,7u)]
        int level = 1; // The enemy level
        public int Level
        {
            get { return level; }
        }

        [SerializeField]
        Vector2 size = Vector3.one;
        public Vector2 Size
        {
            get { return size; }
        }
    }

}
