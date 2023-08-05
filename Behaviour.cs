using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HumanBody;


namespace HumanBody
{
    public class Behaviour : MonoBehaviour
    {
        
        [SerializeField] private HumanBody.Ragdoll m_activeragdoll;
        [SerializeField] private PhysicsBody m_physicbody;
        [SerializeField] private AnimationBody m_animationbody;
        [SerializeField] private Animator PhysicBodyAnimator;
        [SerializeField] private Transform m_camera;
        [SerializeField] public float DetectDistance = 0.5f;
        [SerializeField] public float FloorScope = 60f;
  
        private Rigidbody LeftFoot, RightFoot, HipsRigidbody;
        
        [SerializeField] private bool OnFloorFlag;
        private Vector3 LastDirrection;
        
        private void OnValidate()
        {
            if (m_activeragdoll == null) m_activeragdoll = GetComponent<HumanBody.Ragdoll>();
            if (m_physicbody == null) m_physicbody = GetComponent<PhysicsBody>();
            if (m_animationbody == null) m_animationbody = GetComponent<AnimationBody>();
        }

        private void Start()
        {
            OnFloorFlag = true;
            HipsRigidbody = PhysicBodyAnimator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
            LeftFoot = PhysicBodyAnimator.GetBoneTransform(HumanBodyBones.LeftFoot).GetComponent<Rigidbody>();
            RightFoot = PhysicBodyAnimator.GetBoneTransform(HumanBodyBones.RightFoot).GetComponent<Rigidbody>();
        }

        private void Update()
        {
            CheckRigidbodyOnFloor();
            UpdateMovement();
        }

        private void CheckRigidbodyOnFloor()
        {
            OnFloorFlag = RayCheck(LeftFoot) || RayCheck(RightFoot);
            if (OnFloorFlag && Input.GetKeyDown(KeyCode.Space))
            {
                HipsRigidbody.AddForce(Vector3.up * 100f, ForceMode.Impulse);
            }
        }

        private bool RayCheck(Rigidbody BodyPart)
        {
            Ray ray = new Ray(BodyPart.position, Vector3.down);
            bool OnFloor = Physics.Raycast(ray, out RaycastHit info, DetectDistance, ~(1 << BodyPart.gameObject.layer));

            OnFloor = OnFloor && Vector3.Angle(info.normal, Vector3.up) <= FloorScope;
            
            return OnFloor;

        }
        private void UpdateMovement()
        {
            
            float Vspeed = Input.GetAxis("Vertical");
            float Hspeed = Input.GetAxis("Horizontal");

            Vector3 Camera = m_camera.rotation.eulerAngles;
            Vector3 Speed = new Vector3(Hspeed * 1f, 0, Vspeed * 1f);

            if (!OnFloorFlag) Speed = Vector3.zero;
            // Debug.Log(Speed + " :V");
  
            if (Speed.magnitude > 0.01f)
            {
                m_animationbody.animator.SetBool("moving", true);
            }
            else
            {
                m_animationbody.animator.SetBool("moving", false);
            }
            m_animationbody.animator.SetBool("moving", Speed.magnitude > 0.01f);
            m_animationbody.animator.SetFloat("speed", Speed.magnitude);
            if (Speed.magnitude < 0.01f) return;

            Vector3 targetDirection = Quaternion.Euler(new Vector3(0, Camera.y, 0)) * Speed;
     
            Debug.DrawLine(Vector3.zero, Vector3.zero + targetDirection * 1000f, Color.magenta);
            m_physicbody.TargetDirection = targetDirection;
            
        }

    }
}
