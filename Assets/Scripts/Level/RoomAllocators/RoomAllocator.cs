using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public abstract class RoomAllocator : MonoBehaviour
    {
        Room room;
        public Room Room
        {
            get { return room; }
            set { room = value; }
        }

        public abstract void Allocate();

        protected virtual void Awake()
        {

        }

        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Update is called once per frame






    }

}
