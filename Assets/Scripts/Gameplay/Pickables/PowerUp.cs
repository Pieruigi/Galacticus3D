using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;
using OMTB.Collections;

namespace OMTB.Gameplay
{
    public class PowerUpData
    {
    }

    public abstract class PowerUp : MonoBehaviour, IPickable
    {
        [SerializeField]
        Droppable droppable;
        public Droppable Droppable
        {
            get { return droppable; }
        }

        
        // Not all the powerups need a controller; for example the shield needs the controller, instead the speedup don't
        PowerUpController controller;
        protected Component Controller
        {
            get { return controller; }
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

        public virtual PowerUpData GetData()
        {
            return new PowerUpData();
        }

        public virtual void SetData(PowerUpData data) 
        {

        }

        public bool TryPickUp()
        {
            return PowerUpManager.Instance.TryActivate(this);
        }


        protected void CreateController(System.Type type)
        {
#if UNITY_EDITOR
            if (Level.LevelManager.Instance == null)
                controller = GameObject.FindGameObjectWithTag("Player").AddComponent(type) as PowerUpController;
            else
                controller = Level.LevelManager.Instance.Player.AddComponent(type) as PowerUpController;
#else
            controller = Level.LevelManager.Instance.Player.AddComponent(type) as PowerUpController;
#endif
            controller.Init(this);
        }

        protected void DestroyController()
        {
            Destroy(controller);
        }


    }

}
