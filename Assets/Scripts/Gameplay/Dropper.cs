using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace OMTB.Gameplay
{
    public class Dropper : MonoBehaviour
    {
        [SerializeField]
        [Range(0f,1f)]
        float dropChance = 1;

        [SerializeField]
        List<Droppable> droppables;

        [SerializeField]
        GameObject pickerPrefab;

        // Start is called before the first frame update
        void Start()
        {
            if(droppables == null)
                droppables = new List<Droppable>(CollectionManager.Instance.GetDroppables());
            Debug.Log("Droppables.Count:" + droppables.Count);

            //Collection<string>
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void TryGetRandomDroppable()
        {
            // Check the chance to drop something
            float r = 0;
            
            if(dropChance < 1)
            {
                r = Random.Range(0f, 1f);
                if (r > dropChance)
                    return;
            }

            // Ok, drop something
            Debug.Log("Drop something");

            // Choose the object to be dropped
            r = Random.Range(0f, 1f);
            Debug.Log("r:" + r);

            // Get all the object with the minimum weight
            List<Droppable> l = droppables.FindAll(d => d.DropChance >= r);

            if(l.Count == 0)
            {
                Debug.Log("Nothing to drop");
                return;
            }

            Droppable toDrop = l[Random.Range(0, l.Count)];
            Debug.Log("Dropping " + toDrop.name);

            //GameObject picker = GameObject.Instantiate(pickerPrefab);
            //picker.transform.position = position;
            //picker.transform.rotation = Quaternion.identity;
            //picker.GetComponent<Picker>().AddContent(toDrop.PickerPrefab);
        }
    }

}
