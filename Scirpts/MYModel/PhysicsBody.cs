using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Input = UnityEngine.Windows.Input;

namespace HumanBody
{
    
    
    public class PhysicsBody : Model
    {
        private Rigidbody HipsRigidbody;
        private ConfigurableJoint Hips;
        
        private float RotateSpeed = 2f;
        
        private Rigidbody Physicbody;

        private GameObject Man;
        public Vector3 TargetDirection
        {
            get;
            set;
        }

        private Vector3 LastDirection;
        
        private Quaternion TargetRotation;
        
        
        private void Start()
        {
            LastDirection = Vector3.zero;
            HipsRigidbody = TmpActiveRagdoll.PhysicAnimator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
            Hips = TmpActiveRagdoll.PhysicAnimator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<ConfigurableJoint>();
        }

        private void Update()
        {
    /*
            UpdateDirection();
            Hips.targetRotation = TargetRotation;
            
            // move logic
            
            
            HipsRigidbody.AddForce(TargetDirection * 2f, ForceMode.Acceleration);
            // Debug.Log(Hips.rotation.eulerAngles + " Before");
            
            if (TargetDirection == Vector3.zero && LastDirection != Vector3.zero)
            {
                HipsRigidbody.AddForce(LastDirection * -2f, ForceMode.Acceleration);
            }

            LastDirection = TargetDirection;
    */
        }

        void UpdateDirection()
        {
            if (TargetDirection != Vector3.zero)
            {
                TargetRotation = Quaternion.LookRotation(TargetDirection, Vector3.up);
            }
            else
            {
                TargetRotation = Quaternion.identity;
            }
        }
    }
}
