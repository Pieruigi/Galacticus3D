using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class Optimizer : MonoBehaviour
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
                Rotator[] rotators = child.GetComponentsInChildren<Rotator>();
                Debug.Log("Rotators.Length:" + rotators.Length);
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
                if(walls[i].position.x > pos.x - w && walls[i].position.x < pos.x + w &&
                   walls[i].position.z > pos.z - h && walls[i].position.z < pos.z + h)
                {
                    if (!walls[i].GetChild(0).gameObject.activeSelf)
                        walls[i].GetChild(0).gameObject.SetActive(true);
                }
                else
                {
                    if (walls[i].GetChild(0).gameObject.activeSelf)
                        walls[i].GetChild(0).gameObject.SetActive(false);
                }

                

            }
            
        }
    }
}


