using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB
{
    [RequireComponent(typeof(Roller))]
    public class RolleableSetter : MonoBehaviour
    {
        Transform root;
        List<Transform> rolleables;

        Transform currentRolleable;

        Roller roller;

        // Start is called before the first frame update
        void Start()
        {
            root = transform.root;
            rolleables = new List<Transform>(root.GetComponentsInChildren<Transform>()).FindAll(r=>r.GetComponent<IActivable>()!=null && r.GetComponent<IRolleable>() != null);
            currentRolleable = rolleables.Find(r => r.GetComponent<IActivable>().IsActive());
            roller = GetComponent<Roller>();
            if (currentRolleable)
                roller.Rolleable = currentRolleable.GetComponent<IRolleable>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (currentRolleable == null || !currentRolleable.GetComponent<IActivable>().IsActive())
            {
                currentRolleable = rolleables.Find(r => r.GetComponent<IActivable>().IsActive());
                if(currentRolleable)
                    roller.Rolleable = currentRolleable.GetComponent<IRolleable>();
            }
                
        }
    }

}
