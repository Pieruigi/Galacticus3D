using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Interfaces;

namespace OMTB
{
    public class Twister : MonoBehaviour
    {
        [SerializeField]
        float delay = 2.5f;

        bool isDestroying = false;

        float rotSpeed;
        Vector3 rotAxis;

        float time;
        float scaleDelay = 1.5f;

        private void Awake()
        {
            if (scaleDelay >= delay)
                scaleDelay = 0;
        }

        // Start is called before the first frame update
        void Start()
        {
            GetComponentInParent<IDamageable>().OnDie += HandleOnDie;
        }

        // Update is called once per frame
        void Update()
        {
            if (!isDestroying)
                return;

            
            time += Time.deltaTime;

            // Lerp rotation speed
            rotSpeed = Mathf.Lerp(0f, 720f, time / delay);

            // Rotate
            transform.Rotate(rotAxis, rotSpeed);

            // Scale
            float scale = Mathf.SmoothStep(1, 0.1f, time-scaleDelay / delay-scaleDelay);
            transform.localScale = transform.localScale * scale;
          
        }

        void HandleOnDie(IDamageable damageable)
        {
            isDestroying = true;

            // Ship destroying effect
            time = 0;
            rotAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        }
    }

}
