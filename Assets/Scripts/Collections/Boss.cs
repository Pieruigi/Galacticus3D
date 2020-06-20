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
        [Range(1u,7u)]
        int level = 1; 
        public int Level
        {
            get { return level; }
        }

        [SerializeField]
        List<Room> rooms;
        public IList<Room> Rooms
        {
            get { return rooms.AsReadOnly(); }
        }

    }

}
