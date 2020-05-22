using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class _testNavMeshSurface : MonoBehaviour
{

    NavMeshSurface surf;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GetComponent<NavMeshSurface>().BuildNavMesh();
        }
    }
}
