#define RIGIDBODY
//#define USE_TORQUE
#if RIGIDBODY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace OMTB
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        [Range(1f, 20f)]
        float turningSpeed;

        [SerializeField]
        [Range(1f, 30f)]
        float maxSpeed;
        public float MaxSpeed
        {
            get { return maxSpeed; }
        }

        public float SqrMaxSpeed
        {
            get { return maxSpeed * maxSpeed; }
        }

        [SerializeField]
        [Range(1f, 30f)]
        float acceleration;

        [SerializeField]
        [Range(10f, 200f)]
        float deceleration;


        [SerializeField]
        [Range(0.1f, 1f)]
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

        Rigidbody rb;

        Shooter shooter;
        Vector3 currentSpeed;
        public Vector3 CurrentSpeed
        {
            get { return currentSpeed; }
        }

        bool notSteering = false;
        DateTime notSteeringLastTime;
        float notSteeringTime = 0;
        float notSteeringTimeBase = 1;

        bool isAiming = false;
        public bool IsAiming { get { return isAiming; } }


        // Start is called before the first frame update
        void Start()
        {
            maxSpeedSqr = maxSpeed * maxSpeed;
            rb = GetComponent<Rigidbody>();
            shooter = GetComponent<Shooter>();
        }


        // Update is called once per frame
        void Update()
        {
            if (notSteering)
            {
                if ((DateTime.UtcNow - notSteeringLastTime).TotalSeconds > notSteeringTime)
                    notSteering = false;
                else
                    return;
            }
                

            Vector2 axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            float mag = axis.magnitude;
            mag = Mathf.Clamp01(mag);
            
            float currSpeedMag = currentSpeed.magnitude;
            if (mag != 0) // Accelerate
            {
                // Set velocity
                currSpeedMag += acceleration * Time.deltaTime;

                if (currSpeedMag > maxSpeed)
                    currSpeedMag = maxSpeed;

                // Check direction
                targetDirection = new Vector3(axis.x, 0, axis.y).normalized;
            }
            else // Decelerate
            {
                currSpeedMag -= deceleration * Time.deltaTime;

                if (currSpeedMag < 0)
                    currSpeedMag = 0;

            }


            isAiming = false;
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

                    if ((System.DateTime.UtcNow - lastShootTime).TotalSeconds > 1 / fireRate)
                    {
                        lastShootTime = System.DateTime.UtcNow;
                        shooter.Shoot();
                    }
                    

                }

            }



            // Calculate a rotation a step closer to the target and applies rotation to this object
            Vector3 tmp = Vector3.RotateTowards(transform.forward, targetDirection, turningSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(tmp);

            // Move
            if (!isAiming)
                tmp = Vector3.RotateTowards(currentSpeed.sqrMagnitude == 0 ? transform.forward : currentSpeed.normalized, targetDirection.normalized, turningSpeed * stability * Time.deltaTime, .0f);
            else // Strafe
                tmp = Vector3.RotateTowards(currentSpeed.sqrMagnitude == 0 ? new Vector3(axis.x, 0, axis.y).normalized : currentSpeed.normalized, new Vector3(axis.x, 0, axis.y).normalized, turningSpeed * stability * Time.deltaTime, .0f);

            currentSpeed = tmp.normalized * currSpeedMag;
            rb.MovePosition(rb.position + currentSpeed * Time.deltaTime);

        }


        private void OnCollisionEnter(Collision collision)
        {
            if ("Wall".Equals(collision.gameObject.tag))
            {
                Vector3 totSpeed = currentSpeed + rb.velocity;
                Debug.Log("TotSpeed:" + totSpeed);

                float dot = Vector3.Dot(totSpeed, -collision.contacts[0].normal);
                Vector3 bounceDir;
                
                bounceDir = (collision.contacts[0].normal + totSpeed.normalized).normalized;
                if (bounceDir == Vector3.zero)
                    bounceDir = -totSpeed.normalized;
                
                Vector3 force1 = bounceDir * 250f * dot;
                Vector3 force2 = collision.contacts[0].normal * 1500f;

                rb.AddForce(force1, ForceMode.Impulse);
                rb.AddForce(force2, ForceMode.Impulse);

                float signedAngle = Vector3.SignedAngle(totSpeed, collision.contacts[0].normal, Vector3.up);

                rb.AddTorque(transform.up * -200 * totSpeed.magnitude * Mathf.Sign(signedAngle), ForceMode.Impulse);

                notSteering = true;
                notSteeringLastTime = DateTime.UtcNow;
                notSteeringTime = totSpeed.magnitude / 100f + notSteeringTimeBase;
                currentSpeed = Vector3.zero;
                return;
            }
        }


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
        [Range(1f, 20f)]
        float turningSpeed;

        [SerializeField]
        [Range(1f, 30f)]
        float maxSpeed;

        [SerializeField]
        [Range(1f, 30f)]
        float acceleration;

        [SerializeField]
        [Range(1f, 80f)]
        float deceleration;

        [SerializeField]
        [Range(0.1f, 0.9f)]
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
        Vector3 currentSpeed;

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
            //currentSpeed = characterController.velocity.magnitude;
            float currSpeedMag = currentSpeed.magnitude;
            if (mag != 0) // Accelerate
            {
                // Set velocity
                currSpeedMag += acceleration * Time.deltaTime;

                if (currSpeedMag > maxSpeed)
                    currSpeedMag = maxSpeed;

                // Check direction
                targetDirection = new Vector3(axis.x, 0, axis.y).normalized;


            }
            else // Decelerate
            {
                currSpeedMag -= deceleration * Time.deltaTime;

                if (currSpeedMag < 0)
                    currSpeedMag = 0;

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
            else // Strafe
                newVelocityDirection = Vector3.RotateTowards(characterController.velocity.sqrMagnitude == 0 ? new Vector3(axis.x, 0, axis.y).normalized : characterController.velocity.normalized, new Vector3(axis.x, 0, axis.y).normalized, turningSpeed * stability * Time.deltaTime, .0f);

            // Draw a ray pointing at our target in
            Debug.DrawRay(transform.position, newDirection, Color.red);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);

            // Move
            //Debug.Log("CurrentSpeed:" + currentSpeed);
            currentSpeed = newVelocityDirection.normalized * currSpeedMag;
            //characterController.Move(newVelocityDirection.normalized * currentSpeed * Time.deltaTime);
            characterController.Move(currentSpeed * Time.deltaTime);


            //Debug.Log("Current speed:" + currentSpeed);
        }
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {

            Bouncer bouncer = hit.gameObject.GetComponent<Bouncer>();
            Debug.Log("Collision");
            if (bouncer)
            {
                // Add impulse
                Vector3 impulse = hit.normal;
                impulse.y = 0;
                impulse = impulse.normalized * bouncer.Force;
                currentSpeed += impulse; // Use external force variable
                Debug.Log("Impulse:" + impulse);

            }
        }


    }

    
}



#endif