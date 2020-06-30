using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Level;

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

        //[SerializeField]
        //[Range(1u,7u)]
        //int minLevel = 1; // The enemy level
        //public int MinLevel
        //{
        //    get { return minLevel; }
        //}

        //[SerializeField]
        //[Range(1u, 7u)]
        //int maxLevel = 1; // The enemy level
        //public int MaxLevel
        //{
        //    get { return maxLevel; }
        //}

        [SerializeField]
        SpawnData spawnData;
        public SpawnData SpawnData
        {
            get { return spawnData; }
        }

        [SerializeField]
        Vector2 size = Vector3.one;
        public Vector2 Size
        {
            get { return size; }
        }

        [SerializeField]
        [Range(0f,1f)]
        float droppingRate;
        public float DroppingRate
        {
            get { return droppingRate; }
        }

        public static List<Enemy> GetResourcesForSpawning(int level)
        {
            List<Enemy> ret = new List<Enemy>();
            List<Enemy> temp = new List<Enemy>(Resources.LoadAll<Enemy>(Enemy.ResourceFolder)).FindAll(e => e.SpawnData.MinLevel <= level && e.SpawnData.MaxLevel >= level);

            foreach (Enemy e in temp)
            {
                for (int i = 0; i < (int)e.SpawnData.Rarity; i++)
                    ret.Add(e);
            }
            return ret;
        }
    }

}
