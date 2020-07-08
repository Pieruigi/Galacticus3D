using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collections;

namespace OMTB.Level
{
    public class LevelManager : MonoBehaviour
    {
        public const int NumberOfLevels = 8;

        public static LevelManager Instance { get; private set; }

        float tileSize = 8;
        public float TileSize
        {
            get { return tileSize; }
        }

        int minCommonRooms = 5;
        int maxCommonRooms = 7;

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

        int portalSize = 2;

        List<Room> rooms = new List<Room>(); // List of rooms
        List<Portal> portals = new List<Portal>(); // List of portals
        List<GameObject> enemies = new List<GameObject>();
        Room bossRoom;
        Boss boss;
        Room bankRoom;
        SpecialShip bank;

        List<OMTB.Collections.Room> roomResources;

        public IList<Portal> Portals
        {
            get { return portals.AsReadOnly(); }
        }

        Room startingRoom;
        Room currentRoom;
        public Room CurrentRoom
        {
            get { return currentRoom; }
        }

        float lastRoomPosZ = 0;

        int level = 2;
        public int Level
        {
            get { return level; }
        }

        int count = 1;

        Transform portalGroup;

        private GameObject player;
        public GameObject Player
        {
            get { return player; }
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                portalGroup = new GameObject("PortalGroup").transform;

                SetParamsByLevel();

                CreateLevel();

                player = GameObject.FindGameObjectWithTag("Player");

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
            Debug.Log("Levelmanager - Starting game...");
            
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
            ChooseBoss();

            CheckSpecialShips();

            CreateRooms();

            AddBoss();

            AddEnemies();

            AddSpecialShips();
        
            AddDroppables();

            GameObject.FindObjectOfType<NavMeshBuilder>().BuildNavMesh(rooms);

            // Disable all the objects of each room except for the starting one
            foreach(Room r in rooms)
            {
                Debug.Log("Check room" + r);
                if (startingRoom == r)
                    ActivateRoomObjects(r, true);
                else
                    ActivateRoomObjects(r, false);
            }

            // Set player position
            CreatePlayer();
            
        }

        void ChooseBoss()
        {
            // Load allowed bosses
            //List<Boss> bosses = new List<Boss>(Resources.LoadAll<Boss>(Boss.ResourceFolder)).FindAll(b => b.Level == level || b.Level + 1 == level || b.Level - 1 == level);
            List<Boss> bosses = Boss.GetResourcesForSpawning(level);

            // Get a boss
            boss = bosses[Random.Range(0, bosses.Count)];
        }

        void CheckSpecialShips()
        {
            // Get all the available ships for the current level
            List<SpecialShip> ships = SpecialShip.GetAvailableResources(level);

            if (ships.Count == 0)
                return;

            // Check for bank
            bank = ships.Find(s => s.Type == SpecialShipType.IntergalacticBank);
            
        }

