using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB.Utils
{
    public class AIUtil
    {
        private static float raycastVerticalDisplacement = 0f;

        public static bool IsOnSight(Transform source, Transform target)
        {

            bool onSight = false;
            RaycastHit hit;
            int mask = ~LayerMask.GetMask(new string[] { "Bullet" });
            Debug.DrawRay(source.position, target.position - source.position, Color.red, 3);
            if (Physics.Raycast(source.position, target.position - source.position, out hit, 1000, mask, QueryTriggerInteraction.Ignore))
            {

                if (hit.transform == target)
                    onSight = true;
            }
          
            return onSight;
        }

        public static bool HitObstacle(Vector3 position, Vector3 direction, float distance, out RaycastHit hit)
        {
            int mask = LayerMask.GetMask(new string[] { "Obstacle" });

            if (Physics.Raycast(position, direction, out hit, distance, mask, QueryTriggerInteraction.Ignore))
                return true;

            return false;
        }
    }

}
