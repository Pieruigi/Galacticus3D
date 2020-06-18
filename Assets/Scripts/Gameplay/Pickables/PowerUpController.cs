using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Gameplay
{
    public abstract class PowerUpController : MonoBehaviour
    {
        public abstract void Init(PowerUp powerUp);

        protected virtual void Awake()
        {

        }

        // Start is called before the first frame update
        protected virtual void Start()
        {

        }

        protected virtual void Update() { }
        
    }

}