        void CreateRooms()
        {
            ClearAll();

            // Load all room resources
            roomResources = new List<OMTB.Collections.Room>(Resources.LoadAll<OMTB.Collections.Room>(OMTB.Collections.Room.ResourceFolder));

            List<Room> commonRooms = new List<Room>();

            // Set room total number for the current level
            int totalRooms = Random.Range(minCommonRooms, maxCommonRooms + 1);

            // At least one room of each type of common room
            for (int i = 0; i < minExplorationRooms; i++)
                commonRooms.Add(CreateRoom(RoomType.Exploration).GetComponent<Room>()); //commonRooms.Add(CreateExplorationRoom().GetComponent<Room>());

            for (int i = 0; i < minFightingRoom; i++)
                commonRooms.Add(CreateRoom(RoomType.Fighting).GetComponent<Room>()); //commonRooms.Add(CreateFightingRoom().GetComponent<Room>());
        

            //for (int i = 0; i < minCustomRooms; i++)
            //    commonRooms.Add(new CustomRoom(new CustomRoomConfig() { Width = 12, Height = 9, TileSize = tileSize }));
            

            //Create the other common rooms
            int count = totalRooms - commonRooms.Count;
            for (int i = 0; i < count; i++)
            {

                int r = Random.Range(0, commonRoomTypesNum);
                if (r == 0)
                    commonRooms.Add(CreateRoom(RoomType.Exploration).GetComponent<Room>()); //commonRooms.Add(CreateExplorationRoom().GetComponent<Room>());
                else if (r == 1)
                    commonRooms.Add(CreateRoom(RoomType.Fighting).GetComponent<Room>());//commonRooms.Add(CreateFightingRoom().GetComponent<Room>());

                //else
                //    commonRooms.Add(new CustomRoom(new CustomRoomConfig() { Width = 12, Height = 9, TileSize = tileSize }));
            }


            // Create the starting room
            startingRoom = CreateRoom(RoomType.Starting).GetComponent<Room>();//CreateStartingRoom().GetComponent<Room>();
            currentRoom = startingRoom;

            // Create the boss room
            bossRoom = CreateRoom(RoomType.Boss).GetComponent<Room>();//CreateBossRoom().GetComponent<Room>();
            int bossDepth = Random.Range(minBossRoomDepth, totalRooms - 2);


            // Check for special rooms
            List<Room> specialRooms = new List<Room>();// Local variable only used to create tree
            if (bank)
            {
                Debug.Log("Create BankRoom");
                bankRoom = CreateRoom(RoomType.Bank).GetComponent<Room>();
                specialRooms.Add(bankRoom); 
            }



            //
            // Start creating tree 
            //

            // Put into the following list only rooms that can have more than one portal ( normally the common rooms )
            List<Room> used = new List<Room>();

            // Add the starting room to the used list
            Room prevRoom = startingRoom;
            
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


        #region ROOM_ALLOCATION

        private void CreatePortals(Room room1, Room room2, bool isLocked)
        {
            // Create object
            List<GameObject> res = LoadPortalResources();
            GameObject g = GameObject.Instantiate(res[Random.Range(0, res.Count)]);
            GameObject g2 = GameObject.Instantiate(res[Random.Range(0, res.Count)]);

            g.GetComponent<Portal>().Init(room1, g2.GetComponent<Portal>(), isLocked);
            g2.GetComponent<Portal>().Init(room2, g.GetComponent<Portal>(), isLocked);

            // Set position
            g.transform.position = room1.GetPortalPosition(portalSize, portalSize);
            g2.transform.position = room2.GetPortalPosition(portalSize, portalSize);

            portals.Add(g.GetComponent<Portal>());
            portals.Add(g2.GetComponent<Portal>());

            g.transform.parent = portalGroup;
            g2.transform.parent = portalGroup;

            g.AddComponent<DistanceClipper>();
            g2.AddComponent<DistanceClipper>();

            g.GetComponent<Portal>().OnTeleport += HandleOnTeleport;
            g2.GetComponent<Portal>().OnTeleport += HandleOnTeleport;

        }

        /**
         * Create a room of some kind ( create room hub )
         * */
        GameObject CreateRoom(RoomType roomType)
        {
            GameObject ret = null;
            switch (roomType)
            {
                case RoomType.Exploration:
                    ret = CreateExplorationRoom();
                    break;
                case RoomType.Fighting:
                    ret = CreateFightingRoom();
                    break;
                case RoomType.Starting:
                    ret = CreateStartingRoom();
                    break;
                case RoomType.Boss:
                    ret = CreateBossRoom();
                    break;
                case RoomType.Bank:
                    ret = CreateBankRoom();
                    break;
            }

            if (count > 1)
            {
                Room r = ret.GetComponent<Room>();
                ret.transform.position = Vector3.forward * (lastRoomPosZ + r.Width * r.TileSize + 3 * r.TileSize);
            }

            count++;
            rooms.Add(ret.GetComponent<Room>());
            lastRoomPosZ = ret.transform.position.z;
            return ret;
        }



        #endregion

        #region OBJECTS_ALLOCATION

        void CreatePlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerSpawner ps = startingRoom.GetComponent<PlayerSpawner>();
            player.transform.position = ps.SpawnPoint.position;
            player.transform.rotation = ps.SpawnPoint.rotation;

        }


