using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class Optimizer : MonoBehaviour
    {
        GameObject player;

        float range = 80;
        float sqrRange;

        Transform[] walls;

        // Start is called before the first frame update
        void Start()
        {
            sqrRange = range * range;

            player = GameObject.FindGameObjectWithTag("Player");
            walls = new Transform[transform.childCount];
            
            for(int i=0; i<transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                walls[i] = child;

                // Remove random particle systems ( corresponding light must be inside )
                bool remove = ((int)Random.Range(0,3) == 0) ? false : true;
                if (remove)
                {
                    Destroy(child.GetComponentInChildren<ParticleSystem>());
                }


                // Set some mesh static
                Rotator[] rotators = child.GetChild(0).GetComponentsInChildren<Rotator>();
                for(int j=0; j<rotators.Length; j++)
                {
                    remove = ((int)Random.Range(0, 5) == 0) ? false : true;
                    if (remove)
                    {
                        Destroy(rotators[j]);
                    }
                }
                
                
            }


        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            for(int i=0; i<walls.Length; i++)
            {
                if ((player.transform.position - walls[i].position).sqrMagnitude > sqrRange)
                {
                    if (walls[i].gameObject.activeSelf)
                        walls[i].gameObject.SetActive(false);

                }
                else
                {
                    if (!walls[i].gameObject.activeSelf)
                        walls[i].gameObject.SetActive(true);
                }
                

            }
            
        }
    }
}


