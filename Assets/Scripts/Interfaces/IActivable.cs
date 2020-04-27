using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Interfaces
{
    //public delegate void Activated();
    //public delegate void Deactivated();

    public interface IActivable
    {
        //event Activated OnActivated;
        //event Activated OnDeactivated;

        void Activate();

        void Deactivate();

        bool IsActive();
    }

}
