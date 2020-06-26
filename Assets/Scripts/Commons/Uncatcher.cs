using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class Uncatcher : MonoBehaviour
    {
        Catcher catcher;
        public Catcher Catcher
        {
            get { return catcher; }
            set { catcher = value; }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                catcher.Uncatch();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (Tags.Wall.Equals(collision.gameObject.tag) || Tags.Enemy.Equals(collision.gameObject.tag))
            {
                catcher.Uncatch();
            }
        }
    }

}
