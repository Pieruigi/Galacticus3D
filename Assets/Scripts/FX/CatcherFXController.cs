using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class CatcherFXController : MonoBehaviour
    {
        [SerializeField]
        Catcher catcher;

        [SerializeField]
        Animator animator;

        [SerializeField]
        List<ParticleSystem> particles;

        bool caught = false;
        
        private void Awake()
        {
            catcher.OnCatch += HandleOnCatch;
            catcher.OnUncatch += HandleOnUncatch;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        void HandleOnCatch()
        {
            caught = true;
            animator.SetBool("Catch", true);
        }

        void HandleOnUncatch()
        {
            caught = false;
            animator.SetBool("Catch", false);
            StopPS();
        }

        void PlayPS()
        {
            if (particles[0].isPlaying || !caught)
                return;

            foreach (ParticleSystem ps in particles)
                ps.Play();
        }

        void StopPS()
        {
            if (!particles[0].isPlaying)
                return;

            foreach (ParticleSystem ps in particles)
                ps.Stop();
        }


    }

}
