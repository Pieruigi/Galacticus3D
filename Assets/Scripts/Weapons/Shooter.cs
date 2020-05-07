using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public abstract class Shooter : MonoBehaviour
    {
        [SerializeField]
        Transform owner;
        public Transform Owner
        {
            get { return owner; }
        }

        Weapon weapon;
        public Weapon Weapon
        {
            get { return weapon; }
            set { weapon = value; }
        }

        public abstract void Shoot();

        // Start is called before the first frame update
        protected virtual void Start()
        {
            if(!owner)
                owner = transform.root;

            if (weapon == null)
                weapon = GetComponent<Weapon>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }


}