        void AddEnemies()
        {
            //List<Enemy> enemies = new List<Enemy>(Resources.LoadAll<Enemy>(Enemy.ResourceFolder)).FindAll(e=>e.MinLevel <= level && e.MaxLevel >= level);

            //List<Enemy> enemies = new List<Enemy>(Resources.LoadAll<Enemy>(Enemy.ResourceFolder)).FindAll(e => e.SpawnData.MinLevel <= level && e.SpawnData.MaxLevel >= level);
            
            foreach(Room r in rooms)
            {
                Debug.Log("Enemies - Room:" + r.name);
                RoomEnemyData red = r.GetComponent<RoomEnemyData>();
                if (red)
                {
                    List<Enemy> enemies = Enemy.GetResourcesForSpawning(level, r.RoomType);

                    int count = Random.Range(red.MinEnemyCount, red.MaxEnemyCount + 1);
                    for(int i=0; i<count; i++)
                    {
                        // Add random enemy
                        Enemy e = enemies[Random.Range(0, enemies.Count)];
                        Vector3 pos = r.GetRandomSpawnPosition((int)e.Size.x, (int)e.Size.y);
                        pos.y = 0;
                        GameObject eObj = GameObject.Instantiate(e.PrefabObject);
                        eObj.transform.position = pos;

                        // Add room setter ( used to know which room each enemy belongs to )
                        eObj.AddComponent<RoomReferer>().Reference = r;
                        
                        // Add reference to the collection resource
                        eObj.AddComponent<EnemyReferer>().Reference = e;
                        
                        // Add to the enemy list
                        this.enemies.Add(eObj);
                    }
                    

                }
            }
        }

        void AddBoss()
        {
            // Create boss object
            GameObject bossObj = GameObject.Instantiate(boss.PrefabObject);

            // Set boss in room
            BossPlacer bossPlacer = bossRoom.GetComponent<BossPlacer>();
            bossPlacer.Place(bossObj);

            // Add room setter ( used to know which room each enemy belongs to )
            bossObj.AddComponent<RoomReferer>().Reference = bossRoom.GetComponent<Room>();

            // Add reference to the collection resource
            bossObj.AddComponent<BossReferer>().Reference = boss;
        }

        void AddSpecialShips()
        {
            if (bank)
                AddBank();
        }

        void AddBank()
        {
            // Create bank object
            GameObject bankObj = GameObject.Instantiate(bank.PrefabObject);

            // Set bank in room ( the BossPlacer can be also used to place bank )
            BossPlacer bossPlacer = bankRoom.GetComponent<BossPlacer>();
            bossPlacer.Place(bankObj);

            // Add room setter ( used to know which room each enemy belongs to )
            bankObj.AddComponent<RoomReferer>().Reference = bankRoom.GetComponent<Room>();

            // Add reference to the collection resource
            bankObj.AddComponent<SpecialShipReferer>().Reference = bank;
        }

        void AddDroppables()
        {
            List<Droppable> all = LoadDroppableResources();

            // Add dropper to each enemy
            foreach(GameObject enemy in enemies)
            {
                EnemyReferer er = enemy.GetComponent<EnemyReferer>();
                DropperSetter ds = enemy.AddComponent<DropperSetter>();
                ds.Set(er.Reference.DroppingRate, all);
            }

            // Rooms

        }

        #endregion

        #region RESOURCES_LOADING
        List<GameObject> LoadPortalResources()
        {
            string res = "Portals/";

            List<GameObject> ret = new List<GameObject>(Resources.LoadAll<GameObject>(res));

            return ret;
        }

        List<Droppable> LoadDroppableResources()
        {
            List<Droppable> ret = new List<Droppable>(Resources.LoadAll<Droppable>(Droppable.ResourceFolder));
            return ret;
        }

        #endregion

        #region MISC
        void HandleOnTeleport(Portal portal)
        {
            Debug.Log("Teleporting:" + portal);
            // Disable all the object of the current room
            ActivateRoomObjects(portal.Room, false);
            Debug.Log("Deactivating objects on room:" + portal.Room);

            // Enable of the object of the new room
            ActivateRoomObjects(portal.TargetPortal.Room, true);
            currentRoom = portal.TargetPortal.Room;
            Debug.Log("Activating objects on room:" + portal.TargetPortal.Room);
        }

        void ActivateRoomObjects(Room room, bool value)
        {
            List<Referer<Room>> l = Referer<Room>.GetReferers(room) as List<Referer<Room>>;
            
            foreach (Referer<Room> rs in l)
            {
                Debug.Log("RoomReferer:"+rs.gameObject);
                    rs.gameObject.SetActive(value);
            }
                
        }

