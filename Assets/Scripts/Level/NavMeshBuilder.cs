using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace OMTB.Level
{
    public class NavMeshBuilder : MonoBehaviour
    {
        [SerializeField]
        GameObject groundPrefab;

        [SerializeField]
        List<NavMeshSurface> navMeshSurfaces;

        List<GameObject> groundPlanes = new List<GameObject>();

        public static NavMeshBuilder Instance { get; private set; }

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void BuildNavMesh(List<Room> rooms)
        {
            // Creates a collision plane for each room
            foreach(Room r in rooms)
            {
                groundPlanes.Add(CreateGround(r));
            }

            foreach (NavMeshSurface s in navMeshSurfaces)
                s.BuildNavMesh();
                

            foreach (GameObject g in groundPlanes)
                Destroy(g);

            // Destroy all the navmesh colliders only
            GameObject[] gList = GameObject.FindGameObjectsWithTag("NavMeshCollider");
            for (int i = 0; i < gList.Length; i++)
                Destroy(gList[i].gameObject);

        }

        GameObject CreateGround(Room room)
        {
            GameObject plane = GameObject.Instantiate(groundPrefab);
            plane.transform.localScale = new Vector3(room.Width * room.TileSize / 10f, 1, room.Height * room.TileSize / 10f);
            plane.transform.position = room.transform.position + Vector3.right * (float)room.Width / 2f * room.TileSize + Vector3.back * (float)room.Height / 2f * room.TileSize;
            
            return plane;
        }
    }

}
