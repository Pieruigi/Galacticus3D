using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB.AI
{
    public class FlybyMover : MonoBehaviour, IActivable, IRolleable
    {
        [SerializeField]
        float speed = 5f;

        [SerializeField]
        float angularSpeed = 10f;

        [SerializeField]
        bool useRigidbody;

        Rigidbody rb;

        bool isActive = false;

        TargetSetter targetSetter;
        bool flyAway = false;

        Vector3 targetDir;
        Transform root;

        void Awake()
        {
            root = transform.root;
            targetSetter = GetComponent<TargetSetter>();
            rb = root.GetComponent<Rigidbody>();
            Deactivate();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (!flyAway)
            {
                Vector3 dir = targetSetter.Target.position - root.position;
                targetDir = Vector3.RotateTowards(root.forward, dir, angularSpeed * Time.deltaTime, 0.0f);
            }

            if (!useRigidbody)
            {
                root.position += targetDir * speed * Time.deltaTime;
            }
        }

        void FixedUpdate()
        {
            if (useRigidbody)
            {
                rb.MovePosition(rb.position + targetDir * speed * Time.fixedDeltaTime);
            }
        }

        public void Activate()
        {
            isActive = true;
        }

        public void Deactivate()
        {
            isActive = false;
        }

        public bool IsActive()
        {
            return isActive;
        }

        public float GetMaxAngularSpeed()
        {
            return angularSpeed;
        }

        public float GetMaxSideSpeed()
        {
            return speed;
        }

        public bool IsAiming()
        {
            return false;
        }
    }

}
