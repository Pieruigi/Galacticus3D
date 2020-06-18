using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Level;

namespace OMTB
{
    public class PortalEmitter : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {
            SetColor(GetComponent<Portal>().Room.RoomType);
        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetColor(RoomType roomType)
        {
            Material mat = GetComponentInChildren<MeshRenderer>().material;
            switch (roomType)
            {
                case RoomType.Boss:
                    mat.SetColor("_EmissionColor", 2 * Color.red);
                    break;
                case RoomType.Bank:
                    mat.SetColor("_EmissionColor", 2 * Color.yellow);
                    break;
                default:
                    mat.SetColor("_EmissionColor", 2 * Color.cyan);
                    break;
            }
        }
    }

}
