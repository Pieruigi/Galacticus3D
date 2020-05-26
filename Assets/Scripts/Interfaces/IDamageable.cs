using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Interfaces
{
    public delegate void Destroy(IDamageable damageable);

    public interface IDamageable
    {
        event Destroy OnDestroy;

        void ApplyDamage(float amount);

    }

}
