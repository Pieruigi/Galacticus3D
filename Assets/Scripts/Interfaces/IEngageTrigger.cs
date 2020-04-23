using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Interfaces
{
    public delegate void TargetEngaged();
    public delegate void TargetDisengaged();

    public interface IEngageTrigger
    {
        event TargetEngaged OnTargetEngaged;
        event TargetDisengaged OnTargetDisengaged;

    }

}
