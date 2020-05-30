using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Interfaces
{
    public interface IPickable
    {
        bool TryPickUp(); // Passing player as param I avoid to check for player on every pickable

    }
}

