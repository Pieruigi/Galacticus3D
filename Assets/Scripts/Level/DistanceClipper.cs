using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Level
{
    public class DistanceClipper : MonoBehaviour
    {
        GameObject player;

        List<Transform> children = new List<Transform>();

        float w, h = 140;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            for(int i=0; i<transform.childCount; i++)
            {
                children.Add(transform.GetChild(i));
            }
            

            w = h * Screen.width / Screen.height;
            w = w / 2f;
            h = h / 2f;
            
        }

        // Update is called once per frame
        void Update()
        {

        }


        private void LateUpdate()
        {
            Vector3 pos = player.transform.position;

            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].position.x > pos.x - w && children[i].position.x < pos.x + w &&
                   children[i].position.z > pos.z - h && children[i].position.z < pos.z + h)
                {
                    if (!children[i].gameObject.activeSelf)
                        children[i].gameObject.SetActive(true);
                }
                else
                {
                    if (children[i].gameObject.activeSelf)
                        children[i].gameObject.SetActive(false);
                }



            }

  
            
        }

 
    }
}


