using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Level;

namespace OMTB.Collections
{
    public class Boss : ScriptableObject
    {
        public const string ResourceFolder = "Bosses";

        [SerializeField]
        GameObject prefabObject;
        public GameObject PrefabObject
        {
            get { return prefabObject; }
        }

        [SerializeField]
        string displayName;

        // The enemy average level ( level manager may range from L-1 and L+1 )
        [SerializeField]
        SpawnData spawnData;
        public SpawnData SpawnData
        {
            get { return spawnData; }
        }

        [SerializeField]
        List<Room> rooms;
        public IList<Room> Rooms
        {
            get { return rooms.AsReadOnly(); }
        }

        public static List<Boss> GetResourcesForSpawning(int level)
        {
            List<Boss> ret = new List<Boss>();
            List<Boss> temp = new List<Boss>(Resources.LoadAll<Boss>(ResourceFolder)).FindAll(e => e.SpawnData.MinLevel <= level && e.SpawnData.MaxLevel >= level);

            foreach (Boss e in temp)
            {
                for (int i = 0; i < (int)e.SpawnData.Rarity; i++)
                    ret.Add(e);
            }
            return ret;
        }
    }

}
