using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OMTB
{
    public class TargetSetter : MonoBehaviour
    {
        UnityAction<Transform> OnTargetSet;

        [SerializeField]
        Transform target;
        public Transform Target
        {
            get { return target; }
            set { target = value; OnTargetSet?.Invoke(target); }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
