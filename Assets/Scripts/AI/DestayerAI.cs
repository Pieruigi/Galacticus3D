using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;
using System;
using OMTB.Collections;
using OMTB.Level;

namespace OMTB.AI
{
    public class DestayerAI : MonoBehaviour
    {
        [SerializeField]
        Enemy tieFieResource;

        float tieFieMinRate = 12;
        float tieFieMaxRate = 24;
        float tieFieCurrRate;
        DateTime lastTieFie;

        List<Weapon> cannons;

        int cannonCount;

        TargetSetter targetSetter;

        List<Droppable> droppables;   

        // Start is called before the first frame update
        void Start()
        {
            droppables = new List<Droppable>(Resources.LoadAll<Droppable>(Droppable.ResourceFolder));
            targetSetter = GetComponent<TargetSetter>();

            cannons = new List<Weapon>(GetComponentsInChildren<Weapon>());
            cannonCount = cannons.Count;

            IActivable[] activables = GetComponentsInChildren<IActivable>();
           
            foreach (IActivable activable in activables)
                activable.Activate();

            IDamageable[] damageables = GetComponentsInChildren<IDamageable>();
            foreach(IDamageable damageable in damageables)
            {
                damageable.OnDie += HandleOnDie;
            }

            // Set the first tie fie spawn time
            //lastTieFie = DateTime.UtcNow;
            tieFieCurrRate = UnityEngine.Random.Range(tieFieMinRate, tieFieMaxRate);
            
        }


        void ShipDestroy()
        {
            Destroy(gameObject);
        }

        void HandleOnDie(IDamageable damageable)
        {
            cannonCount--;
            if (cannonCount == 0)
                ShipDestroy(); 
            
            
            
        }

        private void Update()
        {
            if( (DateTime.UtcNow - lastTieFie).TotalSeconds > tieFieCurrRate)
            {
                SpawnTieFie();
            }
        }

        void SpawnTieFie()
        {
            // Create TieFie
            GameObject g = GameObject.Instantiate(tieFieResource.PrefabObject);

            // Add dropper
            //DropperSetter dropperSetter = g.AddComponent<DropperSetter>();
            //dropperSetter.Set(tieFieResource.DroppingRate, droppables);

            IActivable[] activables = g.GetComponentsInChildren<IActivable>();
            foreach(IActivable activable in activables)
                activable.Activate();
           
            // Set spawn position
            Vector3 dir = targetSetter.Target.position - transform.position;
            dir.Normalize();
            Debug.Log("Dir:" + dir);

            float x = UnityEngine.Random.Range(12f, 42f);
            Vector3 pos = transform.position + dir * x;
            pos.y = 0;
            pos.z = targetSetter.Target.position.z;
            pos.z += (UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1) * 50f;
            g.transform.position = pos;

            // Update timer
            lastTieFie = DateTime.UtcNow;
            tieFieCurrRate = UnityEngine.Random.Range(tieFieMinRate, tieFieMaxRate);
        }
    }
}


