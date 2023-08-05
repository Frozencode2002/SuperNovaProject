using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HumanBody
{
    public class AnimationBody : Model
    {
        private ConfigurableJoint[] AnimatorJoints;
        private Transform[] AnimatedBones;
        private Quaternion[] StartJointRotation;
        public Animator animator { get; private set; }
        
        private void Start()
        {
            AnimatorJoints = TmpActiveRagdoll.Joints;
            AnimatedBones = TmpActiveRagdoll.AnimatorBones;
            animator = TmpActiveRagdoll.AnimatedAnimator;
            StartJointRotation = new Quaternion[AnimatorJoints.Length];
            for (int i = 0; i < AnimatorJoints.Length; i++)
            {
                StartJointRotation[i] = AnimatorJoints[i].transform.localRotation;
            }
        }
        private void FixedUpdate()
        {
            UpdateJointsTarget();
        }

        void UpdateJointsTarget()
        {
            // Debug.Log(AnimatorJoints.Length);
            
            
            for (int i = 0; i < AnimatorJoints.Length; i++)
            {
                // ConfigurableJointExtensions.SetTargetRotationLocal(AnimatorJoints[i], AnimatedBones[i].localRotation, StartJointRotation[i]);
                // Debug.Log(AnimatedBones[0].gameObject.name);
                // Debug.Log(AnimatedBones[i].gameObject.name);
                // Debug.Assert(AnimatedBones[0].Find(AnimatorJoints[i].gameObject.name) != null);
               
                if (i == 0)
                {
                    // ConfigurableJointExtensions.SetTargetRotationLocal(AnimatorJoints[i], AnimatedBones[i].localRotation, StartJointRotation[i]);
                }
                else
                {
                    ConfigurableJointExtensions.SetTargetRotationLocal(AnimatorJoints[i], 
                        AnimatedBones[0].FindDeepChild(AnimatorJoints[i].gameObject.name).localRotation, 
                        StartJointRotation[i]);
                }
            }
        }
    }
}
