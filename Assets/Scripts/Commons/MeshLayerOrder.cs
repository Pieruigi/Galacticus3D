using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class MeshLayerOrder : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            MeshRenderer rend = GetComponent<MeshRenderer>();
            rend.sortingLayerID = 1;
            rend.sortingOrder = 10;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

