using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class DamagerEmitter : Emitter
    {


        // Start is called before the first frame update
        protected override void Start()
        {
            Owner = GetComponent<Damager>().Owner;
            base.Start();
        }

    }

}
