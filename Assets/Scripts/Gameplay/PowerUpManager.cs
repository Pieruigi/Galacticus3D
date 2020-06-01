using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OMTB.Collections;
using OMTB.Level;


namespace OMTB.Gameplay
{

    public class PowerUpManager : MonoBehaviour
    {
        public static PowerUpManager Instance { get; private set; }

        GameObject player;
        
        PowerUp current;

        Picker pickerPrefab;

        [SerializeField]
        PowerUp test;



        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                Instance.pickerPrefab = Resources.LoadAll<Picker>("Misc")[0];
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");

            Debug.Log("PP:" + player.GetComponent<Health>());
        }

        // Update is called once per frame
        void Update()
        {

        }

        public bool TryActivate(PowerUp powerUp)
        {
            
            if(current != null)
            {
                ReleaseCurrentPowerUp();
                current.Deactivate();
            }
            
            current = powerUp;

            // Avoid the power up to get destroyed with the picker
            current.transform.parent = transform;

            powerUp.Activate();

            return true;
        }

        void ReleaseCurrentPowerUp()
        {
            // Get position to release

            Ray ray = new Ray(player.transform.position, Vector3.right);
            //float distance = Level.LevelManager.Instance.TileSize;
            float distance = 8;
            Vector3 dropPos = player.transform.position;

            //RaycastHit[] hits = Physics.RaycastAll(ray, distance);
            //if (hits.Length > 0 )
            //{
            //    for(int i=0; i<hits.Length; i++)
            //        Debug.Log("Hits:" + hits[i].transform);
            //}

            int mask = ~LayerMask.NameToLayer("Obstacle");

            if (!Physics.Raycast(ray, distance, mask))
            {
                Debug.Log("Drop right");
                dropPos = player.transform.position + Vector3.right * distance;
            }
            else
            {
                ray = new Ray(player.transform.position, Vector3.left);
                if (!Physics.Raycast(ray, distance, mask))
                {
                    Debug.Log("Drop left");
                    dropPos = player.transform.position + Vector3.left * distance;
                }
                else
                {
                    ray = new Ray(player.transform.position, Vector3.up);
                    if (!Physics.Raycast(ray, distance, mask))
                    {
                        Debug.Log("Drop up");
                        dropPos = player.transform.position + Vector3.up * distance;
                    }
                    else
                    {
                        ray = new Ray(player.transform.position, Vector3.down);
                        if (!Physics.Raycast(ray, distance, mask))
                        {
                            Debug.Log("Drop down");
                            dropPos = player.transform.position + Vector3.down * distance;
                        }
                    }
                }
            }

            DropCurrent(dropPos);
            
            Debug.Log("TO-DO: releasing power up");
            

        }

        void DropCurrent(Vector3 position)
        {
            GameObject picker = GameObject.Instantiate(pickerPrefab.gameObject);

            // Dropper is used on AI, so I'm sure RoomReference component exists
            //picker.AddComponent<RoomReferer>().Reference = LevelManager.Instance.CurrentRoom;

            picker.transform.position = position;
            picker.transform.rotation = Quaternion.identity;

            // Set droppable
            //picker.GetComponent<Picker>().StartDelay = 5; // A bit of delay
            picker.GetComponent<Picker>().SetContent(current.Droppable.Prefab);
            
        }
    }

}
