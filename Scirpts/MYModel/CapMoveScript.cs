using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace HumanBody
{
    public class CapMoveScript : MonoBehaviour
    {
        public Behaviour Info;
        public float RideHeight = 14.5f;
        public float DetectHeight = 30;
        public float RideSpringStrength = 500;
        public float RideSpringDamper = 50;
        public Quaternion OriginRotation;

        public Vector3 TargetDirection;
        private Quaternion TargetRotation;

        private Vector3 Vel;
        [SerializeField] private Vector3 groundVel;
        public float MaxSpeed = 20f;
        public float Acceleration = 100f;
        public AnimationCurve AccelerationFactorFromDot;
        public float MaxAccelForce = 75f;
        public AnimationCurve MaxAccelerationForceFactorFromDot;
        public Vector3 ForceScale = new Vector3(1, 0, 1);
        
        
        public ConfigurableJoint CapJoint;
        [SerializeField] private Rigidbody CapsuleRigid;
        [SerializeField] public float UprightJointSpringStrength = 1200f;
        [SerializeField] public float UprightJointSpringDamper = 50f;

        [SerializeField] private bool hitflag = false;

        [SerializeField] private int RotateBit;
        // slope
        public float maxInclineAngle = 45f; // 最大斜坡角度
        public float targetLeanAngle = 45f; // 目标倾斜角度
        public float rotationSpeed = 5f; // 旋转速度
        public float targetAngle = 0f;
        public bool UpFlag = false;
        public bool SlowFlag = false;

        public bool AbleMove = true;
        // respawn
        public float minYPosition = -120f;
        [SerializeField] private int currentRespawnIndex = 0;
        public Transform[] respawnPoints;
        
        // jump
        public bool jumpFlag = true;

        public bool respawnflag = true;
        void Start()
        {
            RotateBit = 0;
            Vel = Vector3.zero;
            groundVel = Vector3.zero;
            CapJoint = GetComponent<ConfigurableJoint>();
            // OriginRotation = transform.rotation;
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale == 0f) return;
            CapsuleRigid.AddForce(Vector3.down * 125f);
            UpdateHeight();
            UpdateDirection();
            
            // Debug.Log(transform.position.y);
            if (transform.position.y < minYPosition)
            {
                Respawn();
            }
            
            // UpdateUprightForce();
            if (!respawnflag)
            {
                respawnflag = true;
                // Debug.Log(CapsuleRigid.velocity + "now");
            }
        }
        
        void Respawn()
        {
            respawnflag = false;
            transform.position = respawnPoints[currentRespawnIndex].position;
            // Debug.Log(CapsuleRigid.velocity + "Before");
            CapsuleRigid.velocity = Vector3.zero;
            CapsuleRigid.angularVelocity = Vector3.zero;
            // Debug.Log(CapsuleRigid.velocity + "After");
            
        }

        public void UpdateRespawnPoint(int newRespawnIndex)
        {
            currentRespawnIndex = newRespawnIndex;
        }

        void UpdateHeight()
        {
            Ray ray = new Ray(CapsuleRigid.position + Vector3.up * DetectHeight * 0.5f, Vector3.down);
            bool Rayhit = Physics.Raycast(ray, out RaycastHit info, DetectHeight, ~((1 << CapsuleRigid.gameObject.layer) |
                (1 << 6) | (1 << 14) | (1 << 15))); // capsule and ragdoll and areabox and spinging baton
            AbleMove = true;
            RotateBit = 0;
            // Debug.Log(CapsuleRigid.position);
            Debug.DrawLine(CapsuleRigid.position + Vector3.up * DetectHeight * 0.5f, CapsuleRigid.position + Vector3.down * DetectHeight * 0.5f, Color.red, 5f);
            if (Rayhit)
            {
                //检测是否为斜坡，如果是斜坡需要调整人物的倾角
                
                float inclineAngle = Vector3.Angle(info.normal, Vector3.up);
                UpFlag = false;
                if (inclineAngle <= maxInclineAngle)
                {
                    targetAngle = Mathf.Lerp(0, targetLeanAngle, inclineAngle / maxInclineAngle);
                    // Debug.Log(info.normal + " " + targetAngle);
                    // Debug.Log(inclineAngle);
                    if (inclineAngle > 15f)
                    {
                        UpFlag = true;
                    }

                    if (targetAngle > 0f)
                    {
                        if (Math.Abs(info.normal.x) > 0)
                        {
                            if (info.normal.x > 0)
                            {
                                RotateBit = 3;
                            }
                            else
                            {
                                RotateBit = 2;
                            }
                             // rotateZ
                        }
                        else
                        {
                            if (info.normal.z > 0)
                            {
                                RotateBit = 4;
                            }
                            else
                            {
                                RotateBit = 1;
                            }
                        }
                    }
                }
                else
                {
                    AbleMove = false;
                    // Debug.Assert(false);
                }
                
                
                Vector3 vel = CapsuleRigid.velocity;
                Vector3 rayDir = Vector3.down;

                Vector3 otherVel = Vector3.zero;
                Rigidbody hitbody = info.rigidbody;
                
                // Debug.Log(LayerMask.LayerToName(info.collider.gameObject.layer));
                
                if (hitbody != null)
                {
                    hitflag = true;
                    if (LayerMask.LayerToName(info.collider.gameObject.layer) == "FloorDisc")
                    {
                        // Debug.Log("Hit!!");
                        float AngularSpeed = 60 * Mathf.Deg2Rad;

                        Vector3 radiusVector = info.point - info.collider.gameObject.transform.position;

                        Vector3 normalVector = Vector3.Cross(radiusVector, Vector3.up).normalized;

                        groundVel = normalVector * radiusVector.magnitude * AngularSpeed;
                    }
                    else
                    {
                        groundVel = Vector3.zero;
                    }
                    
                }
                else if (LayerMask.LayerToName(info.collider.gameObject.layer) == "StraightBelt")
                {
                    hitflag = true;
                    Vector3 HitVel = new Vector3(0, 0, 1);
                    float HitSpeed = 20f;
                    groundVel = HitVel * HitSpeed;
                }
                else if (LayerMask.LayerToName(info.collider.gameObject.layer) == "TstraightBelt")
                {
                    hitflag = true;
                    Vector3 HitVel = new Vector3(1, 0, 0);
                    float HitSpeed = 20f;
                    groundVel = HitVel * HitSpeed;
                }
                else if (LayerMask.LayerToName(info.collider.gameObject.layer) == "RTstraightBelt")
                {
                    hitflag = true;
                    Vector3 HitVel = new Vector3(-1, 0, 0);
                    float HitSpeed = 20f;
                    groundVel = HitVel * HitSpeed;
                }
                else
                {
                    hitflag = false;
                    groundVel = Vector3.zero;
                }

                if (!Info.AffectFlag) groundVel = Vector3.zero;
                
                float rayDirVel = Vector3.Dot(rayDir, vel);
                float otherDirVel = Vector3.Dot(rayDir, otherVel);

                float relVel = rayDirVel - otherDirVel;

                float x = info.distance - RideHeight;

                float springForce = (x * RideSpringStrength) - (relVel * RideSpringDamper);
                
                Debug.DrawLine(transform.position, transform.position + (rayDir * springForce), Color.green);

                if (!jumpFlag)
                {
                    
                    return;
                }

                CapsuleRigid.AddForce(rayDir * springForce);

                if (hitbody != null)
                {
                    hitbody.AddForceAtPosition(rayDir * -springForce, info.point);
                }
            }
        }

        void UpdateDirection()
        {
            TargetDirection = Info.NewSpeed;
            // Debug.Log("TransSpeed:" + TargetDirection);
            if (TargetDirection != Vector3.zero)
            {
                TargetRotation = Quaternion.LookRotation(TargetDirection, Vector3.up);
                // Debug.Log(TargetRotation.eulerAngles);
                // OriginRotation = TargetRotation;
                
                //此处需要判断是X轴转动还是Z轴转动
                if (RotateBit == 1)
                {
                    TargetRotation = Quaternion.Euler(targetAngle, TargetRotation.eulerAngles.y, 0);
                }
                else if (RotateBit == 2)
                {
                    // Debug.Log(targetAngle);
                    TargetRotation = Quaternion.Euler(0, TargetRotation.eulerAngles.y, targetAngle);
                }
                else if (RotateBit == 3)
                {
                    TargetRotation = Quaternion.Euler(0, TargetRotation.eulerAngles.y, -targetAngle);
                }
                else if (RotateBit == 4)
                {
                    TargetRotation = Quaternion.Euler(-targetAngle, TargetRotation.eulerAngles.y, 0);
                }
                else
                {
                    TargetRotation = Quaternion.Euler(0, TargetRotation.eulerAngles.y, 0);
                }
                CapJoint.targetRotation = Quaternion.Lerp(CapJoint.targetRotation, TargetRotation, Time.deltaTime * rotationSpeed);
                //
                
                // CapJoint.targetRotation = TargetRotation; **
            }
            else
            {
                TargetRotation = Quaternion.identity;
                // OriginRotation = TargetRotation;
            }

            if (Info.InAir) return;
            // Debug.Log(TargetDirection);
            
            // CapsuleRigid.AddForce(TargetDirection, ForceMode.Impulse);
            TargetDirection.Normalize();

            Vector3 unitVel = TargetDirection;

            float velDot = Vector3.Dot(unitVel, TargetDirection);

            float accel = Acceleration * AccelerationFactorFromDot.Evaluate(velDot);
            
        
            
            Vector3 goalVel = TargetDirection * MaxSpeed * Info.AccFlag;

            Vel = Vector3.MoveTowards(Vel, goalVel - groundVel, accel * 0.02f);

            // Debug.Log(Vel + " $$ " + goalVel + groundVel);
            
            Vector3 neededAccel = (Vel - CapsuleRigid.velocity) / 0.02f;

            float maxAccel = MaxAccelForce * MaxAccelerationForceFactorFromDot.Evaluate(velDot);

            neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
            
            // Debug.Log("##:" + Vector3.Scale(neededAccel * CapsuleRigid.mass, ForceScale));

            if (AbleMove)
            {
                CapsuleRigid.AddForce(Vector3.Scale(neededAccel * CapsuleRigid.mass, ForceScale));
            }
        }
        
        void UpdateUprightForce()
        {
            Quaternion characterCurrent = transform.rotation;

            var nowVec = characterCurrent.eulerAngles;
            // Debug.Log("Cur: " + nowVec);
            var newVec = new Vector3(nowVec.x, 0, nowVec.z);
            characterCurrent = Quaternion.Euler(newVec);


            var initVec = OriginRotation.eulerAngles;
            // Debug.Log("Init: " + initVec);
            var newInitVec = new Vector3(initVec.x, 0, initVec.z);
            var initQuater = Quaternion.Euler(newInitVec);
            
            Quaternion toGoal = Quaternion.Inverse(characterCurrent) * initQuater;
            
            // Quaternion toGoal = Quaternion.Inverse(characterCurrent) * OriginRotation;
            // Debug.Assert(characterCurrent * toGoal == OriginRotation);
            // Debug.Log("Det: " + toGoal.eulerAngles);
            
            Vector3 rotAxis;
            float rotDegrees;

            toGoal.Normalize();
            
            toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
            // Debug.Log(rotAxis + " ## " + rotDegrees);
            rotAxis.Normalize();
            // if (rotDegrees > 180) rotDegrees = -(360 - rotDegrees);
            float rotRadians = rotDegrees * Mathf.Deg2Rad;
            // Debug.Log("Torque: " + ((rotAxis * (rotRadians * UprightJointSpringStrength)) - (CapsuleRigid.angularVelocity * UprightJointSpringDamper)));
            CapsuleRigid.AddTorque((rotAxis * (rotRadians * UprightJointSpringStrength)) - (CapsuleRigid.angularVelocity * UprightJointSpringDamper));

        }
    }

}
