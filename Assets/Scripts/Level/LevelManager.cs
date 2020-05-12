using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{


    public class LevelManager : MonoBehaviour
    {
        
        
        public static LevelManager Instance { get; private set; }

        float tileSize = 8;

        int minCommonRooms = 8;
        int maxCommonRooms = 11;

        int minSmallLabyrinths = 3;
        int minHugeLabyrinths = 2;
        int minCustomRooms = 1;
        int commonRoomTypesNum = 3;

        int minBossRoomDepth = 2; // Starts from 0, the starting room


        int smallLabyrinthWallDistance = 1;
        int hugeLabyrinthWallDistance = 2;

        float bankRate = 0.5f;
        float spaceStationRate = 0.5f;

        int galacticusMinLevel = 3;
        float galacticusRate = 0.1f;

        List<Room> rooms;

        Room startingRoom;

        int level;
        public int Level
        {
            get { return level; }
        }

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
            if (Input.GetKeyDown(KeyCode.L))
                CreateRooms();
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
            ClearAll();

            List<Room> commonRooms = new List<Room>();

            // How many rooms in the current level?
            int totalRooms = Random.Range(minCommonRooms, maxCommonRooms + 1);

            // At least one room of each type of common room
            for (int i = 0; i < minSmallLabyrinths; i++)
                commonRooms.Add(new Labyrinth(new LabyrinthConfig() { TilesBetweenWalls = smallLabyrinthWallDistance, Width = 18, Height = 9, TileSize = tileSize }));

            for (int i = 0; i < minHugeLabyrinths; i++)
                commonRooms.Add(new Labyrinth(new LabyrinthConfig() { TilesBetweenWalls = hugeLabyrinthWallDistance, Width = 12, Height = 9, TileSize = tileSize }));

            for (int i = 0; i < minCustomRooms; i++)
                commonRooms.Add(new CustomRoom(new CustomRoomConfig() { Width = 12, Height = 9, TileSize = tileSize }));

            // Create the other common rooms
            int count = totalRooms - commonRooms.Count;
            for (int i = 0; i < count; i++)
            {
                int r = Random.Range(0, commonRoomTypesNum);
                if(r == 0)
                    commonRooms.Add(new Labyrinth(new LabyrinthConfig() { TilesBetweenWalls = smallLabyrinthWallDistance, Width = 18, Height = 9, TileSize = tileSize }));
                else if(r == 1)
                    commonRooms.Add(new Labyrinth(new LabyrinthConfig() { TilesBetweenWalls = hugeLabyrinthWallDistance, Width = 18, Height = 9, TileSize = tileSize }));
                else
                    commonRooms.Add(new CustomRoom(new CustomRoomConfig() { Width = 12, Height = 9, TileSize = tileSize }));
            }

            // Get the starting room
            startingRoom = commonRooms[Random.Range(0, commonRooms.Count)];
           

            // Create the boss room
            Room bossRoom = new BossRoom(new BossRoomConfig() { Width = 10, Height = 10, TileSize = tileSize });
            int bossDepth = Random.Range(minBossRoomDepth, totalRooms - 2);


            // Check for special rooms
            List<Room> specialRooms = new List<Room>();
            if (Random.Range(0f, 1f) <= bankRate)
                specialRooms.Add(new BankRoom(new RoomConfig() { Width = 5, Height = 5, TileSize = tileSize }));

            //
            // Start creating tree
            //
            List<Room> used = new List<Room>();

            // Add the starting room to the used list
            Room prevRoom = startingRoom;
            used.Add(startingRoom);
            // Remove the starting room from the to use list
            commonRooms.Remove(startingRoom);
            
            // Create the tree up to the boss room
            for(int i=0; i<bossDepth; i++)
            {
                Room room = commonRooms[Random.Range(0, commonRooms.Count)];
                commonRooms.Remove(room);
                used.Add(room);

                // Add two portals, one for each room
                prevRoom.AddPortal(new Portal(new PortalConfig() { TargetRoom = room }));
                room.AddPortal(new Portal(new PortalConfig() { TargetRoom = prevRoom }));

                // Update the current room
                prevRoom = room;
            }

            // Attach the boss room to the tree
            prevRoom.AddPortal(new Portal(new PortalConfig() { TargetRoom = bossRoom, IsClosed = true }));
            bossRoom.AddPortal(new Portal(new PortalConfig() { TargetRoom = prevRoom, IsClosed = true }));

            // Add the remaining common rooms
            count = commonRooms.Count;
            for(int i=0; i<count; i++)
            {
                // Get the parent room
                Room parent = used[Random.Range(0, used.Count)];

                // Get the child room
                Room child = commonRooms[0];
                commonRooms.RemoveAt(0);

                // Add to used
                used.Add(child);

                // Set portals
                parent.AddPortal(new Portal(new PortalConfig() { TargetRoom = child }));
                child.AddPortal(new Portal(new PortalConfig() { TargetRoom = parent }));
            }

            // Finish adding the special rooms


            //// Set rooms
            //rooms = new List<Room>();
            //for (int i = 0; i < commonRooms.Count; i++)
            //    rooms.Add(commonRooms[i]);
            //rooms.Add(bossRoom);
            //for (int i = 0; i < specialRooms.Count; i++)
            //    rooms.Add(specialRooms[i]);




#if UNITY_EDITOR
            //
            // Debug
            //
            Debug.Log(".......................................");
            Debug.Log(string.Format("Level:{0}\n", level));
            Debug.Log(string.Format("Number of rooms:{0}\n", totalRooms));
            Debug.Log( string.Format("Number of common rooms:{0}\n", commonRooms.Count));
            Debug.Log(string.Format("Number of special rooms:{0}\n", specialRooms.Count));
            Debug.Log(string.Format("Boss room depth:{0}\n", bossDepth));
           
#endif
        }

        void ClearAll()
        {
            if(rooms != null)
                rooms.Clear();

            startingRoom = null;
        }


    }

}
