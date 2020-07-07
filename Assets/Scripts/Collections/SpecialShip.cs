using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Level;

namespace OMTB.Collections
{
    public enum SpecialShipType { IntergalacticBank, ChargeStation, Galacticus }

    public class SpecialShip : ScriptableObject
    {
        public const string ResourceFolder = "SpecialShips";

        [SerializeField]
        SpecialShipType type;
        public SpecialShipType Type
        {
            get { return type; }
        }

        [SerializeField]
        GameObject prefabObject;
        public GameObject PrefabObject
        {
            get { return prefabObject; }
        }

        [SerializeField]
        string displayName;

        [SerializeField]
        SpawnData spawnData;
        public SpawnData SpawnData
        {
            get { return spawnData; }
        }

        //[SerializeField]
        //[Range(0f,1f)]
        //float spawnRate;
        //public float SpawnRate
        //{
        //    get { return spawnRate; }
        //}


        [SerializeField]
        List<Room> rooms;
        public IList<Room> Rooms
        {
            get { return rooms.AsReadOnly(); }
        }

        public static List<SpecialShip> GetAvailableResources(int level)
        {
            List<SpecialShip> tmp = new List<SpecialShip>(Resources.LoadAll<SpecialShip>(ResourceFolder)).FindAll(e => e.SpawnData.MinLevel <= level && e.SpawnData.MaxLevel >= level);
            Debug.Log("SpecialShips:" + tmp.Count);
            List<SpecialShip> ret = new List<SpecialShip>();
            foreach(SpecialShip ship in tmp)
            {
                float r = Random.Range(0, (float)Rarity.Common * 1.3f);
                if ((float)ship.spawnData.Rarity >= r)
                    ret.Add(ship);
            }
            Debug.Log("SpecialShips->Ret:" + tmp.Count);
            return ret;
            
        }
    }

}
