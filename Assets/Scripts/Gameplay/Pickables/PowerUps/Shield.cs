using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.Gameplay
{
    public class ShieldData: PowerUpData
    {
        public float Damage { get; set; }

    }

    public class Shield : PowerUp
    {
        [SerializeField]
        float damage;
        public float Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        [SerializeField]
        GameObject prefab;
        public GameObject Prefab
        {
            get { return prefab; }
        }

    
        //ShieldController controller;

        

        public override void Activate()
        {
  
            if (damage == 0)
                return;

            CreateController(typeof(ShieldController));
        }

        public override void Deactivate()
        {
            DestroyController();
        }

        public override PowerUpData GetData()
        {
            ShieldData data = new ShieldData();
            data.Damage = damage;
            return data;
        }

        public override void SetData(PowerUpData data)
        {
            damage = (data as ShieldData).Damage;
        }
    }

}
