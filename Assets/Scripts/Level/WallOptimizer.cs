using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Level
{
    public class WallOptimizer : MonoBehaviour
    {
        GameObject player;

        float sqrRange;

        Transform[] walls;

        float w, h = 80;


        // Start is called before the first frame update
        void Start()
        {
            w = h * Screen.width / Screen.height;
            w = w / 2f;
            h = h / 2f;
            Debug.Log("W:" + w);
            Debug.Log("H:" + h);

            player = GameObject.FindGameObjectWithTag("Player");
            walls = new Transform[transform.childCount];
            
            for(int i=0; i<transform.childCount; i++)
            {
                Transform child = transform.GetChild(i); 
                walls[i] = child;

                bool remove = false;
                // Remove random particle systems ( corresponding light must be inside )
                ParticleSystem[] psList = child.GetComponentsInChildren<ParticleSystem>();
              
                for(int j=0; j<psList.Length; j++)
                {
                    remove = ((int)Random.Range(0, 4) == 0) ? false : true;
                    if (remove)
                    {
                        Destroy(psList[j].gameObject);
                    }
                }
                


                // Set some mesh static
                RandomRotator[] rotators = child.GetComponentsInChildren<RandomRotator>();

                for(int j=0; j<rotators.Length; j++)
                {
                    remove = ((int)Random.Range(0, 4) == 0) ? false : true;
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
            Vector3 pos = player.transform.position;

            for(int i=0; i<walls.Length; i++)
            {

                for(int j=0; j<walls[i].childCount; j++)
                {
                    Transform child = walls[i].GetChild(j);
                    if (child.position.x > pos.x - w && child.position.x < pos.x + w && child.position.z > pos.z - h && child.position.z < pos.z + h)
                    {
                        if (!child.gameObject.activeSelf)
                            child.gameObject.SetActive(true);
                    }
                    else
                    {
                        if (child.gameObject.activeSelf)
                            child.gameObject.SetActive(false);
                    }

                }


            }

        }

 
    }
}


