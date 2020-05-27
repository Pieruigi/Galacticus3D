using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;
using OMTB.Level;

namespace OMTB.Gameplay
{
    /**
     * Used on entity that must drop something when they are destroyed ( ex. enemies )
     * */
    public class Dropper : MonoBehaviour
    {
        [SerializeField]
        GameObject droppablePrefab;
        public GameObject DroppablePrefab
        {
            get { return droppablePrefab; }
            set { droppablePrefab = value; }
        }


        private void Awake()
        {
            GetComponent<IDamageable>().OnDestroy += HandleOnDestroy;    
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void Drop()
        {
            // Load picker
            Picker[] res = Resources.LoadAll<Picker>("Misc");
            
            GameObject picker = GameObject.Instantiate(res[0].gameObject);

            // Dropper is used on AI, so I'm sure RoomReference component exists
            picker.AddComponent<RoomReferer>().Reference = GetComponent<RoomReferer>().Reference;

            picker.transform.position = transform.position;
            picker.transform.rotation = Quaternion.identity;

            // Set droppable
            picker.GetComponent<Picker>().StartDelay = 5; // A bit of delay
            picker.GetComponent<Picker>().SetContent(droppablePrefab);
               
            
        }

        void HandleOnDestroy(IDamageable damageable)
        {
            Drop();
        }


    }

}
