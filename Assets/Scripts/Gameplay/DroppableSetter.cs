using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collections;

namespace OMTB.Level
{
    public class DroppableSetter : MonoBehaviour
    {
       
        [SerializeField]
        [Range(0,1)]
        float dropChance;

        [SerializeField]
        bool byScene;

        [SerializeField]
        bool bySpaceStation;

        [SerializeField]
        bool byCommonEnemy; // True if you want this to release common enemies powerup

        [SerializeField]
        OMTB.Collections.Droppable.SpecialEnemyGroupType specialEnemyGroup;

        // Start is called before the first frame update
        void Start()
        {

            List<Droppable> list = new List<Droppable>(Resources.LoadAll<OMTB.Collections.Droppable>(OMTB.Collections.Droppable.ResourceFolder)).
                FindAll(d => d.ByScene == byScene &&
                        d.BySpaceStation == bySpaceStation &&
                        (byCommonEnemy && d.SpecialEnemyGroup == Droppable.SpecialEnemyGroupType.None) &&
                        d.MinLevel <= LevelManager.Instance.Level &&
                        (d.MaxLevel < 0 || d.MaxLevel >= LevelManager.Instance.Level) &&
                        (d.SpecialEnemyGroup == Droppable.SpecialEnemyGroupType.None || d.SpecialEnemyGroup == specialEnemyGroup)
                        );

            Debug.Log("LIst:" + list.Count);
            Droppable ret;
            if (TryGetRandom(list, out ret))
            {
                Debug.Log("Ret:" + ret.name);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                List<Droppable> list = new List<Droppable>(Resources.LoadAll<OMTB.Collections.Droppable>(OMTB.Collections.Droppable.ResourceFolder)).
                 FindAll(d => d.ByScene == byScene &&
                         d.BySpaceStation == bySpaceStation &&
                         (byCommonEnemy && d.SpecialEnemyGroup == Droppable.SpecialEnemyGroupType.None) &&
                         d.MinLevel <= LevelManager.Instance.Level &&
                         (d.MaxLevel < 0 || d.MaxLevel >= LevelManager.Instance.Level) &&
                         (d.SpecialEnemyGroup == Droppable.SpecialEnemyGroupType.None || d.SpecialEnemyGroup == specialEnemyGroup)
                         );

                Droppable ret;
                if (TryGetRandom(list, out ret))
                {
                    Debug.Log("Ret:" + ret.name);
                }
            }
        }

        public bool TryGetRandom(List<Droppable> list, out Droppable ret)
        {
            ret = null;
            // Check the chance to drop something
            float r = 0;

            if (dropChance < 1)
            {
                r = Random.Range(0f, 1f);
                if (r > dropChance)
                    return false;
            }

            // Ok, drop something
            Debug.Log("Drop something");

            // Choose the object to be dropped
            r = Random.Range(0f, 1f);
            Debug.Log("r:" + r);

            // Get all the object with the minimum weight
            List<Droppable> l = list.FindAll(d => d.DropChance >= r);

            if (l.Count == 0)
            {
                Debug.Log("Nothing to drop");
                return false;
            }

            ret = l[Random.Range(0, l.Count)];
          
            return true;
            //GameObject picker = GameObject.Instantiate(pickerPrefab);
            //picker.transform.position = position;
            //picker.transform.rotation = Quaternion.identity;
            //picker.GetComponent<Picker>().AddContent(toDrop.PickerPrefab);
        }
    }
}


