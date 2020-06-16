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

        [SerializeField]
        Transform leftHangar;

        [SerializeField]
        Transform rightHangar;

        float tieFieMinRate = 30;
        float tieFieMaxRate = 60;
        float tieFieCurrRate;
        DateTime lastTieFie;
        float tieFieMaxNumber = 3;

        List<Weapon> cannons;

        int cannonCount;

        TargetSetter targetSetter;

        List<Droppable> droppables;

        List<GameObject> tieFies = new List<GameObject>();

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
                if(tieFies.Count < tieFieMaxNumber)
                    SpawnTieFie();
            }
        }

        void SpawnTieFie()
        {
            // Update timer
            lastTieFie = DateTime.UtcNow;
            tieFieCurrRate = UnityEngine.Random.Range(tieFieMinRate, tieFieMaxRate);

            // Create TieFie
            GameObject g = GameObject.Instantiate(tieFieResource.PrefabObject);
            g.SetActive(false);
            // Add dropper
            //DropperSetter dropperSetter = g.AddComponent<DropperSetter>();
            //dropperSetter.Set(tieFieResource.DroppingRate, droppables);

            
           
            // Set spawn position
            Vector3 dir = targetSetter.Target.position - transform.position;
            dir.Normalize();
            Debug.Log("Dir:" + dir);

            float x = UnityEngine.Random.Range(12f, 42f);
            Vector3 pos = transform.position + dir * x;
            pos.y = 0;
            pos.z = targetSetter.Target.position.z;
            pos.z += (UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1) * 50f;

            Transform t;
            RaycastHit hit;
            if (OMTB.Utils.AIUtil.HitObstacle(leftHangar.transform.position, -transform.right, 100f, out hit, true))
                t = rightHangar;
            else
                if (OMTB.Utils.AIUtil.HitObstacle(rightHangar.transform.position, transform.right, 100f, out hit, true))
                    t = leftHangar;
            else
                t = UnityEngine.Random.Range(0, 2) == 0 ? leftHangar : rightHangar;

            g.transform.position = t.position;
            g.transform.forward = t.forward;
            
            Gameplay.ShipLauncher sm = g.AddComponent<Gameplay.ShipLauncher>();
            sm.TargetPosition = pos;

            g.GetComponent<IDamageable>().OnDie += HandleOnTieFieDie;
            tieFies.Add(g);

            g.SetActive(true);


        }

        void HandleOnTieFieDie(IDamageable damageable)
        {
            tieFies.RemoveAll(t => t.GetComponent<IDamageable>() == damageable);
        }

        
    }
}


