using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collections;

namespace OMTB.Level
{
    public class DropperSetter : MonoBehaviour
    {
        [SerializeField]
        [Range(0f,1f)]
        float droppingRate;

        [SerializeField]
        List<DroppingData> commonDroppables;

        private void Awake()
        {
            float r = Random.Range(0f, 1f);
            if (r == 0 || r > droppingRate)
                return;
            
            List<GameObject> all = new List<GameObject>();
            foreach(DroppingData dd in commonDroppables)
            {
                for (int i = 0; i < dd.Weight; i++)
                    all.Add(dd.Droppable);
            }

            GameObject d = all[Random.Range(0, all.Count)];
            Debug.Log(string.Format("Enemy {0} will drop {1}", gameObject.name, d.name));
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
