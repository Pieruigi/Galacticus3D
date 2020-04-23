using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Utils
{
    public class AIUtil
    {
     
        public static bool IsOnSight(Transform source, Transform target)
        {

            bool onSight = false;
            RaycastHit hit;
            Debug.DrawRay(source.position, target.position - source.position, Color.red, 3);
            if (Physics.Raycast(source.position, target.position - source.position, out hit, 1000, ~0, QueryTriggerInteraction.Ignore))
            {
                Debug.Log("Hit:" + hit.transform);
                if (hit.transform == target)
                    onSight = true;
            }

            return onSight;
        }
    }

}
