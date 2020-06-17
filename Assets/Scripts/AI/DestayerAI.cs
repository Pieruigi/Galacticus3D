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

        float tieFieMinRate = 15;
        float tieFieMaxRate = 40;
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

            // deactivate to prevent 'agent is not on navhmesh' error
            g.SetActive(false);

            // Add dropper
            if (g.GetComponent<RoomReferer>())
            {
                DropperSetter dropperSetter = g.AddComponent<DropperSetter>();
                dropperSetter.Set(tieFieResource.DroppingRate, droppables);
            }

            // Get the hangar depending on the player position 
            Vector3 v = targetSetter.Target.position - transform.position;
            Transform t;
            if (Vector3.Dot(v, transform.right) > 0) // Right hangar
                t = rightHangar;
            else
                t = leftHangar;

            // Set position and rotation
            g.transform.position = t.position;
            Debug.Log("GPos:" + g.transform.position);
            g.transform.forward = t.forward;
            g.GetComponentInChildren<OMTB.Utils.HeightAlligner>().SetHeight(-3f);
            
            // handle tiefie destruction event
            g.GetComponent<IDamageable>().OnDie += HandleOnTieFieDie;

            // add tiefie to the list
            tieFies.Add(g);

            // activate the tiefie
            g.SetActive(true);

        }

        void HandleOnTieFieDie(IDamageable damageable)
        {
            tieFies.RemoveAll(t => t.GetComponent<IDamageable>() == damageable);
        }

        
    }
}


