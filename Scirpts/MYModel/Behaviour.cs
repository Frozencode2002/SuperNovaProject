using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanBody;
using Unity.VisualScripting;


namespace HumanBody
{
    public class Behaviour : MonoBehaviour
    {
        public CapMoveScript info;
        [SerializeField] private HumanBody.Ragdoll m_activeragdoll;
        [SerializeField] private PhysicsBody m_physicbody;
        [SerializeField] private AnimationBody m_animationbody;
        // [SerializeField] private Animator AnimationAnimator;
        [SerializeField] private Animator PhysicBodyAnimator;
        [SerializeField] private Transform m_camera;
        [SerializeField] public float DetectDistance = 7.5f;
        [SerializeField] public float CapsuleDistance = 7.5f;
        [SerializeField] public float FloorScope = 60f;
  
        private Rigidbody LeftFoot, RightFoot, HipsRigidbody;
        [SerializeField] public Rigidbody CapsuleBody;
        [SerializeField] private bool OnFloorFlag;
        public bool AffectFlag;
        [SerializeField] private bool EnableJump = true;

        private bool banCap = false;
        private Vector3 LastDirrection;

        public bool InAir;
        public float AccFlag;
        public Vector3 NewSpeed;

        [SerializeField] private CapMoveScript capsule;
        private void OnValidate()
        {
            if (m_activeragdoll == null) m_activeragdoll = GetComponent<HumanBody.Ragdoll>();
            if (m_physicbody == null) m_physicbody = GetComponent<PhysicsBody>();
            if (m_animationbody == null) m_animationbody = GetComponent<AnimationBody>();
        }

        private void Start()
        {
            // Physics.gravity *= 2;
            AffectFlag = true;
            OnFloorFlag = true;
            LeftFoot = PhysicBodyAnimator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).GetComponent<Rigidbody>();
            RightFoot = PhysicBodyAnimator.GetBoneTransform(HumanBodyBones.RightLowerLeg).GetComponent<Rigidbody>();
        }

        private void Update()
        {
            Debug.DrawLine(CapsuleBody.position, CapsuleBody.position + 5 * Vector3.down, Color.green);
            if (Time.timeScale == 0f) return;
            
            CheckRigidbodyOnFloor();
            UpdateMovement();
        }

        private void CheckRigidbodyOnFloor()
        {
            EnableJump = true;
            capsule.jumpFlag = true;
            
            Ray Leftray = new Ray(LeftFoot.position, Vector3.down);
            bool LeftOnFloor = Physics.Raycast(Leftray, out RaycastHit Leftinfo, DetectDistance, ~((1 << LeftFoot.gameObject.layer) | (1 << 7)));
            bool LeftAffect = Physics.Raycast(Leftray, out RaycastHit CapLeftinfo, CapsuleDistance, ~((1 << LeftFoot.gameObject.layer) | (1 << 7)));
            LeftOnFloor = LeftOnFloor && Vector3.Angle(Leftinfo.normal, Vector3.up) <= FloorScope;
            
            Ray Rightray = new Ray(RightFoot.position, Vector3.down);
            bool RightOnFloor = Physics.Raycast(Rightray, out RaycastHit Rightinfo, DetectDistance, ~((1 << RightFoot.gameObject.layer) | (1 << 7)));
            bool RightAffect = Physics.Raycast(Rightray, out RaycastHit CapRightinfo, CapsuleDistance, ~((1 << RightFoot.gameObject.layer) | (1 << 7)));
            RightOnFloor = RightOnFloor && Vector3.Angle(Rightinfo.normal, Vector3.up) <= FloorScope;
            
            OnFloorFlag = LeftOnFloor || RightOnFloor;
            AffectFlag = LeftAffect || RightAffect;
            
            if (banCap) // jumpflag: false时不会回正
            {
                capsule.jumpFlag = false;
            }
            else
            {
                capsule.jumpFlag = LeftAffect && RightAffect;
            }

            if (Leftinfo.distance < 3.5f && Rightinfo.distance < 3.5f)
            {
                EnableJump = true;
            }
            else
            {
                EnableJump = false;
            }

            if (OnFloorFlag && EnableJump && (Input.GetKeyDown(KeyCode.Space)))
            {
                CapsuleBody.AddForce(Vector3.up * 325f, ForceMode.Impulse);
                // CapsuleBody.AddForce(Vector3.up * 350f, ForceMode.Impulse);
                StartCoroutine(SkipOneSecond());
            }
        }

        private IEnumerator SkipOneSecond()
        {
            // capsule.jumpFlag = false;
            banCap = true; 
            yield return new WaitForSeconds(1f);
            banCap = false;
        }
/*
        private bool RayCheck(Rigidbody BodyPart)
        {
            Ray ray = new Ray(BodyPart.position, Vector3.down);
            // Debug.DrawLine(BodyPart.position, BodyPart.position + Vector3.down * DetectDistance, Color.yellow);
            // Debug.DrawLine(BodyPart.position, BodyPart.position + Vector3.down * CapsuleDistance, Color.black);
            bool OnFloor = Physics.Raycast(ray, out RaycastHit info, DetectDistance, ~((1 << BodyPart.gameObject.layer) | (1 << 7)));
            
            if (banCap) // jumpflag: false时不会回正
            {
                capsule.jumpFlag = false;
            }
            else
            {
                capsule.jumpFlag = Physics.Raycast(ray, out RaycastHit capinfo, CapsuleDistance, ~((1 << BodyPart.gameObject.layer) | (1 << 7)));
            }
            OnFloor = OnFloor && Vector3.Angle(info.normal, Vector3.up) <= FloorScope;
            if (info.distance < 2f)
            {
                EnableJump = true;
            }
            else
            {
                EnableJump = false;
            }
            return OnFloor;

        }
*/
        private void UpdateMovement()
        {
            if (Time.timeScale == 0f) return;
            
            float Vspeed = Input.GetAxis("Vertical");
            float Hspeed = Input.GetAxis("Horizontal");

            if (Input.GetKey(KeyCode.LeftShift) && !info.UpFlag && !info.SlowFlag)
            {
                AccFlag = 1.5f;
                m_animationbody.animator.SetFloat("speed", 1.05f);
            }
            else
            {
                AccFlag = 1f;
                m_animationbody.animator.SetFloat("speed", 0.75f);
            }
            
            Vector3 Camera = m_camera.rotation.eulerAngles;
            Vector3 Speed = new Vector3(Hspeed * AccFlag, 0, Vspeed * AccFlag);
            // Debug.Log("Speed:" + Speed);
            if (!OnFloorFlag)
            {
                InAir = true;
            }
            else
            {
                InAir = false;
            }
            // Debug.Log(Speed + " :V");
  
            if (OnFloorFlag && Speed.magnitude > 0.01f)
            {
                m_animationbody.animator.SetBool("moving", true);
            }
            else
            {
                m_animationbody.animator.SetBool("moving", false);
            }
            if (OnFloorFlag)
            {
                m_animationbody.animator.SetBool("onfloorflag", true);
            }
            else
            {
                m_animationbody.animator.SetBool("onfloorflag", false);
            }
            // m_animationbody.animator.SetBool("moving", Speed.magnitude > 0.01f);
            // m_animationbody.animator.SetFloat("speed", Speed.magnitude);

            Vector3 targetDirection = Quaternion.Euler(new Vector3(0, Camera.y, 0)) * Speed;
            NewSpeed = targetDirection;
            Debug.DrawLine(Vector3.zero, Vector3.zero + targetDirection * 1000f, Color.magenta);
            // m_physicbody.TargetDirection = targetDirection;
            
        }

    }
}
