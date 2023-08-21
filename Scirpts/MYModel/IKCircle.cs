using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HumanBody
{
    public class IKCircle : MonoBehaviour
    {
        public Transform LeftLeg;

        public Transform RightLeg;

        public Vector3 CircleCenter;

        public float radius;

        private float angle;
        // Start is called before the first frame update
        void Start()
        {
            CircleCenter = (LeftLeg.position + RightLeg.position) / 2;
        }

        // Update is called once per frame
        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(CircleCenter, radius * 0.01f);
        }

        void Update()
        {
            
            
            Vector3 pointDirection = (LeftLeg.position - CircleCenter).normalized;

            radius = (LeftLeg.position - CircleCenter).magnitude;
            
            float angle = Mathf.Atan2(pointDirection.y, pointDirection.z);

            Vector3 NxtPosition = new Vector3(RightLeg.position.x,
                CircleCenter.y + radius * Mathf.Sin(angle + Mathf.PI),
                CircleCenter.z + radius * Mathf.Cos(angle + Mathf.PI));

            RightLeg.position = NxtPosition;
        }
    }
}
