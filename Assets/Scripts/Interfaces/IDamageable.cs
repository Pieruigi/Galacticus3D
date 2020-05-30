using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Interfaces
{
    public delegate void Die(IDamageable damageable);

    public interface IDamageable
    {
        event Die OnDie;

        void ApplyDamage(float amount);

    }

}
