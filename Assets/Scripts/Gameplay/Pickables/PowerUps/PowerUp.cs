using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;
using OMTB.Collections;

namespace OMTB.Gameplay
{
    public abstract class PowerUp : MonoBehaviour, IPickable
    {
        [SerializeField]
        Droppable droppable;
        public Droppable Droppable
        {
            get { return droppable; }
        }

        public abstract void Activate();

        public abstract void Deactivate();

        
        protected virtual void Awake()
        {
            
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        // Update is called once per frame
        protected virtual void Update()
        {

        }

        public bool TryPickUp()
        {
            return PowerUpManager.Instance.TryActivate(this);
        }

        
    }

}
