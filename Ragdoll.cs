using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HumanBody
{
    public class Ragdoll : MonoBehaviour
    {
        [SerializeField] private Transform AnimatorBody;
        [SerializeField] private Rigidbody PhysicBody;
        [SerializeField] public int BasicSpring;
        [SerializeField] public int BasicDamper;
        [SerializeField] public int HipsSpring;
        [SerializeField] public int HipsDamper;
        [SerializeField] public int LegsSpring;
        [SerializeField] public int LegsDamper;
        public Transform[] AnimatorBones { get; private set; }
        public Rigidbody[] PhysicBones { get; private set; }
        public ConfigurableJoint[] Joints { get; private set; }

        [SerializeField] public Animator AnimationAnimator;
        [SerializeField] public Animator PhysicAnimator;

        public Animator AnimatedAnimator
        {
            get
            {
                return AnimationAnimator;
            }
            private set
            {
                AnimatedAnimator = value;
            }
        }
        private void OnValidate()
        {
            var Animator = GetComponentsInChildren<Animator>();
            if (Animator.Length == 2)
            {
                if (AnimationAnimator == null) AnimationAnimator = Animator[0];
                if (PhysicAnimator == null) PhysicAnimator = Animator[1];
                if (AnimatorBody == null) AnimatorBody = AnimationAnimator.GetBoneTransform(HumanBodyBones.Hips);
                if (PhysicBody == null)
                    PhysicBody = PhysicAnimator.GetBoneTransform(HumanBodyBones.Hips).GetComponent<Rigidbody>();
            }
        }

        private void Awake()
        {
            if (AnimatorBones == null) AnimatorBones = AnimatorBody?.GetComponentsInChildren<Transform>();
            if (PhysicBones == null) PhysicBones = PhysicBody?.GetComponentsInChildren<Rigidbody>();
            if (Joints == null) Joints = PhysicBody?.GetComponentsInChildren<ConfigurableJoint>();

            for (int i = 0; i < Joints.Length; i++)
            {

                JointDrive TmpAngularDrive = Joints[i].angularXDrive;
                TmpAngularDrive.positionSpring = BasicSpring;
                TmpAngularDrive.positionDamper = BasicDamper;
                Joints[i].angularXDrive = TmpAngularDrive;
                Joints[i].angularYZDrive = TmpAngularDrive;
            }

            // modify hips
            ConfigurableJoint HipsJoint = PhysicAnimator?.GetBoneTransform(HumanBodyBones.Hips).GetComponent<ConfigurableJoint>();
            JointDrive tmpJoint = HipsJoint.angularXDrive;
            tmpJoint.positionSpring = HipsSpring;
            tmpJoint.positionDamper = HipsDamper;
            HipsJoint.angularXDrive = tmpJoint;
            HipsJoint.angularYZDrive = tmpJoint;
            
            // modify legs
            ConfigurableJoint LegsJoint = PhysicAnimator?.GetBoneTransform(HumanBodyBones.LeftFoot).GetComponent<ConfigurableJoint>();
            tmpJoint = LegsJoint.angularXDrive;
            tmpJoint.positionSpring = LegsSpring;
            tmpJoint.positionDamper = LegsDamper;
            LegsJoint.angularXDrive = tmpJoint;
            LegsJoint.angularYZDrive = tmpJoint;
            
            LegsJoint = PhysicAnimator?.GetBoneTransform(HumanBodyBones.RightFoot).GetComponent<ConfigurableJoint>();
            tmpJoint = LegsJoint.angularXDrive;
            tmpJoint.positionSpring = LegsSpring;
            tmpJoint.positionDamper = LegsDamper;
            LegsJoint.angularXDrive = tmpJoint;
            LegsJoint.angularYZDrive = tmpJoint;
        }

    }
}
