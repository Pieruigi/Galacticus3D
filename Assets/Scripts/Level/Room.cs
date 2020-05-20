using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Level
{
    public class RoomConfig
    {
        
        public int Width { get; set; }

        public int Height { get; set; }

        public float TileSize { get; set; }
    }

    public abstract class Room : MonoBehaviour
    {
        int width, height;
        float tileSize;

        GameObject wallRoot;
        public GameObject WallRoot
        {
            get { return wallRoot; }
        }

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

        }

        // Update is called once per frame
        void Update()
        {

        }

        
        public virtual void Init(RoomConfig config)
        {
            Debug.Log("Init");
            width = config.Width;
            height = config.Height;
            tileSize = config.TileSize;
        }
    }
}

