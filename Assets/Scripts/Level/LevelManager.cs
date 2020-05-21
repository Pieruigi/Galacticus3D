using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Level
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        float tileSize = 8;

        int minCommonRooms = 8;
        int maxCommonRooms = 11;

        int minExplorationRooms = 3;
        int minFightingRoom = 2;
        int minCustomRooms = 0;
        int commonRoomTypesNum = 3;

        int minBossRoomDepth = 2; // Starts from 0, the starting room


        int explorationRoomWallDistance = 2;
        int fightingRoomWallDistance = 5;

        int minExplorationRoomWallsH = 3;
        int maxExplorationRoomWallsH = 6;
        int minExplorationRoomWallsV = 2;
        int maxExplorationRoomWallsV = 3;

        int minFigthingRoomWallsH = 3;
        int maxFightingRoomWallsH = 6;
        int minFightingRoomWallsV = 2;
        int maxFightingRoomWallsV = 3;

        float bankRate = 0.75f;
        float spaceStationRate = 0.5f;

        int galacticusMinLevel = 3;
        float galacticusRate = 0.1f;

        List<Room> rooms = new List<Room>();
        //List<GameObject> allocators = new List<GameObject>();
        List<Portal> portals = new List<Portal>();
        public IList<Portal> Portals
        {
            get { return portals.AsReadOnly(); }
        }

        Room startingRoom;

        float lastRoomPosZ = 0;

        int level;
        public int Level
        {
            get { return level; }
        }

        int count = 1;

        Transform portalGroup;

        private void Awake()
        {


            if (Instance == null)
            {
                Instance = this;

                portalGroup = new GameObject("PortalGroup").transform;

                SetParamsByLevel();

                CreateLevel();


//                AllocateLevel();

                Debug.Log("Levelmanager Awake completed");
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
            Debug.Log("Levelmanager Start completed");

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

            // At least one room of each type of common room
            for (int i = 0; i < minExplorationRooms; i++)
                commonRooms.Add(CreateExplorationRoom().GetComponent<Room>());

            for (int i = 0; i < minFightingRoom; i++)
                commonRooms.Add(CreateFightingRoom().GetComponent<Room>());
        

            //for (int i = 0; i < minCustomRooms; i++)
            //    commonRooms.Add(new CustomRoom(new CustomRoomConfig() { Width = 12, Height = 9, TileSize = tileSize }));
            

            //Create the other common rooms
            int count = totalRooms - commonRooms.Count;
            for (int i = 0; i < count; i++)
            {

                int r = Random.Range(0, commonRoomTypesNum);
                if (r == 0)
                    commonRooms.Add(CreateExplorationRoom().GetComponent<Room>());
                else if (r == 1)
                    commonRooms.Add(CreateFightingRoom().GetComponent<Room>());
                
                //else
                //    commonRooms.Add(new CustomRoom(new CustomRoomConfig() { Width = 12, Height = 9, TileSize = tileSize }));
            }

            // Get the starting room
            startingRoom = commonRooms[Random.Range(0, commonRooms.Count)];


            // Create the boss room
            Room bossRoom = CreateBossRoom().GetComponent<Room>();
            int bossDepth = Random.Range(minBossRoomDepth, totalRooms - 2);


            // Check for special rooms
            List<Room> specialRooms = new List<Room>();
            if (Random.Range(0f, 1f) <= bankRate)
                specialRooms.Add(CreateBankRoom().GetComponent<BankRoom>());

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
            for (int i = 0; i < bossDepth; i++)
            {
                Room room = commonRooms[Random.Range(0, commonRooms.Count)];
                commonRooms.Remove(room);
                used.Add(room);

                // Create portals
                CreatePortals(prevRoom, room, false);
        
                // Update the current room
                prevRoom = room;
            }

            // Add the boss room ( only portals ); the used list only holds common rooms
            CreatePortals(prevRoom, bossRoom, false);
            
            // Add remaining common rooms
            count = commonRooms.Count;
            for (int i = 0; i < count; i++)
            {
                // Get the parent room
                Room parent = used[Random.Range(0, used.Count)];

                // Get the child room
                Room child = commonRooms[0];
                commonRooms.RemoveAt(0);

                // Add to used
                used.Add(child);

                // Set portals
                CreatePortals(parent, child, false);
                
            }

            // Add special rooms
            count = specialRooms.Count;
            for (int i = 0; i < count; i++)
            {
                // Get the parent room
                Room parent = used[Random.Range(0, used.Count)];

                // Get the child room
                Room child = specialRooms[i];

                // Set portals
                CreatePortals(parent, child, false);
             
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
            int count = portals.Count;
            for(int i=0; i<count; i++)
            {
                Destroy(portals[0].gameObject);
            }
            portals.Clear();
            count = rooms.Count;
            for (int i = 0; i < count; i++)
            {
                Destroy(rooms[0].gameObject);
            }
        }

        #endregion


        #region LEVEL_ALLOCATION
        private void CreatePortals(Room room1, Room room2, bool isLocked)
        {
            int portalSize = 2;

            // Create object
            List<GameObject> res = LoadPortalResources();
            GameObject g = GameObject.Instantiate(res[Random.Range(0, res.Count)]);
            GameObject g2 = GameObject.Instantiate(res[Random.Range(0, res.Count)]);

            g.GetComponent<Portal>().Init(room1, g2.GetComponent<Portal>(), isLocked);
            g2.GetComponent<Portal>().Init(room2, g.GetComponent<Portal>(), isLocked);

            // Set position
            g.transform.position = room1.GetRandomSpawnPosition(portalSize, portalSize);
            g2.transform.position = room2.GetRandomSpawnPosition(portalSize, portalSize);

            portals.Add(g.GetComponent<Portal>());
            portals.Add(g2.GetComponent<Portal>());

            g.transform.parent = portalGroup;
            g2.transform.parent = portalGroup;

            g.AddComponent<DistanceClipper>();
            g2.AddComponent<DistanceClipper>();
         

        }

        List<GameObject> LoadPortalResources()
        {
            string res = "Portals/";

            List<GameObject> ret = new List<GameObject>(Resources.LoadAll<GameObject>(res));

            return ret;
        }

        private GameObject CreateRoom(RoomConfig config, System.Type roomType)
        {
            GameObject ret = new GameObject("Room_"+count);
            if(count > 1)
            {
                ret.transform.position = Vector3.forward * ( lastRoomPosZ +  config.Width * config.TileSize + 3 * config.TileSize );
            }

            lastRoomPosZ = ret.transform.position.z;

            Room comp = null;
            if (roomType == typeof(Labyrinth))
                comp = ret.AddComponent<Labyrinth>();
            else if (roomType == typeof(BossRoom))
                comp = ret.AddComponent<BossRoom>();
            else if (roomType == typeof(BankRoom))
                comp = ret.AddComponent<BankRoom>();

            comp.Init(config);
            comp.Create();
            count++;
            return ret;
        }

        private GameObject CreateExplorationRoom()
        {
            int w = explorationRoomWallDistance + (explorationRoomWallDistance + 1) * Random.Range(minExplorationRoomWallsH, maxExplorationRoomWallsH + 1);// /*num of horizontal walls*/2;

            int h = explorationRoomWallDistance + (explorationRoomWallDistance + 1) * Random.Range(minExplorationRoomWallsV, maxExplorationRoomWallsV + 1);// */*num of vertical walls*/2;

            // Create Room object
            return CreateRoom(new LabyrinthConfig() { CorridorWidth = explorationRoomWallDistance, Width = w, Height = h, TileSize = tileSize }, typeof(Labyrinth));
        }

        private GameObject CreateFightingRoom()
        {
            int w = fightingRoomWallDistance + (fightingRoomWallDistance + 1) * Random.Range(minFigthingRoomWallsH, maxFightingRoomWallsH + 1);// * 2;
            int h = fightingRoomWallDistance + (fightingRoomWallDistance + 1) * Random.Range(minFightingRoomWallsV, maxFightingRoomWallsV + 1);//*1;
            return CreateRoom(new LabyrinthConfig() { CorridorWidth = fightingRoomWallDistance, Width = w, Height = h, TileSize = tileSize }, typeof(Labyrinth));
        }

        private GameObject CreateCustomRoom()
        {
            throw new System.Exception("Not implemented");
        }

        private GameObject CreateBossRoom()
        {
            GameObject room = CreateRoom(new RoomConfig() { Width = 10, Height = 6, TileSize = 8 }, typeof(BossRoom));
            room.name += "_Boss";
            return room;
        }

        private GameObject CreateBankRoom()
        {
            GameObject room = CreateRoom(new RoomConfig() { Width = 10, Height = 6, TileSize = 8 }, typeof(BankRoom));
            room.name += "_Bank";
            return room;
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
