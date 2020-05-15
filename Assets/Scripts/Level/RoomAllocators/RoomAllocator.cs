using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public abstract class RoomAllocator : MonoBehaviour
    {
        Room room;
        public Room Room
        {
            get { return room; }
            set { room = value; }
        }

        GameObject wallRoot;
        public GameObject WallRoot
        {
            get { return wallRoot; }
        } 

        public abstract void Allocate();

        protected virtual void Awake()
        {
            // Create wall root and add the optimizer script
            wallRoot = new GameObject("Walls");
            wallRoot.transform.parent = transform;
            wallRoot.transform.localPosition = Vector3.zero;
            wallRoot.transform.localRotation = Quaternion.identity;
            wallRoot.AddComponent<Optimizer>();

        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
            AllocateBorders();
            Allocate();
        }

        protected virtual void AllocateBorders()
        {
            List<GameObject> borders = LoadBorderResources();
            if (borders.Count == 0)
                return;
            GameObject g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
            g.transform.parent = wallRoot.transform;
            g.transform.localPosition = ( Vector3.left*0.5f + Vector3.forward * 0.5f ) * Room.TileSize;
            g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
            g.transform.parent = wallRoot.transform;
            g.transform.localPosition = ( Vector3.right*0.5f + Vector3.right*Room.Width + Vector3.forward*0.5f ) * Room.TileSize;
            g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
            g.transform.parent = wallRoot.transform;
            g.transform.localPosition = (Vector3.left*0.5f + Vector3.back*Room.Height + Vector3.back*0.5f) * Room.TileSize;
            g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
            g.transform.parent = wallRoot.transform;
            g.transform.localPosition = (Vector3.right*Room.Width + Vector3.right * 0.5f + Vector3.back * Room.Height + Vector3.back * 0.5f) * Room.TileSize;
            for(int i=0; i<Room.Width; i++)
            {
                g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
                g.transform.parent = wallRoot.transform;
                g.transform.localPosition = (Vector3.right * i + Vector3.right * 0.5f + Vector3.forward * 0.5f) * Room.TileSize;
                g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
                g.transform.parent = wallRoot.transform;
                g.transform.localPosition = (Vector3.right * i + Vector3.right * 0.5f + Vector3.back * 0.5f + Vector3.back*Room.Height) * Room.TileSize;
            }
            for (int i = 0; i < Room.Height; i++)
            {
                g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
                g.transform.parent = wallRoot.transform;
                g.transform.localPosition = (Vector3.left * 0.5f + Vector3.back * 0.5f + Vector3.back * i) * Room.TileSize;
                g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
                g.transform.parent = wallRoot.transform;
                g.transform.localPosition = (Vector3.right * 0.5f + Vector3.right*Room.Width + Vector3.back * 0.5f + Vector3.back * i) * Room.TileSize;
            }
        }


        List<GameObject> LoadBorderResources()
        {
            string wallRes = "Borders/";

            string folder = string.Format("{0}x{1}", Room.TileSize, Room.TileSize);
            List<GameObject> ret = new List<GameObject>(Resources.LoadAll<GameObject>(wallRes + folder));
            
            return ret;
        }



    }

}
