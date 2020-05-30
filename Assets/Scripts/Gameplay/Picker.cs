﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public class Picker : MonoBehaviour
    {

#if UNITY_EDITOR
        [SerializeField]
        GameObject testContentPrefab;
#endif

        bool noPicking = false;

        GameObject content;


        float startDelay = 0;
        public float StartDelay
        {
            get { return startDelay; }
            set { startDelay = value; }
        }

        private void Awake()
        {
            noPicking = true;
        }

        // Start is called before the first frame update
        void Start()
        {
#if UNITY_EDITOR
            SetContent(testContentPrefab);
#endif
        }

        // Update is called once per frame
        void Update()
        {
          
        }

        public void SetContent(GameObject contentPrefab)
        {
            
            GameObject g = GameObject.Instantiate(contentPrefab);
            g.transform.parent = transform;
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;
            g.transform.localScale = Vector3.zero;
            content = g;

            LeanTween.scale(content, Vector3.one, 1f).setEaseInOutElastic();
            noPicking = false;

            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (noPicking)
                return;

            if ("Player".Equals(other.tag))
            {
                Debug.Log("Picked");
                if (content.GetComponentInChildren<IPickable>().TryPickUp())
                {
                    noPicking = true;
                    LeanTween.scale(content, Vector3.zero, 1f).setEaseInOutElastic();
                    GameObject.Destroy(gameObject,1);
                }
                    
            }
        }


       
    }

}
