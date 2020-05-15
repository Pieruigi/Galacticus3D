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


        int smallLabyrinthWallDistance = 2;
        int hugeLabyrinthWallDistance = 3;

        float bankRate = 0.75f;
        float spaceStationRate = 0.5f;

        int galacticusMinLevel = 3;
        float galacticusRate = 0.1f;

        List<Room> rooms = new List<Room>();

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

            int smallWidth = smallLabyrinthWallDistance + (smallLabyrinthWallDistance + 1)* /*num of horizontal walls*/2;

            int smallHeight = smallLabyrinthWallDistance + (smallLabyrinthWallDistance+1)*/*num of vertical walls*/2;

            int hugeWidth = hugeLabyrinthWallDistance + (hugeLabyrinthWallDistance+1)*2;
            int hugeHeight = hugeLabyrinthWallDistance + (hugeLabyrinthWallDistance+1)*1;

            // At least one room of each type of common room
            for (int i = 0; i < minSmallLabyrinths; i++)
                commonRooms.Add(new Labyrinth(new LabyrinthConfig() { TilesBetweenWalls = smallLabyrinthWallDistance, Width = smallWidth, Height = smallHeight, TileSize = tileSize }));
            
            for (int i = 0; i < minHugeLabyrinths; i++)
                commonRooms.Add(new Labyrinth(new LabyrinthConfig() { TilesBetweenWalls = hugeLabyrinthWallDistance, Width = hugeWidth, Height = hugeHeight, TileSize = tileSize }));

            for (int i = 0; i < minCustomRooms; i++)
                commonRooms.Add(new CustomRoom(new CustomRoomConfig() { Width = 12, Height = 9, TileSize = tileSize }));

            // Create the other common rooms
            int count = totalRooms - commonRooms.Count;
            for (int i = 0; i < count; i++)
            {
                int r = Random.Range(0, commonRoomTypesNum);
                if(r == 0)
                    commonRooms.Add(new Labyrinth(new LabyrinthConfig() { TilesBetweenWalls = smallLabyrinthWallDistance, Width = smallWidth, Height = smallHeight, TileSize = tileSize }));
                else if(r == 1)
                    commonRooms.Add(new Labyrinth(new LabyrinthConfig() { TilesBetweenWalls = hugeLabyrinthWallDistance, Width = hugeWidth, Height = hugeHeight, TileSize = tileSize }));
                else
                    commonRooms.Add(new CustomRoom(new CustomRoomConfig() { Width = 12, Height = 9, TileSize = tileSize }));
            }

            // Get the starting room
            startingRoom = commonRooms[Random.Range(0, commonRooms.Count)];

#if UNITY_EDITOR
            for (int i = 0; i < commonRooms.Count; i++)
                commonRooms[i].RoomName = "Room_" + i;
#endif

            // Create the boss room
            Room bossRoom = new BossRoom(new BossRoomConfig() { Width = 10, Height = 10, TileSize = tileSize });
            int bossDepth = Random.Range(minBossRoomDepth, totalRooms - 2);
#if UNITY_EDITOR
            bossRoom.RoomName = "BossRoom";
#endif


            // Check for special rooms
            List<Room> specialRooms = new List<Room>();
            if (Random.Range(0f, 1f) <= bankRate) 
                specialRooms.Add(new BankRoom(new RoomConfig() { Width = 5, Height = 5, TileSize = tileSize }));

#if UNITY_EDITOR
            if(specialRooms.Count > 0)
                specialRooms[specialRooms.Count-1].RoomName = "BankRoom";
#endif
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

            // Add the boss room ( only portals ); the used list only holds common rooms
            prevRoom.AddPortal(new Portal(new PortalConfig() { TargetRoom = bossRoom, IsClosed = true }));
            bossRoom.AddPortal(new Portal(new PortalConfig() { TargetRoom = prevRoom, IsClosed = true }));

            // Add remaining common rooms
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

            // Add special rooms
            count = specialRooms.Count;
            for(int i=0; i<count; i++)
            {
                // Get the parent room
                Room parent = used[Random.Range(0, used.Count)];

                // Get the child room
                Room child = specialRooms[i];

                // Set portals
                parent.AddPortal(new Portal(new PortalConfig() { TargetRoom = child }));
                child.AddPortal(new Portal(new PortalConfig() { TargetRoom = parent }));
            }

            // Store all rooms
            foreach (Room r in commonRooms)
                rooms.Add(r);
                
            foreach(Room r in specialRooms)
                rooms.Add(r);

            rooms.Add(bossRoom);

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
            Debug.Log(string.Format("StartingRoom:{0}", startingRoom));
            Debug.Log("Rooms:");
            foreach (Room r in used)
                Debug.Log(r);

#endif
        }

        void ClearAll()
        {
            startingRoom = null;
            rooms.Clear();
            
        }


    }

}
