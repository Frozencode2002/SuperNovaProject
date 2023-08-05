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

        private Quaternion TargetRotation;

        private void Start()
        {
            HipsRigidbody = TmpActiveRagdoll.PhysicAnimator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
            Hips = TmpActiveRagdoll.PhysicAnimator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<ConfigurableJoint>();
        }

        private void Update()
        {
    
            UpdateDirection();
            
            HipsRigidbody.AddForce(TargetDirection * 10f, ForceMode.Acceleration);
            // Debug.Log(Hips.rotation.eulerAngles + " Before");
            Hips.targetRotation = TargetRotation;
             // Debug.Log(Hips.rotation.eulerAngles + " After");
             // Debug.Log(Hips.rotation + " ## " + TmpActiveRagdoll.PhysicAnimator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>().rotation);
       
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
