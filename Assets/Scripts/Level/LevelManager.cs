using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{


    public class LevelManager : MonoBehaviour
    {
        
        public static LevelManager Instance { get; private set; }

        float tileSize = 8;

        int minRoom = 9;
        int maxRoom = 11;
       
        float bankRate = 0.5f;
        float spaceStationRate = 0.5f;

        int galacticusMinLevel = 3;
        float galacticusRate = 0.1f;

        List<Room> rooms;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                SetParamsByLevel();

                InitLevel();
            }
            else
            {
                GameObject.Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            //CollectionManager.Create();
            //System.DateTime start = System.DateTime.UtcNow;
            //OMTB.Gameplay.Droppable[] a = Resources.LoadAll<OMTB.Gameplay.Droppable>("Droppables");
            //System.DateTime end = System.DateTime.UtcNow;
            //Debug.Log("Loaded " + a.Length + " in " + (end - start).TotalMilliseconds +" mills.");

        }

        // Update is called once per frame
        void Update()
        {

        }

        /**
         * Specify parameters by level
         * */
        void SetParamsByLevel()
        {
            
            
        }

        void InitLevel()
        {
            CreateRooms();
        }

        void CreateRooms()
        {
            rooms = new List<Room>();

            // How many rooms in the current level?
            int count = Random.Range(minRoom, maxRoom + 1);

            // At least one room of each type
            int small = 1, huge = 1, custom = 1;
            rooms.Add(new Labyrinth(new LabyrinthConfig() { TilesBetweenWalls = 1, Width = 18, Height = 9, TileSize = tileSize }));
            rooms.Add(new Labyrinth(new LabyrinthConfig() { TilesBetweenWalls = 2, Width = 12, Height = 9, TileSize = tileSize }));
            rooms.Add(new CustomRoom(new CustomRoomConfig() { Width = 12, Height = 9, TileSize = tileSize }));
        }
    }

}
