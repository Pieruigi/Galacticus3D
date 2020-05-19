﻿using System.Collections;
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
        int hugeLabyrinthWallDistance = 5;

        int minSmallLabyrinthWallsH = 3;
        int maxSmallLabyrinthWallsH = 6;
        int minSmallLabyrinthWallsV = 2;
        int maxSmallLabyrinthWallsV = 3;

        int minHugeLabyrinthWallsH = 3;
        int maxHugeLabyrinthWallsH = 6;
        int minHugeLabyrinthWallsV = 2;
        int maxHugeLabyrinthWallsV = 3;

        float bankRate = 0.75f;
        float spaceStationRate = 0.5f;

        int galacticusMinLevel = 3;
        float galacticusRate = 0.1f;

        List<Room> rooms = new List<Room>();
        List<GameObject> allocators = new List<GameObject>(); 

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

                CreateLevel();

                

                AllocateLevel();
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

        #region LEVEL_CREATION
        void CreateLevel()
        {
            CreateRooms();
        }

        
        void CreateRooms()
        {
            ClearAll();

            List<Room> commonRooms = new List<Room>();

            // How many rooms in the current level?
            int totalRooms = Random.Range(minCommonRooms, maxCommonRooms + 1);


            int w, h;
         

            // At least one room of each type of common room
            for (int i = 0; i < minSmallLabyrinths; i++)
            {
                w = smallLabyrinthWallDistance + (smallLabyrinthWallDistance + 1) * Random.Range(minSmallLabyrinthWallsH, maxSmallLabyrinthWallsH + 1);// /*num of horizontal walls*/2;

                h = smallLabyrinthWallDistance + (smallLabyrinthWallDistance + 1) * Random.Range(minSmallLabyrinthWallsV, maxSmallLabyrinthWallsV + 1);// */*num of vertical walls*/2;
                commonRooms.Add(new Labyrinth(new LabyrinthConfig() { TilesBetweenWalls = smallLabyrinthWallDistance, Width = w, Height = h, TileSize = tileSize }));
            }
                
            
            for (int i = 0; i < minHugeLabyrinths; i++)
            {
                w = hugeLabyrinthWallDistance + (hugeLabyrinthWallDistance + 1) * Random.Range(minHugeLabyrinthWallsH, maxHugeLabyrinthWallsH + 1);// * 2;
                h = hugeLabyrinthWallDistance + (hugeLabyrinthWallDistance + 1) * Random.Range(minHugeLabyrinthWallsV, maxHugeLabyrinthWallsV + 1);//*1;
                commonRooms.Add(new Labyrinth(new LabyrinthConfig() { TilesBetweenWalls = hugeLabyrinthWallDistance, Width = w, Height = h, TileSize = tileSize }));
            }
                

            for (int i = 0; i < minCustomRooms; i++)
                commonRooms.Add(new CustomRoom(new CustomRoomConfig() { Width = 12, Height = 9, TileSize = tileSize }));

            // Create the other common rooms
            int count = totalRooms - commonRooms.Count;
            for (int i = 0; i < count; i++)
            {
        
                int r = Random.Range(0, commonRoomTypesNum);
                if(r == 0)
                {
                    w = smallLabyrinthWallDistance + (smallLabyrinthWallDistance + 1) * Random.Range(minSmallLabyrinthWallsH, maxSmallLabyrinthWallsH + 1);// /*num of horizontal walls*/2;

                    h = smallLabyrinthWallDistance + (smallLabyrinthWallDistance + 1) * Random.Range(minSmallLabyrinthWallsV, maxSmallLabyrinthWallsV + 1);// */*num of vertical walls*/2;
                    commonRooms.Add(new Labyrinth(new LabyrinthConfig() { TilesBetweenWalls = smallLabyrinthWallDistance, Width = w, Height = h, TileSize = tileSize }));
                }
                    
                else if(r == 1)
                {
                    w = hugeLabyrinthWallDistance + (hugeLabyrinthWallDistance + 1) * Random.Range(minHugeLabyrinthWallsH, maxHugeLabyrinthWallsH + 1);// * 2;
                    h = hugeLabyrinthWallDistance + (hugeLabyrinthWallDistance + 1) * Random.Range(minHugeLabyrinthWallsV, maxHugeLabyrinthWallsV + 1);//*1;
                    commonRooms.Add(new Labyrinth(new LabyrinthConfig() { TilesBetweenWalls = hugeLabyrinthWallDistance, Width = w, Height = h, TileSize = tileSize }));
                }
                    
                else
                    commonRooms.Add(new CustomRoom(new CustomRoomConfig() { Width = 12, Height = 9, TileSize = tileSize }));
            }

            // Get the starting room
            startingRoom = commonRooms[Random.Range(0, commonRooms.Count)];


            for (int i = 0; i < commonRooms.Count; i++)
                commonRooms[i].RoomName = "Room_" + i;

            // Create the boss room
            Room bossRoom = new BossRoom(new BossRoomConfig() { Width = 10, Height = 10, TileSize = tileSize });
            int bossDepth = Random.Range(minBossRoomDepth, totalRooms - 2);

            bossRoom.RoomName = "BossRoom";


            // Check for special rooms
            List<Room> specialRooms = new List<Room>();
            if (Random.Range(0f, 1f) <= bankRate) 
                specialRooms.Add(new BankRoom(new RoomConfig() { Width = 5, Height = 5, TileSize = tileSize }));


            if(specialRooms.Count > 0)
                specialRooms[specialRooms.Count-1].RoomName = "BankRoom";

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
            foreach (Room r in used)
                rooms.Add(r);
            
            foreach (Room r in specialRooms)
                rooms.Add(r);

            rooms.Add(bossRoom);
            
            


            //
            // Debug
            //
            DebugLevel();

        }

        void ClearAll()
        {
            startingRoom = null;
            rooms.Clear();
            
        }

        #endregion


        #region LEVEL_ALLOCATION

        void AllocateLevel()
        {
            float dist = 0; // Distance between rooms
            foreach(Room room in rooms)
            {
                dist += (room.Height + 4) * room.TileSize;

                // Create root game object
                GameObject obj = new GameObject(room.RoomName);
                allocators.Add(obj);

                obj.transform.position = Vector3.zero + Vector3.forward * dist;
                obj.transform.rotation = Quaternion.identity;
                
                // Add the component and allocate objects
                //RoomAllocator allocator = obj.AddComponent<RoomAllocator>();
                System.Type t = RoomAllocatorFactory.Instance.GetRoomAllocator(room.GetType());
                if(t != null)
                {
                    RoomAllocator alloc = obj.AddComponent(t) as RoomAllocator;
                    alloc.Room = room;
                    //alloc.Allocate();
                }
                    
                //allocator.Allocate(room);
            }
        }

        #endregion

        void DebugLevel()
        {
            Debug.Log(".......................................");
            Debug.Log(string.Format("Level:{0}\n", level));
            Debug.Log(string.Format("Number of rooms:{0}\n", rooms.Count));
            Debug.Log(string.Format("StartingRoom:{0}", startingRoom));
            Debug.Log("Rooms:");
            foreach (Room r in rooms)
                Debug.Log(r);
        }

    }

}
