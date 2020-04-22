using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AI : MonoBehaviour
{
    [SerializeField]
    Transform target;

    AIPath aiPath;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        aiPath.destination = target.position;
    }
}
