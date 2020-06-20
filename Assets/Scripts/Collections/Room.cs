using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Level;

namespace OMTB.Collections
{
    public enum RoomType { Starting, Exploration, Fighting, Boss, Bank }

    public class Room : ScriptableObject
    {
        public const string ResourceFolder = "Rooms";

        [SerializeField]
        RoomType roomType;
        public RoomType RoomType
        {
            get { return roomType; }
        }

        [SerializeField]
        [Tooltip("Leave null if you don't have any template for this room")]
        GameObject prefabObject;
        public GameObject PrefabObject
        {
            get { return prefabObject; }
        }

        

    }

}
