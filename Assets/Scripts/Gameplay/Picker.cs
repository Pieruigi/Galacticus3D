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

        float startDelay = 0;
        public float StartDelay
        {
            get { return startDelay; }
            set { startDelay = value; }
        }

        bool starting = false;

        private void Awake()
        {
            noPicking = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (starting)
            {
                startDelay -= Time.deltaTime;
                if(startDelay < 0)
                {
                    LeanTween.scale(content, Vector3.one, 1f).setEaseInOutElastic();
                    noPicking = false;
                    starting = false;
                }
            }
        }

        public void SetContent(GameObject contentPrefab)
        {
            GameObject g = GameObject.Instantiate(contentPrefab);
            g.transform.parent = transform;
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;
            g.transform.localScale = Vector3.zero;
            content = g;

            if(startDelay == 0)
            {
                LeanTween.scale(content, Vector3.one, 1f).setEaseInOutElastic();
                noPicking = false;
            }
            else
            {
                starting = true;
            }

            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (noPicking || starting)
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
