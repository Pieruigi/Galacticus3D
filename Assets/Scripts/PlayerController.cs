//#define RIGIDBODY
#if RIGIDBODY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float force = 10;

    [SerializeField]
    float turningSpeed;

    [SerializeField]
    float maxVelocity = 5;

    float maxVelocitySqr;

    Rigidbody rb;
    Vector3 targetDirection;


    bool isGamepadConnected = false;


    // Start is called before the first frame update
    void Start()
    {
        maxVelocitySqr = maxVelocity * maxVelocity;
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turningSpeed*Time.deltaTime, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Check input
        Vector2 axis = new Vector2( Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float mag = axis.magnitude;
        mag = Mathf.Clamp01(mag);
        if (mag != 0)
        {
            
            rb.AddForce(transform.forward * force * mag, ForceMode.Force);

            // Check direction
            targetDirection = new Vector3(axis.x, 0, axis.y).normalized;
        }

        // Using mouse to set target direction ????
        if (!isGamepadConnected)
        {
            Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
            playerPos.z = 0;
            Debug.Log("ScreenPoint:" + playerPos);
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Debug.Log("MousePos:" + mousePos);
            Vector3 dir = (mousePos - playerPos).normalized;
            dir.z = dir.y;
            dir.y = 0;
            // Check direction
            targetDirection = dir;
        }

        if (rb.velocity.sqrMagnitude > maxVelocitySqr)
            rb.velocity = rb.velocity.normalized * maxVelocity;

        Debug.Log("Velocity:" + rb.velocity.magnitude);
    }
}
#endif

#if !RIGIDBODY



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OMTB
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        [Range(3f, 8f)]
        float turningSpeed;

        [SerializeField]
        [Range(4f, 12f)]
        float maxSpeed;

        [SerializeField]
        [Range(6f, 14f)]
        float acceleration;

        [SerializeField]
        [Range(4f, 10f)]
        float deceleration;

        [SerializeField]
        [Range(0.25f, 1f)]
        float stability; // Lateral velocity

        [Header("****Move to Weapon class")]
        [SerializeField] // Movo to weapon
        [Range(2f, 8f)]
        float fireRate;
        System.DateTime lastShootTime;

        float maxSpeedSqr;
        Vector3 targetDirection;

        [SerializeField]
        bool isGamepadConnected = false;
        [SerializeField]
        bool mouseHasGamepadBehavior = true;

        CharacterController characterController;

        Shooter shooter;

        // Start is called before the first frame update
        void Start()
        {
            maxSpeedSqr = maxSpeed * maxSpeed;
            characterController = GetComponent<CharacterController>();
            shooter = GetComponent<Shooter>();
        }


        // Update is called once per frame
        void Update()
        {
            Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            float mag = axis.magnitude;
            mag = Mathf.Clamp01(mag);
            float currentSpeed = characterController.velocity.magnitude;
            if (mag != 0) // Accelerate
            {
                // Set velocity
                currentSpeed += acceleration * Time.deltaTime;

                if (currentSpeed > maxSpeed)
                    currentSpeed = maxSpeed;

                // Check direction
                targetDirection = new Vector3(axis.x, 0, axis.y).normalized;


            }
            else // Decelerate
            {
                currentSpeed -= deceleration * Time.deltaTime;

                if (currentSpeed < 0)
                    currentSpeed = 0;

            }


            bool isAiming = false;
            // If I'm not using the gamepad the direction will be controlled via mouse and aiming is true
            if (!isGamepadConnected)
            {
                if (!mouseHasGamepadBehavior)
                {
                    Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
                    playerPos.z = 0;

                    Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

                    Vector3 dir = (mousePos - playerPos).normalized;
                    dir.z = dir.y;
                    dir.y = 0;
                    // Check direction
                    targetDirection = dir;
                }



                if (Input.GetMouseButton(0))
                {
                    if (mouseHasGamepadBehavior)
                    {
                        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
                        playerPos.z = 0;

                        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);

                        Vector3 dir = (mousePos - playerPos).normalized;
                        dir.z = dir.y;
                        dir.y = 0;
                        // Check direction
                        targetDirection = dir;
                    }
                    
                    isAiming = true;
                    if ((System.DateTime.UtcNow - lastShootTime).TotalSeconds > 1 / fireRate)
                    {
                        lastShootTime = System.DateTime.UtcNow;
                        shooter.Shoot();
                    }
                }
                
            }
            else // Check if I'm aiming with the right gamepad stick
            {

                Vector2 aimAxis = new Vector2(Input.GetAxis("AimHorizontal"), Input.GetAxis("AimVertical"));
                if (aimAxis != Vector2.zero)
                {
                    isAiming = true;

                    targetDirection.x = aimAxis.x;
                    targetDirection.z = aimAxis.y;

                    if((System.DateTime.UtcNow - lastShootTime).TotalSeconds > 1 / fireRate)
                    {
                        lastShootTime = System.DateTime.UtcNow;
                        shooter.Shoot();
                    }
                    

                }

            }



            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, turningSpeed * Time.deltaTime, 0.0f);

            Vector3 newVelocityDirection;
            if (!isAiming)
                newVelocityDirection = Vector3.RotateTowards(characterController.velocity.sqrMagnitude == 0 ? transform.forward : characterController.velocity.normalized, targetDirection.normalized, turningSpeed * stability * Time.deltaTime, .0f);
            else
                newVelocityDirection = Vector3.RotateTowards(characterController.velocity.sqrMagnitude == 0 ? new Vector3(axis.x, 0, axis.y).normalized : characterController.velocity.normalized, new Vector3(axis.x, 0, axis.y).normalized, turningSpeed * stability * Time.deltaTime, .0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);

            // Move
            Debug.Log("VelocityMag:" + newVelocityDirection.magnitude);
            Debug.Log("CurrentSpeed:" + currentSpeed);
            characterController.Move(newVelocityDirection.normalized * currentSpeed * Time.deltaTime);


            //Debug.Log("Current speed:" + currentSpeed);
        }


    }
}



#endif