using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _TestDestroyShip : MonoBehaviour
{
    [SerializeField]
    GameObject ship;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ship.GetComponent<OMTB.Interfaces.IDamageable>().ApplyDamage(1000);
        }
    }
}
