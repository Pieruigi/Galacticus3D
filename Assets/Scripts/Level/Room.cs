using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collections;

namespace OMTB.Level
{

    
    
    public class RoomConfig
    {
        public RoomType RoomType { get; set; }
        
        public int Width { get; set; }

        public int Height { get; set; }

        public float TileSize { get; set; }
    }

    public abstract class Room : MonoBehaviour
    {
        //[SerializeField]
        RoomType roomType;
        public RoomType RoomType
        {
            get { return roomType; }
            set { roomType = value; }
        }

        [SerializeField]
        int width, height;
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }

        [SerializeField]
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

        

        int[] tiles; // tile[i] == 0 means the tile at the index 'i' is free
     

        protected abstract void CreateWalls();

        protected virtual void Awake()
        {
            // Create wall root and add the optimizer script
            wallRoot = new GameObject("Walls");
            wallRoot.transform.parent = transform;
            wallRoot.transform.localPosition = Vector3.zero;
            wallRoot.transform.localRotation = Quaternion.identity;
            wallRoot.AddComponent<WallOptimizer>();

        }

        
        // Start is called before the first frame update
        protected virtual void Start()
        {
            
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public virtual void Create()
        {
            CreateBorders();
            CreateWalls();
         
        }

        public virtual void Init(RoomConfig config)
        {
            roomType = config.RoomType;
            width = config.Width;
            height = config.Height;
            tileSize = config.TileSize;
            tiles = new int[width * height];
        }

        /**
         * Width: how many horizontal tiles it takes
         * height: how many vertical tiles it takes
         * */
        public virtual Vector3 GetRandomSpawnPosition(int widthInTiles, int heightInTiles)
        {
            // Get all the tiles that match reference
            List<int> allowedTiles = GetRandomAdjacentFreeTiles(heightInTiles, widthInTiles);

            // Get a random group
            int rootTile = allowedTiles[Random.Range(0, allowedTiles.Count)];

            for (int i = 0; i < heightInTiles; i++)
            {
                for (int j = 0; j < widthInTiles; j++)
                {
                    int index = rootTile + j + (i * width);
                    tiles[index] = 1;

                }
            }

            float x = rootTile%width + widthInTiles - (float)widthInTiles / 2f;
            x *= TileSize;
            float z = (rootTile/width + heightInTiles - (float)heightInTiles / 2f);
            z *= -TileSize;
        
            return  (transform.position + new Vector3(x, 0, z));

        }

        public virtual Vector3 GetPortalPosition(int widthInTiles, int heightInTiles)
        {
            return GetRandomSpawnPosition(widthInTiles, heightInTiles);
        } 

        protected void SetTile(int index)
        {
            tiles[index] = 1;
        }

        protected void ResetTile(int index)
        {
            tiles[index] = 0;
        }
       
        protected int GetTile(int index)
        {
            return tiles[index];
        }

        /**
        * Some droppables may occupie more than one tile
        * */
        protected List<int> GetRandomAdjacentFreeTiles(int rows, int cols)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < tiles.Length; i++)
                indices.Add(i);
            List<int> all = indices.FindAll(delegate (int t)
            {
                // Debug.Log("Delegate t:" + t);
                if (t%width + cols > width)
                    return false;
                if (t/width + rows > height)
                    return false;



                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        int index = t + j + (i * width);

                      
                        if (tiles[index] != 0)
                            return false;
                    }
                }

                return true;
            });

            return all;
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

