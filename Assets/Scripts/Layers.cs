using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class Layers
    {

        // Used by the navmesh surface to check ground
        public static readonly string Ground = "Ground";

        // Used by the navmesh surface to check walls
        public static readonly string Obstacle = "Obstacle";

        // Used by bullets ( self collision disabled )
        public static readonly string Bullet = "Bullet";

        // Used to avoid collision with bullets
        public static readonly string NoBulletCollision = "NoBulletCollision";

        // Used by AI in raycast ( for example when aiming to exclude certain directions )
        public static readonly string AIAvoidance = "AIAvoidance";

        // Used by AI in raycast to enable/disable friendly fire
        public static readonly string NoFriendlyFire = "NoFriendlyFire";

        // Used for example by catchers to avoid player collision
        //public static readonly string NoDefaultCollision = "NoDefaultCollision";

    }

}
