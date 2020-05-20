using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class RoomAllocatorFactory
    {
        Dictionary<System.Type, System.Type> allocators;

        static RoomAllocatorFactory instance;
        public static RoomAllocatorFactory Instance
        {
            get { if(instance == null) Create(); return instance; }
            
        }

        public System.Type GetRoomAllocator(System.Type roomType)
        {
            if (!allocators.ContainsKey(roomType))
                return null;

            return allocators[roomType];
        }

        RoomAllocatorFactory() 
        {
            allocators = new Dictionary<System.Type, System.Type>();
            allocators.Add(typeof(Labyrinth) , typeof(LabyrinthAllocator));
            allocators.Add(typeof(CustomRoom), typeof(CustomRoomAllocator));
            allocators.Add(typeof(BossRoom), typeof(BossRoomAllocator));
            allocators.Add(typeof(BankRoom), typeof(BankRoomAllocator));
        }

        static void Create()
        {
            instance = new RoomAllocatorFactory();

        }
    }

}
