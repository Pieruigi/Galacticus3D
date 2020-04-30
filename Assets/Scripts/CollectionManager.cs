using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collections;

namespace OMTB
{
    public class CollectionManager
    {
        const string basePath = "Assets/Resources/";
        const string droppablesFolder = "Droppables";

        public const string DroppablesResourcePath = basePath + "Droppables" + "/";

        private List<Droppable> droppables;

        static CollectionManager instance;
        public static CollectionManager Instance 
        { 
            get { return GetCollectionManager(); }
        }

        CollectionManager()
        {

            droppables = new List<Droppable>(Resources.LoadAll<Droppable>(droppablesFolder)); 
            
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

}
