using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Level
{
    public class RoomSetter : MonoBehaviour
    {
        static List<RoomSetter> objects = new List<RoomSetter>();

        Room room;
        public Room Room
        {
            get { return room; }
            set { room = value; }
        }

        private void Awake()
        {
            objects.Add(this);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnDestroy()
        {
            objects.Remove(this);
        }

        public static List<RoomSetter> GetObjects(Room room)
        {
            return objects.FindAll(o => o.Room == room);
        }
    }

}
