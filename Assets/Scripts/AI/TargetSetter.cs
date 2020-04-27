using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace OMTB
{
    public class TargetSetter : MonoBehaviour
    {
        public UnityAction<Transform> OnTargetChanged;

        [SerializeField]
        Transform target;
        public Transform Target
        {
            get { return target; }
            set { target = value; OnTargetChanged?.Invoke(target); }
        }
        // Start is called before the first frame update
        void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Target = player.transform;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
