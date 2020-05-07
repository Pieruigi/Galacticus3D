using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class Emitter : MonoBehaviour
    {
        [SerializeField]
        Transform owner;
        public Transform Owner
        {
            get { return owner; }
            protected set { owner = value; }
        }

        //[SerializeField]
        bool isEnemy = false;
        //public bool IsEnemy
        //{
        //    get { return isEnemy; }
        //    set { isEnemy = value; }
        //}

        // Start is called before the first frame update
        protected virtual void Start()
        {
            Color c = Color.cyan;
            if ("Enemy".Equals(owner.tag))
            {
                c = Color.red;
                isEnemy = true;
            }
                

            if (isEnemy)
            {
                GetComponent<MeshRenderer>().sharedMaterial.SetColor("_EmissionColor", c * 2);
            }
            else
            {
                GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", c * 2);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
