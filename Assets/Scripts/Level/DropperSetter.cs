using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collections;

namespace OMTB.Level
{
    public class DropperSetter : MonoBehaviour
    {
        //[SerializeField]
        //[Range(0f,1f)]
        //float droppingRate;

        [SerializeField]
        List<Droppable> droppables;

        private void Awake()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Set(float rate, List<Droppable> droppables)
        {
            float r = Random.Range(0f, 1f);
            if (r == 0 || r > rate)
            {
                Destroy(this);
                return;
            }
                

            List<Droppable> all = new List<Droppable>();
            foreach (Droppable d in droppables)
            {
                for (int i = 0; i < (int)d.Rarity; i++)
                    all.Add(d);
            }

            Droppable droppable = all[Random.Range(0, all.Count)];
            Debug.Log(string.Format("Enemy {0} will drop {1}", gameObject.name, droppable.name));

            OMTB.Gameplay.Dropper dropper = gameObject.AddComponent<OMTB.Gameplay.Dropper>();
            dropper.DroppablePrefab = droppable.Prefab;

            Destroy(this);

        }
    }

}