        #endregion

        #region INTERNAL_USE
        /**
         * Creates a room with no template ( ex. for labyrinth )
         * */
        private GameObject CreateRoom(RoomConfig config, System.Type roomType)
        {
            GameObject ret = new GameObject("Room_" + count);
            Room comp = ret.AddComponent(roomType) as Room;
            comp.Init(config);
            comp.Create();
            return ret;
        }

        /**
         * Creates a room from a given template ( ex. boss rooms )
         * */
        private GameObject CreateRoom(GameObject roomPrefab)
        {
            GameObject ret = GameObject.Instantiate(roomPrefab);
            Room r = ret.GetComponent<Room>();
            r.Create();
            return ret;
        }

        private GameObject CreateExplorationRoom()
        {
            int w = explorationRoomWallDistance + (explorationRoomWallDistance + 1) * Random.Range(minExplorationRoomWallsH, maxExplorationRoomWallsH + 1);// /*num of horizontal walls*/2;

            int h = explorationRoomWallDistance + (explorationRoomWallDistance + 1) * Random.Range(minExplorationRoomWallsV, maxExplorationRoomWallsV + 1);// */*num of vertical walls*/2;

            // Create Room object
            GameObject r = CreateRoom(new LabyrinthConfig() { RoomType = RoomType.Exploration, CorridorWidth = explorationRoomWallDistance, Width = w, Height = h, TileSize = tileSize }, typeof(Labyrinth));
            RoomEnemyData red = r.AddComponent<RoomEnemyData>();
            red.Init(new RoomEnemyDataConfig() { MinEnemyCount = 0, MaxEnemyCount = 3 });
            return r;
        }

        private GameObject CreateFightingRoom()
        {
            int w = fightingRoomWallDistance + (fightingRoomWallDistance + 1) * Random.Range(minFigthingRoomWallsH, maxFightingRoomWallsH + 1);// * 2;
            int h = fightingRoomWallDistance + (fightingRoomWallDistance + 1) * Random.Range(minFightingRoomWallsV, maxFightingRoomWallsV + 1);//*1;

            GameObject r = CreateRoom(new LabyrinthConfig() { RoomType = RoomType.Fighting, CorridorWidth = fightingRoomWallDistance, Width = w, Height = h, TileSize = tileSize }, typeof(Labyrinth));
            RoomEnemyData red = r.AddComponent<RoomEnemyData>();
            red.Init(new RoomEnemyDataConfig() { MinEnemyCount = 9, MaxEnemyCount = 13 });

            return r;
        }

        private GameObject CreateCustomRoom()
        {
            throw new System.Exception("Not implemented");
        }


        private GameObject CreateBossRoom()
        {

            // Get one of the available rooms for the current boss
            OMTB.Collections.Room roomRes = boss.Rooms[Random.Range(0, boss.Rooms.Count)];

            // Instanziate room
            GameObject room = CreateRoom(roomRes);

            room.name += "_Boss";

            return room;
        }

        private GameObject CreateStartingRoom()
        {
            // Get an available starting room ( we might even put the available list directly in the player settings )
            OMTB.Collections.Room roomRes = roomResources.Find(r => r.RoomType == RoomType.Starting);

            // Get the room prefab
            GameObject roomPrefab = roomRes.PrefabObject;

            // Instanziate the room
            GameObject room = CreateRoom(roomRes);

            //GameObject room = CreateRoom(new RoomConfig() {RoomType = RoomType.Boss, Width = 80, Height = 80, TileSize = 8 }, typeof(EmptyRoom));
            room.name += "_Starting";



            return room;
        }

        /** 
         * Creates a room given a room resource
         * */
        GameObject CreateRoom(OMTB.Collections.Room roomResource)
        {
            // Create the room
            GameObject room = CreateRoom(roomResource.PrefabObject);

            // Set the room type
            room.GetComponent<Room>().RoomType = roomResource.RoomType;

            return room;
        }

        private GameObject CreateBankRoom()
        {
            // Get room prefab

            OMTB.Collections.Room roomRes = bank.Rooms[Random.Range(0,bank.Rooms.Count)];
            
            // Instanziate room
            GameObject room = CreateRoom(roomRes);

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
