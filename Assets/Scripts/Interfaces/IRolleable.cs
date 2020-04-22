using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Interfaces
{
    public interface IRolleable
    {

        float GetMaxAngularSpeed();

        float GetMaxSideSpeed();

        bool IsAiming();

    }

}
