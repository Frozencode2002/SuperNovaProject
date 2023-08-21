using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace HumanBody
{
    public class FootController : MonoBehaviour
    {
        static Vector3[] CastOnSurface(Vector3 point, float halfRange, Vector3 up)
        {
            Vector3[] res = new Vector3[2];
            RaycastHit hit;
            Ray ray = new Ray(new Vector3(point.x, point.y + halfRange, point.z), -up);

            if (Physics.Raycast(ray, out hit, 2f * halfRange))
            {
                res[0] = hit.point;
                res[1] = hit.normal;
            }
            else
            {
                res[0] = point;
            }
            return res;
        }

        public Behaviour Info;
        [SerializeField] private Vector3 Velocity;
        public float AccSpeed;
        private Vector3 LastVelocity;
        
        public Animator AnimationAnimator;

        public Animator PhysicsAnimator;

        public float smoothness = 2f;
        public float stepHeight = 0.2f;
        public float stepLength = 1f;
        public float angularSpeed = 0.1f;
        public float bounceAmplitude = 0.05f;
        public bool running = false;
        
        public Transform pivot;
        public Transform scaler;
        
        private Transform PhysicsLeftFoot;

        private Transform PhysicsRightFoot;
        
        [SerializeField] private Transform LeftFootIKTarget;

        [SerializeField] private Transform RightFootIKTarget;
        
        [SerializeField] private Transform LeftFootIKTargetRig;

        [SerializeField] private Transform RightFootIKTargetRig;
        
        private Vector3 initBodyPos;

        private Vector3 velocity;
        private Vector3 lastVelocity;
        
        private void Start()
        {
            PhysicsLeftFoot = PhysicsAnimator.GetBoneTransform(HumanBodyBones.LeftFoot);
            PhysicsRightFoot = PhysicsAnimator.GetBoneTransform(HumanBodyBones.RightFoot);
            
            initBodyPos = transform.localPosition;
        }

        private void FixedUpdate()
        {
            
            // Debug.Log(Velocity + " " + AccSpeed);
            Velocity = Info.NewSpeed;
            
            AccSpeed = Info.AccFlag;
            
            
            
            if (AccSpeed == 2f) running = true;
            Velocity *= AccSpeed;

            Velocity = (Velocity + smoothness * LastVelocity) / (1f + smoothness);
            
            LastVelocity = Velocity;
/*
            scaler.localScale = new Vector3(scaler.localScale.x, stepHeight * 2f * Velocity.magnitude,
                stepLength * Velocity.magnitude);
*/          
            scaler.rotation = quaternion.LookRotation(Velocity.normalized, Vector3.up);
            
            pivot.Rotate(Vector3.right,angularSpeed, Space.Self);

            Vector3 desiredPositionLeft = LeftFootIKTarget.position;
            Vector3 desiredPositionRight = RightFootIKTarget.position;

            
            
            Vector3[] posNormLeft = CastOnSurface(PhysicsLeftFoot.position, 2f, Vector3.up);

            if (posNormLeft[0].y > desiredPositionLeft.y)
            {
                LeftFootIKTargetRig.position = posNormLeft[0];
            }
            else
            {
                LeftFootIKTargetRig.position = desiredPositionLeft;
            }

            if (posNormLeft[1] != Vector3.zero)
            {
                LeftFootIKTargetRig.rotation = Quaternion.LookRotation(Velocity.normalized, posNormLeft[1]);
            }
            
            Vector3[] posNormRight = CastOnSurface(PhysicsRightFoot.position, 2f, Vector3.up);

            if (posNormRight[0].y > desiredPositionRight.y)
            {
                RightFootIKTargetRig.position = posNormRight[0];
            }
            else
            {
                RightFootIKTargetRig.position = desiredPositionRight;
            }

            if (posNormRight[1] != Vector3.zero)
            {
                RightFootIKTargetRig.rotation = Quaternion.LookRotation(Velocity.normalized, posNormRight[1]);
            }
            
            float feetDistance = Mathf.Clamp01(Mathf.Abs(LeftFootIKTargetRig.localPosition.z - RightFootIKTargetRig.localPosition.z) / (stepLength / 4f));

            float heightReduction = (running ? Mathf.Clamp01(velocity.magnitude) * bounceAmplitude - bounceAmplitude * Mathf.Clamp(velocity.magnitude, 0f, 10f) * feetDistance : bounceAmplitude * Mathf.Clamp(velocity.magnitude, 0f, 10f) * feetDistance);
            transform.localPosition = initBodyPos - heightReduction * Vector3.up;
            scaler.localPosition = new Vector3(0f, heightReduction, 0f);
            
        }
    }
}
