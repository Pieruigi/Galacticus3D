using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collections;

namespace OMTB
{
    public class CollectionManager
    {
        
        public const string DroppablesFolder = "Droppables";

        

        private List<Droppable> droppables;

        static CollectionManager instance;
        public static CollectionManager Instance 
        { 
            get { return GetCollectionManager(); }
        }

        CollectionManager()
        {

            droppables = new List<Droppable>(Resources.LoadAll<Droppable>(DroppablesFolder)); 
            
        }

        static void Create()
        {
            instance = new CollectionManager();
        }

        public IList<Droppable> GetDroppables()
        {
            return droppables.AsReadOnly();
        }

        private static CollectionManager GetCollectionManager()
        {
            if (instance == null)
                Create();

           return instance;



        }
    }

    //public class Collection<T>
    //{
    //    private List<T> objects;

    //    static Collection<T> instance;

        
    //    public static Collection<T> Instance
    //    {
    //        get { return GetCollection(); }
    //    }

    //    private static Collection<T> GetCollection()
    //    {
    //        if (instance == null)
    //            new Collection<T>();

    //        return instance;

    //    }
    //}


}
