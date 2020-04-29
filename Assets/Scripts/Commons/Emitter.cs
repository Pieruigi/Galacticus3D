using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class Emitter : MonoBehaviour
    {
        [SerializeField]
        bool isEnemy = false;
        public bool IsEnemy
        {
            get { return isEnemy; }
            set { isEnemy = value; }
        }

        // Start is called before the first frame update
        void Start()
        {
            Color c = Color.blue;
            if (isEnemy)
                c = Color.red;

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
