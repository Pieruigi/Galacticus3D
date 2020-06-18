﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class ParticleSystemScaler : MonoBehaviour
    {
        [SerializeField]
        ParticleSystemScalingMode scalingMode;

        [SerializeField]
        float scale = 1;
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        ParticleSystem ps;

        // Start is called before the first frame update
        void Start()
        {
            ps = GetComponent<ParticleSystem>();

        }

        // Update is called once per frame
        void Update()
        {
            var main = ps.main;
            main.scalingMode = scalingMode;//ParticleSystemScalingMode.Hierarchy;

            ps.transform.parent.localScale = scale * Vector3.one;
        }
    }

}
