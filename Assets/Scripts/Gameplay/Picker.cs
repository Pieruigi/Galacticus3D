using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public class Picker : MonoBehaviour
    {
    
        bool noPicking = false;

        GameObject content;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Init(GameObject contentPrefab)
        {
            GameObject g = GameObject.Instantiate(contentPrefab);
            g.transform.parent = transform;
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;
            g.transform.localScale = Vector3.zero;
            content = g;

            LeanTween.scale(content, Vector3.one, 1f).setEaseInOutElastic();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (noPicking)
                return;

            if("Player".Equals(other.tag))
            {
                Debug.Log("Picked");
                if (content.GetComponent<IPickable>().TryPickUp(other.gameObject))
                {
                    noPicking = true;
                    LeanTween.scale(content, Vector3.zero, 1f).setEaseInOutElastic();
                    GameObject.Destroy(gameObject,1);
                }
                    
            }
        }

       
    }

}
