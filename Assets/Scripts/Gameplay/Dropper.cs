using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collections;

namespace OMTB.Gameplay
{
    public class Dropper : MonoBehaviour
    {
        [SerializeField]
        [Range(0f,1f)]
        float dropChance = 1;

        List<Droppable> droppables;
        // Start is called before the first frame update
        void Start()
        {
            droppables = new List<Droppable>(CollectionManager.Instance.GetDroppables());
            Debug.Log("Droppables.Count:" + droppables.Count);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
                TryDropSomething();
        }

        public void TryDropSomething()
        {
            // Check the chance to drop something
            float r = Random.Range(0f, 1f);

            if (r > dropChance)
                return;
    // Ok, drop something
            Debug.Log("Drop something");

            // Choose the object to be dropped
            r = Random.Range(0f, 1f);

            // Get the object with the minimum weight
            Droppable dMin = null;
            float wMin = 0;

            foreach(Droppable d in droppables)
            {
                if (d.DropChance > r)
                    continue;

                if((dMin == null) || (wMin > d.DropChance))
                {
                    dMin = d;
                    wMin = d.DropChance;
                }
                    
            }

                
            if(dMin == null)
            {
                Debug.Log("Nothing to drop");
                return;
            }

            // Get all the object with the minimum weight
            List<Droppable> l = droppables.FindAll(d => d.DropChance == wMin);

            dMin = l[Random.Range(0, l.Count)];
            Debug.Log("Dropping " + dMin.name);
            
        }
    }

}
