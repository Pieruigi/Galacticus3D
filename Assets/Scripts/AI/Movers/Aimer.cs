using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;
using UnityEngine.AI;

namespace OMTB.AI
{
    [RequireComponent(typeof(TargetSetter))]
    public class Aimer : MonoBehaviour, IActivable, IFreezable
    {
        
        [SerializeField]
        float angularSpeed;
        float angularSpeedDefault;

        [SerializeField]
        Transform root;


        bool isActive = false;

        Vector3 targetPos;
        TargetSetter targetSetter;
        float sqrAvgDistance;
       
      
        private void Awake()
        {
            angularSpeedDefault = angularSpeed;
            Deactivate();
        }

        // Start is called before the first frame update
        void Start()
        {
            if(!root)
                root = transform.root;
            
            targetSetter = GetComponent<TargetSetter>();

            RandomizeValues();

        }

        // Update is called once per frame
        void Update()
        {

            if (!isActive)
                return;

            // Aim player
            Quaternion targetRot = Quaternion.LookRotation((targetSetter.Target.position - root.transform.position ).normalized);
            Quaternion rot = Quaternion.RotateTowards(root.transform.rotation, targetRot, angularSpeed * Time.deltaTime);
            root.rotation = rot;
            

        }

       

        public void Activate()
        {
            isActive = true;
            
            Reset();
        }

        public void Deactivate()
        {
            isActive = false;
            Reset();
        }

        void Reset()
        {
      
        }

     
        void RandomizeValues()
        {
            angularSpeed = Random.Range(angularSpeedDefault * 0.8f, angularSpeedDefault * 1.2f);
        }



        public bool IsActive()
        {
            return isActive;
        }

        public void Freeze(bool value)
        {
            gameObject.SetActive(!value);
        }
    }

}
