using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Level
{
    public class BossPlacer : MonoBehaviour
    {
        [SerializeField]
        List<Transform> bossSpawns;

        // Start is called before the first frame update
        void Start()
        {
            //Clear();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Place(GameObject boss)
        {
            if(bossSpawns.Count == 0)
                throw new System.Exception("No place for boss.");
            
            // Get empty object
            Transform spawn = bossSpawns[Random.Range(0, bossSpawns.Count)];

            // Remove empty from list
            bossSpawns.Remove(spawn);

            // Set boss position and rotation
            boss.transform.position = spawn.position;
            boss.transform.rotation = spawn.rotation;

            // Destroy empty
            //Destroy(spawn.gameObject);
        }

        //void Clear()
        //{
        //    // Remove unused portal spawns
        //    int count = bossSpawns.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        Destroy(bossSpawns[0].gameObject);
        //    }
        //    bossSpawns.Clear();
        //}
    }

}
