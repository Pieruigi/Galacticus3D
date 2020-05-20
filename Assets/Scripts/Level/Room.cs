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
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }

        float tileSize;
        public float TileSize
        {
            get { return tileSize; }
        }
       
        GameObject wallRoot;
        public GameObject WallRoot
        {
            get { return wallRoot; }
        }

        protected abstract void CreateWalls();

        protected virtual void Awake()
        {
            // Create wall root and add the optimizer script
            wallRoot = new GameObject("Walls");
            wallRoot.transform.parent = transform;
            wallRoot.transform.localPosition = Vector3.zero;
            wallRoot.transform.localRotation = Quaternion.identity;
            //wallRoot.AddComponent<Optimizer>();

        }

        
        // Start is called before the first frame update
        protected virtual void Start()
        {
            CreateBorders();
            CreateWalls();
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

       
       
        protected virtual void CreateBorders()
        {
            List<GameObject> borders = LoadBorderResources();
            if (borders.Count == 0)
                return;
            GameObject g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
            g.transform.parent = wallRoot.transform;
            g.transform.localPosition = (Vector3.left * 0.5f + Vector3.forward * 0.5f) * tileSize;
            g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
            g.transform.parent = wallRoot.transform;
            g.transform.localPosition = (Vector3.right * 0.5f + Vector3.right * width + Vector3.forward * 0.5f) * tileSize;
            g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
            g.transform.parent = wallRoot.transform;
            g.transform.localPosition = (Vector3.left * 0.5f + Vector3.back * height + Vector3.back * 0.5f) * tileSize;
            g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
            g.transform.parent = wallRoot.transform;
            g.transform.localPosition = (Vector3.right * width + Vector3.right * 0.5f + Vector3.back * height + Vector3.back * 0.5f) * tileSize;
            for (int i = 0; i < width; i++)
            {
                g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
                g.transform.parent = wallRoot.transform;
                g.transform.localPosition = (Vector3.right * i + Vector3.right * 0.5f + Vector3.forward * 0.5f) * tileSize;
                g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
                g.transform.parent = wallRoot.transform;
                g.transform.localPosition = (Vector3.right * i + Vector3.right * 0.5f + Vector3.back * 0.5f + Vector3.back * height) * tileSize;
            }
            for (int i = 0; i < height; i++)
            {
                g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
                g.transform.parent = wallRoot.transform;
                g.transform.localPosition = (Vector3.left * 0.5f + Vector3.back * 0.5f + Vector3.back * i) * tileSize;
                g = GameObject.Instantiate(borders[Random.Range(0, borders.Count)]);
                g.transform.parent = wallRoot.transform;
                g.transform.localPosition = (Vector3.right * 0.5f + Vector3.right * width + Vector3.back * 0.5f + Vector3.back * i) * tileSize;
            }
        }


        List<GameObject> LoadBorderResources()
        {
            string wallRes = "Borders/";

            string folder = string.Format("{0}x{1}", tileSize, tileSize);
            List<GameObject> ret = new List<GameObject>(Resources.LoadAll<GameObject>(wallRes + folder));

            return ret;
        }
    }
}

