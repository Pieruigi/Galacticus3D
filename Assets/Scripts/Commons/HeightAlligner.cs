using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Utils
{
    public class HeightAlligner : MonoBehaviour
    {
        [SerializeField]
        float speed = 10;

        float heightDefault;
        float heightCurrent;
        
        private void Awake()
        {
            heightDefault = transform.position.y;
            heightCurrent = heightDefault; 
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //if (heightDefault == heightCurrent)
            //    return;

            SetHeight(Mathf.MoveTowards(heightCurrent, heightDefault, speed * Time.deltaTime));

        }

        public void SetHeight(float value)
        {
            Vector3 pos = transform.localPosition;
            pos.y = value;
            transform.localPosition = pos;
            heightCurrent = pos.y;
        }
    }

}
