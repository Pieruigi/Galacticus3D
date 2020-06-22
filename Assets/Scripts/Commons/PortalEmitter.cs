using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Level;
using OMTB.Collections;

namespace OMTB
{
    public class PortalEmitter : MonoBehaviour
    {

        Portal portal;

        float dir = -1;
        float pulseSpeed = 1f;
        
        float time = 0;
        float maxEmitPower = 1.5f;
        float minEmitPower = 0.85f;

        GameObject graphics;

        // Start is called before the first frame update
        void Start()
        {
            portal = GetComponent<Portal>();
            graphics = portal.transform.GetChild(0).gameObject;
            SetColor(portal.TargetPortal.Room.RoomType, maxEmitPower);
        }

        // Update is called once per frame
        void Update()
        {
            if (!graphics.activeSelf)
                return;

            if (!portal.AlreadyUsed)
            {
                // Pulse
                time += pulseSpeed * Time.deltaTime;
                
                float emitPower = Mathf.Lerp(dir > 0 ? minEmitPower : maxEmitPower, dir > 0 ? maxEmitPower : minEmitPower, time);
                
                SetColor(portal.TargetPortal.Room.RoomType, emitPower);

                if(time >= 1 || time <= 0)
                {
                    time = 0;
                    dir *= -1;
                }

            }
        }

        void SetColor(RoomType roomType, float emitPower)
        {
            Material mat = GetComponentInChildren<MeshRenderer>().material;
            switch (roomType)
            {
                case RoomType.Boss:
                    mat.SetColor("_EmissionColor", emitPower * Color.red);
                    break;
                case RoomType.Bank:
                    mat.SetColor("_EmissionColor", emitPower * Color.yellow);
                    break;
                default:
                    mat.SetColor("_EmissionColor", emitPower * Color.cyan);
                    break;
            }
        }
    }

}
